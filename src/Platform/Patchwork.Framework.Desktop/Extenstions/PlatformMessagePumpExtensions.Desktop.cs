#region Usings
using System.Drawing;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Windowing;
#endregion

namespace Patchwork.Framework.Extensions
{
    public static partial class PlatformMessagePumpExtensions
    {
        #region Methods
        public static void PushWindowStateChangedMessage(this IPlatformMessagePump pump,
                                                         INWindow window, NWindowState state)
        {
            pump.PushWindowMessage(new WindowMessageData(window).StateChanged(state));
            //pump.Push(new PlatformMessage(MessageIds.Window, new WindowMessageData(WindowMessageIds.StateChanged, window)));
        }
        #endregion
    }
}