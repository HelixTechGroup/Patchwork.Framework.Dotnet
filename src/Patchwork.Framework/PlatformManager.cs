#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Shield.Framework.IoC.Native.DependencyInjection;
using Shin.Framework;
using Shin.Framework.Extensions;
using Shin.Framework.Logging.Loggers;
using Shin.Framework.Logging.Native;
using SysEnv = System.Environment;
#endregion

namespace Patchwork.Framework
{
    public class PlatformManager
    {
        public static event EventHandler<IPlatformMessage> ProcessMessage;

        #region Members
        //private static PlatformManager m_platform;
        //private static CancellationTokenSource m_tokenSource;
        //private static  CancellationToken m_token;
        //private static IoCContainer m_container;
        #endregion

        #region Properties
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

        public static void Initialize(ILogger logger)
        {
            Application.CreateConsole();
            Logger = logger;
            Logger.Initialize();
            //m_container.CreateChildContainer();            
            Logger.AddLogProvider(new ConsoleLogger());
            Environment.DetectPlatform();
            Application.Initialize();
            MessagePump.Initialize();
            ProcessMessage += OnProcessMessage;
        }

        public static void Initialize()
        {
            Initialize(new Logger());
            //m_token = token;
            //m_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(m_token);                                  
        }

        public static void Dispose()
        {
            MessagePump.Dispose();
            Application.Dispose();
            Logger.Dispose();
            Application.CloseConsole();
            ProcessMessage.Dispose();
        }

        public static void Run(CancellationToken token)
        {
            Logger.LogDebug("Pumping Messages.");
            while (!token.IsCancellationRequested)
                while (MessagePump.Poll(out var e, token))
                    /*Task.Run(() => */ProcessMessage.Raise(MessagePump, e)/*, token)*/;
            Logger.LogDebug("Exit Pumping Messages.");
        }

        public static void Create()
        {
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
        }

        //protected IOperatingSystemInformation CreateOs(IEnumerable<Assembly> assemblies)
        //{
        //    var osType = assemblies
        //                .Where(a => Attribute.IsDefined(a, typeof(AssemblyPlatformAttribute)))
        //                .SelectMany(a => a.GetTypes())
        //                .Where(t => !t.GetInterfaces().Where(i => i.IsAssignableFrom(typeof(IOperatingSystemInformation))).IsEmpty())
        //                .OrderBy(t => t == typeof(OperatingSystemInformation)).FirstOrDefault();

        //    if (osType == null)
        //        throw new InvalidOperationException("No platform found. Are you missing assembly references?");

        //    var os = Activator.CreateInstance(osType) as IOperatingSystemInformation;
        //    if (os == null)
        //        throw new InvalidOperationException("No platform found. Are you missing assembly references?");

        //    os.DetectOperatingSystem();
        //    return os;
        //}

        //protected IRuntimeInformation CreateRuntime(IEnumerable<Assembly> assemblies)
        //{
        //    var osType = assemblies
        //                .SelectMany(a => a.GetTypes())
        //                .Where(t => !t.GetInterfaces().Where(i => i.IsAssignableFrom(typeof(IRuntimeInformation))).IsEmpty())
        //                .OrderBy(t => t == typeof(RuntimeInformation)).FirstOrDefault();

        //    if (osType == null)
        //        throw new InvalidOperationException("No platform found. Are you missing assembly references?");

        //    var os = Activator.CreateInstance(osType) as IRuntimeInformation;
        //    if (os == null)
        //        throw new InvalidOperationException("No platform found. Are you missing assembly references?");

        //    os.DetectRuntime();
        //    return os;
        //}

        private static OperatingSystemType GetOsType()
        {
            var id = SysEnv.OSVersion.Platform;
            switch ((int)id)
            {
                case 6: // PlatformID.MacOSX:
                    return OperatingSystemType.MacOS;
                case 4: // PlatformID.Unix:	
                    return OperatingSystemType.Unix;
                case 0: // PlatformID.Win32S:
                case 1: // PlatformID.Win32Windows:
                case 2: // PlatformID.Win32NT:
                case 3: // PlatformID.WinCE:
                    return OperatingSystemType.Windows;
                default:
                    return OperatingSystemType.Unknown;
            }
        }

        private static void OnProcessMessage(object sender, IPlatformMessage message) 
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