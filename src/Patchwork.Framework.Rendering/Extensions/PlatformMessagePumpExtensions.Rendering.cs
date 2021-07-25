#region Usings
using System.Drawing;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Windowing;
#endregion

namespace Patchwork.Framework.Extensions
{
    public static partial class PlatformMessagePumpExtensions
    {
        #region Methods
        public static void PushRenderMessage(this IPlatformMessagePump pump, RenderMessageIds messageId, INRenderer renderer)
        {
            pump.Push(new PlatformMessage(MessageIds.Rendering, new RenderMessageData(messageId, renderer)));
        }

        public static void PushRenderMessage(this IPlatformMessagePump pump, IRenderMessageData data)
        {
            pump.Push(new PlatformMessage(MessageIds.Rendering, data));
        }

        public static void PushRenderOsMessage(this IPlatformMessagePump pump,
                                               INWindow window,
                                               Point requestedPosition)
        {

        }

        public static void PushFrameBuffer(this IPlatformMessagePump pump, INRenderer renderer, NFrameBuffer frameBuffer)
        {
            pump.PushRenderMessage(new RenderMessageData(renderer).SetFrameBuffer(frameBuffer.Copy()));
        }

        public static void PushRenderWindowMessage(this IPlatformMessagePump pump,
                                                   INWindow window,
                                                   Point requestedPosition)
        {
            //pump.PushWindowMessage(new RenderMessageData(window));
        }
        #endregion
    }
}