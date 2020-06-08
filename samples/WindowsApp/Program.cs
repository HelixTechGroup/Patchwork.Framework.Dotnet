using System;
using System.Threading;
using Patchwork.Framework;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Interop.User32;
using Shin.Framework;
using Shin.Framework.Logging.Loggers;
using Shin.Framework.Logging.Native;
using Shin.Framework.Messaging;

namespace WindowsApp
{
    internal class Program
    {
        #region Methods
        private static readonly CancellationTokenSource m_cts = new CancellationTokenSource();

        private static void Main(string[] args)
        {
            var log = new Logger();
            log.Initialize();
            //log.AddLogProvider(new ConsoleLogger());

            //WindowsDesktopApplication.CreateConsole();

            //var env = new ApplicationEnvironment();
            //env.DetectPlatform();
            PlatformManager.Create();
            PlatformManager.Initialize(log);
            PlatformManager.ProcessMessage += OnMessage;
            //var pump = new PlatformMessagePump(log, PlatformManager.CurrentPlatform.Application);
            //pump.Initialize(cts.Token);
            PlatformManager.Application.CreateWindow().Show();
            PlatformManager.Run(m_cts.Token);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            //WindowsDesktopApplication.CloseConsole();
            PlatformManager.Dispose();
        }

        private static void OnMessage(object sender, IPlatformMessage message)
        {
            PlatformManager.Logger.LogDebug("Message type: " + message.Id);

            switch (message.Id)
            {
                case MessageIds.Window:
                    var wmsg = message as WindowMessage;
                    Throw.IfNull(wmsg);
                    PlatformManager.Logger.LogDebug("--Message sub type: " + wmsg.MessageId);
                    break;
                case MessageIds.Quit:
                    m_cts.Cancel();
                    break;
            }
            
        }
        #endregion
    }
}