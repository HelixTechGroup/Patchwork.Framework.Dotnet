#region Usings
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Shield.Framework.IoC.Native.DependencyInjection;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.Logging.Loggers;
using Shin.Framework.Logging.Native;
using RuntimeInformation = Patchwork.Framework.Environment.RuntimeInformation;
using SysEnv = System.Environment;
#endregion

namespace Patchwork.Framework
{
    public static partial class Core
    {
        public static event ProcessMessageHandler ProcessMessage;

        public static event Func<OperatingSystemType> DetectUnixSystemType;

        public static event Action Startup;

        public static event Action Shutdown;

        #region Members
        //private static PlatformManager m_platform;
        //private static CancellationTokenSource m_tokenSource;
        //private static  CancellationToken m_token;
        //private static IoCContainer m_container;
        private static Task m_runTask;
        #endregion

        #region Properties
        public static bool IsInitialized { get; private set; }

        public static bool IsRunning { get; private set; }

        public static INativeApplication Application { get; private set; }

        public static INativeThreadDispatcher Dispatcher { get; private set; }

        public static IApplicationEnvironment Environment { get; private set; }

        public static PlatformMessagePump MessagePump { get; private set; }

        public static ILogger Logger { get; private set; }
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

            //Application.CreateConsole();
            //m_container.CreateChildContainer();            
            Application.Initialize();
            MessagePump.Initialize();
            ProcessMessage += OnProcessMessage;
            //m_token = token;
            //m_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(m_token);  
            InitializeResourcesShared(); 
            IsInitialized = true;                               
        }

        static partial void DisposeManagedResourcesShared();

        static partial void DisposeUnmanagedResourcesShared();

        static partial void InitializeResourcesShared();

        public static void Dispose()
        {
            IsInitialized = false;
            m_runTask?.ConfigureAwait(false);
            m_runTask?.Dispose();
            DisposeUnmanagedResourcesShared();
            MessagePump.Dispose();
            Application.Dispose();
            DisposeManagedResourcesShared();
            Logger.Dispose();
        }

        static partial void RunResourcesShared(CancellationToken token);

        public static void Run(CancellationToken token)
        {
            if (!IsInitialized)
                Throw.Exception<InvalidOperationException>();

            token.Register(() => Dispatcher.Signal(NativeThreadDispatcherPriority.Send));
            var tasks = new ConcurrentList<Task>();
            Startup?.Invoke();
            IsRunning = true;
            Logger.LogDebug("Pumping Messages.");
            while (!token.IsCancellationRequested)
            {
                while (MessagePump.Poll(out var e, token))
                {
                    var message = e;
                    tasks.Add(Task.Run(() => ProcessMessage?.Invoke(message)).ContinueWith((t) => tasks.Remove(t)));
                }

                RunResourcesShared(token);
            }

            var whenAll = Task.WhenAll(tasks);
            Task.WhenAll(whenAll).ConfigureAwait(false);

            for (; ; )
            {
                while (!whenAll.IsCompleted)
                {
                    Console.Write(".");
                    Thread.Sleep(500);
                }
                break;
            }

            Logger.LogDebug("Exit Pumping Messages.");
            IsRunning = false;
            Shutdown?.Invoke();
        }

        public static void RunAsync(CancellationToken token)
        {
            m_runTask = new Task(() => { Run(token); });//Task.Run(() => { Run(token); });
            //.ContinueWith((t) => { Dispose(); })
            //m_runTask.ConfigureAwait(false);
            m_runTask.Start();
        }

        public static void Create()
        {
            Create(new Logger());
        }

        public static void Create(ILogger logger)
        {
            Logger = logger;
            Logger.Initialize();
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
            MessagePump = new PlatformMessagePump(Logger, Application);

            Environment.DetectPlatform();
            CreateResourcesShared();
        }

        static partial void CreateResourcesShared();

        private static IOperatingSystemInformation CreateOs(IEnumerable<Assembly> assemblies)
        {
            var osType = assemblies
                        .Where(a => Attribute.IsDefined(a, typeof(AssemblyPlatformAttribute)))
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