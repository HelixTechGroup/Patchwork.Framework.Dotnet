#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Manager;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Threading;
using Shield.Framework.IoC.Native.DependencyInjection;
using Shield.Framework.Threading;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.IoC.DependencyInjection;
using Shin.Framework.Logging.Native;
using SysEnv = System.Environment;
#endregion

namespace Patchwork.Framework
{
    public delegate void ProcessMessageHandler(IPlatformMessage message);

    public static partial class Core
    {
        #region Events
        public static event Func<OsType> DetectUnixSystemType;
        public static event ProcessMessageHandler ProcessMessage;

        public static event Action Shutdown;

        public static event Action Startup;
        #endregion

        #region Members
        //private static PlatformManager m_platform;
        private static CancellationTokenSource m_tokenSource;
        //private static  CancellationToken m_token;
        //private static IContainer m_container;
        //private static ConcurrentDictionary<Type, Lazy<IPlatformManager>> m_managers;
        private static Task m_runTask;
        private static IList<Task> m_tasks;
        #endregion

        #region Properties
        public static INApplication Application { get; private set; }

        public static INThreadDispatcher Dispatcher { get; private set; }

        public static IPlatformEnvironment Environment { get; private set; }

        public static IContainer IoCContainer { get; private set; }

        public static bool IsCreated { get; private set; }

        public static bool IsInitialized { get; private set; }

        public static bool IsRunning { get; private set; }

        public static ILogger Logger { get; private set; }

        public static IEnumerable<IPlatformManager> Managers
        {
            get { return IoCContainer.ResolveAll<IPlatformManager>(); }
        }

        public static IPlatformMessagePump MessagePump { get; private set; }
        #endregion

        #region Methods
        //public static void Initialize()
        //{
        //    Initialize(new CancellationToken());
        //}

        public static void Initialize()
        {
            if (!IsCreated || IsInitialized)
                return;

            m_tasks = new ConcurrentList<Task>();

            //Application.CreateConsole();
            //m_container.CreateChildContainer();            
            Application.Initialize();
            MessagePump.Initialize();
            ProcessMessage += OnProcessMessage;
            //m_token = token;
            //m_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(m_token);  
            InitializeResourcesShared();
            AddManager<PlatformWindowManager>();
            AddManager<PlatformRenderManager>();
            var mans = IoCContainer.ResolveAll<IPlatformManager>();
            foreach (var m in mans)
                m.Initialize();

            IsInitialized = true;
        }

        public static void Dispose()
        {
            IsInitialized = false;
            IsCreated = false;
            m_runTask?.ConfigureAwait(false);
            m_runTask?.Dispose();

            DisposeUnmanagedResourcesShared();
            MessagePump.Dispose();
            Application.Dispose();
            DisposeManagedResourcesShared();
            //foreach (var m in m_container.ResolveAll<IPlatformManager>())
            //    m.Create();

            Logger.Dispose();
            IoCContainer.Dispose();
        }

        public static void Run(CancellationToken token)
        {
            if (!IsCreated || !IsInitialized)
                Throw.Exception<InvalidOperationException>();

            m_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
            m_tokenSource.Token.Register(() => Dispatcher.Signal(NThreadDispatcherPriority.Send));
            Startup?.Invoke();
            IsRunning = true;
            Logger.LogDebug("Pumping Messages.");
            var mans = IoCContainer.ResolveAll<IPlatformManager>();
            while (!m_tokenSource.IsCancellationRequested)
            {
                while (MessagePump.Poll(out var e, m_tokenSource.Token))
                {
                    var message = e;
                    m_tasks.Add(Task.Run(() => ProcessMessage?.Invoke(message as IPlatformMessage)).ContinueWith(t => m_tasks.Remove(t)));
                }

                PreRunResourcesShared(m_tokenSource.Token);
                foreach (var m in mans)
                    m.Pump(m_tokenSource.Token);
                PostRunResourcesShared(m_tokenSource.Token);
            }

            foreach (var m in mans)
                m.Wait();

            var whenAll = Task.WhenAll(m_tasks);
            Task.WhenAll(whenAll).ConfigureAwait(false);

            for (;;)
            {
                while (!whenAll.IsCompleted)
                {
                    Console.Write(".");
                    Thread.Sleep(500);
                }

                break;
            }

            Logger.LogDebug("Exit Pumping Messages.");
            m_tasks.Clear();
            IsRunning = false;
            Shutdown?.Invoke();
        }

        public static void RunAsync(CancellationToken token)
        {
            m_runTask = new Task(() => { Run(token); }); //Task.Run(() => { Run(token); });
            //.ContinueWith((t) => { Dispose(); })
            //m_runTask.ConfigureAwait(false);
            m_runTask.Start();
        }

        public static void Create()
        {
            Create(new Logger(), new IoCContainer());
        }

        public static void Create(ILogger logger)
        {
            Create(logger, new IoCContainer());
        }

        public static void Create(IContainer iocContainer)
        {
            Create(new Logger(), iocContainer);
        }

        public static void Create(IContainer iocContainer, params IPlatformManager[] managers)
        {
            Create(new Logger(), iocContainer, managers);
        }

        public static void Create(ILogger logger, IContainer iocContainer, params IPlatformManager[] managers)
        {
            Logger = logger;
            Logger.Initialize();
            IoCContainer = iocContainer;
            IoCContainer.Register(logger);
            var tmpManagers = managers.ToList();
            var os = GetOsType();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                      .Where(a => Attribute.IsDefined(a, typeof(AssemblyPlatformAttribute))).ToArray();

            var platform = assemblies
                          .Where(a => Attribute.IsDefined(a, typeof(AssemblyPlatformAttribute)))
                          .GetAssemblyAttribute<AssemblyPlatformAttribute>()
                          .Where(attribute => attribute.RequiredOS == os)
                          .OrderBy(attribute => attribute.Priority).FirstOrDefault();

            if (platform == null)
                throw new InvalidOperationException("No platform found. Are you missing assembly references?");

            IoCContainer.Register(platform.OperatingSystemType);
            IoCContainer.Register(platform.RuntimeType);
            IoCContainer.Register(platform.ApplicationType);
            IoCContainer.Register<PlatformEnvironment>();
            IoCContainer.Register<MainMessagePump>();

            Application = IoCContainer.Resolve(platform.ApplicationType) as INApplication;
            Dispatcher = IoCContainer.Resolve(platform.DispatcherType) as INThreadDispatcher;
            Environment = IoCContainer.Resolve<PlatformEnvironment>();
            MessagePump = IoCContainer.Resolve<MainMessagePump>();

            Environment.DetectPlatform();
            NThreadDispatcherSynchronizationContext.InstallIfNeeded();
            CreateResourcesShared();
            //Attribute.IsDefined(a, typeof(AssemblyPlatformAttribute))
            var tmp = GetAssemblyAttributes<PlatformAttribute>(os).Where(a => a.ManagerType != null);
            //var tmp2 = tmp.Select(m => m.ManagerType);
            var t = new Task(() =>
                             {
                                 foreach (var a in tmp)
                                     IoCContainer.Register(a.ManagerType);
                                 //var i = a.ManagerType.GetInterfaces().Where(t => (t.IsInterface && t.ContainsInterface<IPlatformManager>())).ToArray();
                                 //m_managers[i[0]] = new Lazy<IPlatformManager>(m_container.Resolve(a.ManagerType) as IPlatformManager);
                             });
            t.Start();

            var t1 = new Task(() =>
                              {
                                  foreach (var m in tmpManagers)
                                      IoCContainer.Register(m.GetType(), m);
                                  //var i = m.GetType().GetInterfaces().Where(t => (t.IsInterface && t.ContainsInterface<IPlatformManager>())).ToArray();
                                  //m_managers[i[0]] = new Lazy<IPlatformManager>(m);
                              });
            t1.Start();

            Task.WaitAll(t, t1);

            foreach (var m in IoCContainer.ResolveAll<IPlatformManager>())
                m.Create();
            //foreach (var manager in tmpManagers)
            //{
            //    var aType = typeof(IPlatformManager);
            //    var types = manager.GetType().GetInterfaces().
            //                        Where(t => 
            //                                  (aType.IsAssignableFrom(t) && t.IsInterface && !t.IsGenericType && t != aType)).ToList();
            //    //if (!types.Any(t1 => t1.ContainsInterface<IPlatformManager>()))
            //    //    continue;

            //    //var t2 = types.Select(t1 => t1.ContainsInterface<IPlatformManager>());
            //    foreach (var t in types)
            //    {
            //        m_container.Register(t, manager);
            //        m_managers[t] = new Lazy<IPlatformManager>(manager);
            //            manager.Create();

            //            //var i = t.ContainsInterface<IPlatformManager>();
            //        //if (i)
            //        //{
            //        //    m_managers[t] = new Lazy<IPlatformManager>(manager);
            //        //    manager.Create();
            //        //    break;
            //        //}

            //        //foreach (var type in t.GetInterfaces())
            //        //{
            //        //    //if (type.ContainsInterface<IPlatformManager>())
            //        //    //{
            //        //    //    m_managers[type] = new Lazy<IPlatformManager>(manager);
            //        //    //    manager.Create();
            //        //    //}

            //        //    var i = t.ContainsInterface<IPlatformManager>();
            //        //    if (i)
            //        //    {
            //        //        m_managers[t] = new Lazy<IPlatformManager>(manager);
            //        //        manager.Create();
            //        //        break;
            //        //    }

            //        //    foreach (var type in t.GetInterfaces())
            //        //    {
            //        //        if (type.ContainsInterface<IPlatformManager>())
            //        //        {
            //        //            m_managers[type] = new Lazy<IPlatformManager>(manager);
            //        //            manager.Create();
            //        //        }
            //        //    }
            //        //}
            //    }
            //}
            IsCreated = true;
            IsInitialized = false;
        }

        public static TManager AddManager<TManager>() where TManager : IPlatformManager, new()
        {
            IoCContainer.Register<TManager>();
            var instance = IoCContainer.Resolve<TManager>();
            instance.Create();
            if (IsInitialized)
                instance.Initialize();

            return instance;
        }

        public static void AddManager<TManager>(TManager instance) where TManager : IPlatformManager
        {
            IoCContainer.Register(instance);
            //m_managers[typeof(TManager)] = new Lazy<IPlatformManager>(instance);

            if (!instance.IsCreated)
                instance.Create();
            if (IsInitialized && !instance.IsInitialized)
                instance.Initialize();
        }

        private static bool TestInterface<TInterface>(Type t)
        {
            if (t.ContainsInterface<TInterface>()) return true;

            foreach (var type in t.GetInterfaces())
            {
                if (type.ContainsInterface<TInterface>()) return true;

                if (TestInterface<TInterface>(type))
                    return true;
            }

            return false;
        }

        private static IEnumerable<TAttribute> GetAssemblyAttributes<TAttribute>(OsType osType)
            where TAttribute : PlatformAttribute
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                      .Where(a => Attribute.IsDefined(a, typeof(TAttribute))).ToArray();

            return assemblies
                  .Where(a => Attribute.IsDefined(a, typeof(TAttribute)))
                  .GetAssemblyAttribute<TAttribute>()
                  .Where(attribute => attribute.RequiredOS == osType)
                  .OrderBy(attribute => attribute.Priority);
        }

        static partial void DisposeManagedResourcesShared();

        static partial void DisposeUnmanagedResourcesShared();

        static partial void InitializeResourcesShared();

        static partial void PreRunResourcesShared(CancellationToken token);

        static partial void PostRunResourcesShared(CancellationToken token);

        static partial void CreateResourcesShared();

        private static IOsInformation CreateOs(IEnumerable<Assembly> assemblies)
        {
            var osType = assemblies
                        .Where(a => Attribute.IsDefined(a, typeof(PlatformAttribute)))
                        .SelectMany(a => a.GetTypes())
                        .Where(t => !t.GetInterfaces().Where(i => i.IsAssignableFrom(typeof(IOsInformation))).IsEmpty())
                        .OrderBy(t => t == typeof(OSInformation)).FirstOrDefault();

            if (osType == null)
                throw new InvalidOperationException("No platform found. Are you missing assembly references?");

            var os = Activator.CreateInstance(osType) as IOsInformation;
            if (os == null)
                throw new InvalidOperationException("No platform found. Are you missing assembly references?");

            os.DetectOperatingSystem();
            return os;
        }

        private static IRuntimeInformation CreateRuntime(IEnumerable<Assembly> assemblies)
        {
            var osType = assemblies
                        .SelectMany(a => a.GetTypes())
                        .Where(t => !t.GetInterfaces().Where(i => i.IsAssignableFrom(typeof(IRuntimeInformation))).IsEmpty())
                        .OrderBy(t => t == typeof(RuntimeInformation)).FirstOrDefault();

            if (osType == null)
                throw new InvalidOperationException("No platform found. Are you missing assembly references?");

            var os = Activator.CreateInstance(osType) as IRuntimeInformation;
            if (os == null)
                throw new InvalidOperationException("No platform found. Are you missing assembly references?");

            os.DetectRuntime();
            return os;
        }

        private static OsType GetOsType()
        {
            var id = SysEnv.OSVersion.Platform;
            switch ((int)id)
            {
                case 6: // PlatformID.MacOSX:
                    return OsType.MacOS;
                case 4: // PlatformID.Unix:	
                case 128:
                    return DetectUnixSystemType?.Invoke() ?? OsType.Unix;
                case 0: // PlatformID.Win32S:
                case 1: // PlatformID.Win32Windows:
                case 2: // PlatformID.Win32NT:
                case 3: // PlatformID.WinCE:
                    return OsType.Windows;
                default:
                    return OsType.Unknown;
            }
        }

        private static void OnProcessMessage(IPlatformMessage message)
        {
            Logger.LogDebug("Found Messages.");
            switch (message.Id)
            {
                case MessageIds.Quit:
                    if (!m_tokenSource.IsCancellationRequested)
                        m_tokenSource.Cancel();
                    break;
                case MessageIds.Rendering:
                    Logger.LogDebug("Stop me");
                    break; 
            }
        }
        #endregion
    }
}