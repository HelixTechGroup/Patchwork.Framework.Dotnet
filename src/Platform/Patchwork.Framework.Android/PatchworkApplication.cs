using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Runtime;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Messaging;
using Shin.Framework.Logging.Loggers;
using Shin.Framework.Logging.Native;

namespace Patchwork.Framework
{
    [Application]
    public class PatchworkApplication : Application
    {
        private static readonly CancellationTokenSource m_cts = new CancellationTokenSource();

        public CancellationTokenSource Token { get { return m_cts; } }

        public PatchworkApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();
            //app init ...
            var log = new Logger();
            log.Initialize();
            log.AddLogProvider(new ConsoleLogger(false));
            Core.DetectUnixSystemType += () => OSType.Android;
            Core.ProcessMessage += OnMessage;
            Core.Create(log);
            Core.Initialize();            
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Core.Dispose();

            base.Dispose(disposing);
        }

        private void OnMessage(IPlatformMessage message)
        {
            Core.Logger.LogDebug("Message type: " + message.Id);
            switch (message.Id)
            {
                case MessageIds.Window:
                    var data = message.Data as WindowMessageData;
                    //Throw.IfNull(wmsg).ArgumentNullException(nameof(message));
                    Core.Logger.LogDebug("--Message sub type: " + data?.MessageId);
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
    }
}
