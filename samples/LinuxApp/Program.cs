using System;
using System.Threading;
using Patchwork.Framework;
using Patchwork.Framework.Messaging;
using Shin.Framework.Logging.Loggers;
using Shin.Framework.Logging.Native;

namespace LinuxApp
{
    class Program
    {
        #region Methods
        private static readonly CancellationTokenSource m_cts = new CancellationTokenSource();

        private static void Main(string[] args)
        {
            var log = new Logger();
            log.Initialize();
            log.AddLogProvider(new ConsoleLogger());
            PlatformManager.Create(log);
            PlatformManager.Initialize();
            PlatformManager.ProcessMessage += OnMessage;
            PlatformManager.Application.CreateWindow().Show();
            PlatformManager.Run(m_cts.Token);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            PlatformManager.Dispose();
        }

        private static void OnMessage(IPlatformMessage message)
        {
            PlatformManager.Logger.LogDebug("Message type: " + message.Id);
            switch (message.Id)
            {
                case MessageIds.Window:
                    var data = message.Data as WindowMessageData;
                    //Throw.IfNull(wmsg).ArgumentNullException(nameof(message));
                    PlatformManager.Logger.LogDebug("--Message sub type: " + data.MessageId);
                    break;
                case MessageIds.Quit:
                    m_cts.Cancel();
                    break;
                case MessageIds.Display:
                    break;
                case MessageIds.System:
                    break;
                case MessageIds.Keyboard:
                    break;
                case MessageIds.Mouse:
                    break;
                case MessageIds.Joystick:
                    break;
                case MessageIds.Controller:
                    break;
                case MessageIds.Touch:
                    break;
                case MessageIds.Sensor:
                    break;
                case MessageIds.Audio:
                    break;
                case MessageIds.User:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
        #endregion
    }
}
