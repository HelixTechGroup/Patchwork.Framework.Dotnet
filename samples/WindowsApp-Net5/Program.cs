#region Usings
using System;
using System.Threading;
using Patchwork.Framework;
using Patchwork.Framework.Manager;
using Patchwork.Framework.Messaging;
using Shield.Framework.IoC.Native.DependencyInjection;
using Shin.Framework.Logging.Loggers;
using Shin.Framework.Logging.Native;
#endregion

namespace WindowsApp
{
    internal class Program
    {
        #region Members
        private static readonly CancellationTokenSource m_cts = new CancellationTokenSource();
        #endregion

        #region Methods
        [MTAThread]
        private static void Main(string[] args)
        {
            var log = new Logger();
            log.Initialize();
            log.AddLogProvider(new ConsoleLogger());

            Core.Startup += OnStartup;
            Core.Shutdown += OnShutdown;
            Core.ProcessMessage += OnMessage;

            Core.Create(log);
            Core.CreateConsole();
            Core.Initialize();

            Core.Window.CreateWindow().Show();
            Core.Run(m_cts.Token);

            Core.Dispose();
            Core.CloseConsole();
        }

        private static void OnStartup() { }

        private static void OnShutdown()
        {
            Core.Logger.LogNone("Press any key to exit.");
            try
            {
                while (!Console.KeyAvailable)
                {
                    Core.Logger.LogNone(".");
                    Thread.Sleep(100);
                }
            }
            catch (InvalidOperationException iEx)
            {
                
            }
        }

        private static void OnMessage(IPlatformMessage message)
        {
            Core.Logger.LogDebug("Message type: " + message.Id);
            switch (message.Id)
            {
                case MessageIds.Window:
                    var data = message.RawData as IWindowMessageData;
                    //Throw.IfNull(wmsg).ArgumentNullException(nameof(message));
                    Core.Logger.LogDebug("--Message sub type: " + data?.MessageId);
                    break;
                case MessageIds.Rendering:
                    var data2 = message.RawData as IRenderMessageData;
                    //Throw.IfNull(wmsg).ArgumentNullException(nameof(message));
                    Core.Logger.LogDebug("--Message sub type: " + data2?.MessageId);
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