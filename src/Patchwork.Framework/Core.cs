#region Usings
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.Logging.Native;
using SysEnv = System.Environment;
using TypeExtensions = Shin.Framework.Extensions.TypeExtensions;
#endregion

namespace Patchwork.Framework
{
    public delegate void ProcessMessageHandler(IPlatformMessage message);

    public static partial class Core
    {
        #region Events
        public static event Func<OperatingSystemType> DetectUnixSystemType;
        public static event ProcessMessageHandler ProcessMessage;

        public static event Action Shutdown;

        public static event Action Startup;
        #endregion

        #region Members
        private static ConcurrentDictionary<Type, Lazy<IPlatformManager>> m_managers;

        //private static PlatformManager m_platform;
        //private static CancellationTokenSource m_tokenSource;
        //private static  CancellationToken m_token;
        //private static IoCContainer m_container;
        private static Task m_runTask;
        private static IList<Task> m_tasks;
        #endregion

        #region Properties
        public static INativeApplication Application { get; private set; }

        public static INativeThreadDispatcher Dispatcher { get; private set; }

        public static IApplicationEnvironment Environment { get; private set; }
        public static bool IsInitialized { get; private set; }

        public static bool IsRunning { get; private set; }

        public static ILogger Logger { get; private set; }

        public static CoreMessagePump MessagePump { get; private set; }

        public static IEnumerable<IPlatformManager> Managers
        {
            get
            {
                return m_managers.Values.Where(lazy => lazy.IsValueCreated).Select(lazy => lazy.Value);
            }
        }
        #endregion

        #region Methods
        //public static void Initialize()
        //{
        //    Initialize(new CancellationToken());
        //}

        public static void Initialize()
        {
            if (IsInitialized)
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
            foreach (var manager in m_managers.Values)
                manager.Value.Initialize();

            IsInitialized = true;
        }

        public static void Dispose()
        {
            IsInitialized = false;
            m_runTask?.ConfigureAwait(false);
            m_runTask?.Dispose();

            DisposeUnmanagedResourcesShared();
            MessagePump.Dispose();
            Application.Dispose();
            DisposeManagedResourcesShared();
            foreach (var manager in m_managers.Values)
                manager.Value.Dispose();

            Logger.Dispose();
        }

        public static void Run(CancellationToken token)
        {
            if (!IsInitialized)
                Throw.Exception<InvalidOperationException>();

            token.Register(() => Dispatcher.Signal(NativeThreadDispatcherPriority.Send));
            Startup?.Invoke();
            IsRunning = true;
            Logger.LogDebug("Pumping Messages.");
            while (!token.IsCancellationRequested)
            {
                while (MessagePump.Poll(out var e, token))
                {
                    var message = e;
                    m_tasks.Add(Task.Run(() => ProcessMessage?.Invoke(message as IPlatformMessage)).ContinueWith(t => m_tasks.Remove(t)));
                }

                PreRunResourcesShared(token);
                foreach (var manager in m_managers.Values)
                    manager.Value.Pump(token);
                PostRunResourcesShared(token);
            }

            foreach (var manager in m_managers.Values)
                manager.Value.Wait();

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
            Create(new Logger());
        }

        public static void Create(params IPlatformManager[] managers)
        {
            Create(new Logger(), managers);
        }

        public static void Create(ILogger logger, params IPlatformManager[] managers)
        {
            Logger = logger;
            Logger.Initialize();
            m_managers = new ConcurrentDictionary<Type, Lazy<IPlatformManager>>();
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

            var osInfo = platform.OperatingSystemType == null
                             ? new OperatingSystemInformation()
                             : Activator.CreateInstance(platform.OperatingSystemType) as IOperatingSystemInformation;
            var runtimeInfo = platform.RuntimeType == null
                                  ? new RuntimeInformation()
                                  : Activator.CreateInstance(platform.RuntimeType) as IRuntimeInformation;

            Application = Activator.CreateInstance(platform.ApplicationType) as INativeApplication;
            Dispatcher = Activator.CreateInstance(platform.DispatcherType) as INativeThreadDispatcher;
            Environment = new PlatformEnvironment(osInfo, runtimeInfo);
            MessagePump = new CoreMessagePump(Logger, Application);

            Environment.DetectPlatform();
            CreateResourcesShared();
            //Attribute.IsDefined(a, typeof(AssemblyPlatformAttribute))
            var tmp  = GetAssemblyAttributes<PlatformAttribute>(os).Where(a => a.ManagerType != null);
            tmpManagers.AddRange(tmp.Select(m => Activator.CreateInstance(m.ManagerType) as IPlatformManager));

            foreach (var manager in tmpManagers)
            {
                var aType = typeof(IPlatformManager);
                var types = manager.GetType().GetInterfaces().
                                    Where(t => 
                                              (aType.IsAssignableFrom(t) && t.IsInterface && !t.IsGenericType && t != aType)).ToList();
                //if (!types.Any(t1 => t1.ContainsInterface<IPlatformManager>()))
                //    continue;

                //var t2 = types.Select(t1 => t1.ContainsInterface<IPlatformManager>());
                foreach (var t in types)
                {
                    m_managers[t] = new Lazy<IPlatformManager>(manager);
                        manager.Create();

                        //var i = t.ContainsInterface<IPlatformManager>();
                    //if (i)
                    //{
                    //    m_managers[t] = new Lazy<IPlatformManager>(manager);
                    //    manager.Create();
                    //    break;
                    //}

                    //foreach (var type in t.GetInterfaces())
                    //{
                    //    //if (type.ContainsInterface<IPlatformManager>())
                    //    //{
                    //    //    m_managers[type] = new Lazy<IPlatformManager>(manager);
                    //    //    manager.Create();
                    //    //}

                    //    var i = t.ContainsInterface<IPlatformManager>();
                    //    if (i)
                    //    {
                    //        m_managers[t] = new Lazy<IPlatformManager>(manager);
                    //        manager.Create();
                    //        break;
                    //    }

                    //    foreach (var type in t.GetInterfaces())
                    //    {
                    //        if (type.ContainsInterface<IPlatformManager>())
                    //        {
                    //            m_managers[type] = new Lazy<IPlatformManager>(manager);
                    //            manager.Create();
                    //        }
                    //    }
                    //}
                }
            }
        }

        private static bool TestInterface<TInterface>(Type t)
        {
            if (t.ContainsInterface<TInterface>())
            {
                return true;
            }

            foreach (var type in t.GetInterfaces())
            {
                if (type.ContainsInterface<TInterface>())
                {
                    return true;
                }

                if (TestInterface<TInterface>(type))
                    return true;
            }

            return false;
        }

        private static IEnumerable<TAttribute> GetAssemblyAttributes<TAttribute>(OperatingSystemType osType)
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

        public static void AddManager<TManager>() where TManager : IPlatformManager, new()
        {
            var instance = new TManager();
            m_managers[typeof(TManager)] = new Lazy<IPlatformManager>(instance);

            instance.Create();
            if (IsInitialized)
                instance.Initialize();
        }

        public static void AddManager<TManager>(TManager instance) where TManager : IPlatformManager
        {
            m_managers[typeof(TManager)] = new Lazy<IPlatformManager>(instance);

            instance.Create();
            if (IsInitialized && !instance.IsInitialized)
                instance.Initialize();
        }

        static partial void DisposeManagedResourcesShared();

        static partial void DisposeUnmanagedResourcesShared();

        static partial void InitializeResourcesShared();

        static partial void PreRunResourcesShared(CancellationToken token);

        static partial void PostRunResourcesShared(CancellationToken token);

        static partial void CreateResourcesShared();

        private static IOperatingSystemInformation CreateOs(IEnumerable<Assembly> assemblies)
        {
            var osType = assemblies
                        .Where(a => Attribute.IsDefined(a, typeof(PlatformAttribute)))
                        .SelectMany(a => a.GetTypes())
                        .Where(t => !t.GetInterfaces().Where(i => i.IsAssignableFrom(typeof(IOperatingSystemInformation))).IsEmpty())
                        .OrderBy(t => t == typeof(OperatingSystemInformation)).FirstOrDefault();

            if (osType == null)
                throw new InvalidOperationException("No platform found. Are you missing assembly references?");

            var os = Activator.CreateInstance(osType) as IOperatingSystemInformation;
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

        private static OperatingSystemType GetOsType()
        {
            var id = SysEnv.OSVersion.Platform;
            switch ((int)id)
            {
                case 6: // PlatformID.MacOSX:
                    return OperatingSystemType.MacOS;
                case 4: // PlatformID.Unix:	
                case 128:
                    return DetectUnixSystemType?.Invoke() ?? OperatingSystemType.Unix;
                case 0: // PlatformID.Win32S:
                case 1: // PlatformID.Win32Windows:
                case 2: // PlatformID.Win32NT:
                case 3: // PlatformID.WinCE:
                    return OperatingSystemType.Windows;
                default:
                    return OperatingSystemType.Unknown;
            }
        }

        private static void OnProcessMessage(IPlatformMessage message)
        {
            Logger.LogDebug("Found Messages.");
            switch (message.Id)
            {
                case MessageIds.Quit:
                    break;
            }
        }
        #endregion
    }
}