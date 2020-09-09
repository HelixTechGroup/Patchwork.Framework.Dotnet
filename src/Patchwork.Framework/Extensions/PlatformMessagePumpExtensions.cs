using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;

namespace Patchwork.Framework.Extensions
{
    public static class PlatformMessagePumpExtensions
    {
        public static void PushWindowMessage(this PlatformMessagePump pump, WindowMessageIds messageId, INativeWindow window)
        {
            pump.Push(new PlatformMessage(MessageIds.Window, new WindowMessageData(messageId, window)));
        }

        public static void PushWindowMessage(this PlatformMessagePump pump, IWindowMessageData data)
        {
            pump.Push(new PlatformMessage(MessageIds.Window, data));
        }

        public static void PushWindowMovingMessage(this PlatformMessagePump pump,
                                                     INativeWindow window,
                                                     Point requestedPosition)
        {
            pump.PushWindowMessage(new WindowMessageData(window).PositionChanging(requestedPosition));
        }

        public static void PushWindowMovedMessage(this PlatformMessagePump pump,
                                                   INativeWindow window,
                                                   Point requestedPosition)
        {
            pump.PushWindowMessage(new WindowMessageData(window).PositionChanged(requestedPosition));
        }

        public static void PushWindowResizingMessage(this PlatformMessagePump pump,
                                                   INativeWindow window,
                                                   Size currentSize,
                                                   Size requestedSize)
        {
            pump.PushWindowMessage(new WindowMessageData(window).SizeChanging(requestedSize));
        }

        public static void PushWindowResizedMessage(this PlatformMessagePump pump,
                                                  INativeWindow window,
                                                  Size requestedSize)
        {
            pump.PushWindowMessage(new WindowMessageData(window).SizeChanged(requestedSize));
        }

        public static void PushWindowFocusingMessage(this PlatformMessagePump pump,
                                                     INativeWindow window)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanging(false));
        }

        public static void PushWindowFocusedMessage(this PlatformMessagePump pump,
                                                    INativeWindow window)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanged(false));
        }

        public static void PushWindowFocusChangingMessage(this PlatformMessagePump pump,
                                                     INativeWindow window,
                                                     bool requestedFocus)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanging(requestedFocus));
        }

        public static void PushWindowFocusChangedMessage(this PlatformMessagePump pump,
                                                        INativeWindow window,
                                                        bool requestedFocus)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanged(requestedFocus));
        }

        public static void PushWindowUnfocusingMessage(this PlatformMessagePump pump,
                                                       INativeWindow window)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanging(false));
        }

        public static void PushWindowUnfocusedMessage(this PlatformMessagePump pump,
                                                      INativeWindow window)
        {
            pump.PushWindowMessage(new WindowMessageData(window).FocusChanged(false));
        }
    }
}
