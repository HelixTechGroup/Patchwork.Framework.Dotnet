using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Windowing;

namespace Patchwork.Framework.Extensions
{
    public static class PlatformMessagePumpExtensions
    {
        public static void PushWindowMessage(this IPlatformMessagePump pump, WindowMessageIds messageId, INWindow window)
        {
            pump.Push(new PlatformMessage(MessageIds.Window, new WindowMessageData(messageId, window)));
        }

        public static void PushWindowMessage(this IPlatformMessagePump pump, IWindowMessageData data)
        {
            pump.Push(new PlatformMessage(MessageIds.Window, data));
        }

        public static void PushWindowMovingMessage(this IPlatformMessagePump pump,
                                                     INWindow window,
                                                     Point requestedPosition)
        {
            pump.PushWindowMessage(new WindowMessageData(window).PositionChanging(requestedPosition));
        }

        public static void PushWindowMovedMessage(this IPlatformMessagePump pump,
                                                   INWindow window,
                                                   Point requestedPosition)
        {
            pump.PushWindowMessage(new WindowMessageData(window).PositionChanged(requestedPosition));
        }

        public static void PushWindowResizingMessage(this IPlatformMessagePump pump,
                                                   INWindow window,
                                                   Size requestedSize)
        {
            pump.PushWindowMessage(new WindowMessageData(window).SizeChanging(requestedSize));
        }

        public static void PushWindowResizedMessage(this IPlatformMessagePump pump,
                                                  INWindow window,
                                                  Size requestedSize)
        {
            pump.PushWindowMessage(new WindowMessageData(window).SizeChanged(requestedSize));
        }

        public static void PushWindowFocusingMessage(this IPlatformMessagePump pump,
                                                     INWindow window)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanging(true));
        }

        public static void PushWindowFocusedMessage(this IPlatformMessagePump pump,
                                                    INWindow window)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanged(true));
        }

        public static void PushWindowFocusChangingMessage(this IPlatformMessagePump pump,
                                                     INWindow window,
                                                     bool requestedFocus)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanging(requestedFocus));
        }

        public static void PushWindowFocusChangedMessage(this IPlatformMessagePump pump,
                                                        INWindow window,
                                                        bool requestedFocus)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanged(requestedFocus));
        }

        public static void PushWindowUnfocusingMessage(this IPlatformMessagePump pump,
                                                       INWindow window)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanging(false));
        }

        public static void PushWindowUnfocusedMessage(this IPlatformMessagePump pump,
                                                      INWindow window)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanged(false));
        }
    }
}
