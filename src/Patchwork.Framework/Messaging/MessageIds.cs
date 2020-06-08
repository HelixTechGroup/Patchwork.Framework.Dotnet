using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Messaging
{
    public enum MessageIds : int
    {
        Quit = -1,
        Display,
        System,
        Window,
        Keyboard,
        Mouse,
        Joystick,
        Controller,
        Touch,
        Sensor,
        Audio,
        User
    }
}
