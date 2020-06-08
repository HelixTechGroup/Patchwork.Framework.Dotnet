using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct Input
    {
        public InputType Type;
        public InputPacket Packet;

        public static void InitHardwareInput(out Input input, uint message, ushort low, ushort high)
        {
            input = new Input
                    {
                        Type = InputType.INPUT_HARDWARE,
                        Packet = new InputPacket
                                 {
                                     HardwareInput = new HardwareInput
                                                     {
                                                         Message = message,
                                                         Low = low,
                                                         High = high
                                                     }
                                 }
                    };
        }

        public static void InitHardwareInput(out Input input, uint message, uint wParam)
        {
            InitHardwareInput(out input, message, (ushort) wParam, (ushort) (wParam >> 16));
        }

        public static void InitKeyboardInput(out Input input, ushort scanCode, bool isKeyUp,
                                             bool isExtendedKey = false, uint timestampMillis = 0)
        {
            input = new Input
                    {
                        Type = InputType.INPUT_KEYBOARD,
                        Packet = new InputPacket
                                 {
                                     KeyboardInput =
                                     {
                                         Time = timestampMillis,
                                         Flags = KeyboardInputFlags.KEYEVENTF_SCANCODE,
                                         ScanCode = scanCode,
                                         VirtualKeyCode = 0
                                     }
                                 }
                    };
            if (isKeyUp) input.Packet.KeyboardInput.Flags |= KeyboardInputFlags.KEYEVENTF_KEYUP;
            if (isExtendedKey) input.Packet.KeyboardInput.Flags |= KeyboardInputFlags.KEYEVENTF_EXTENDEDKEY;
        }

        public static void InitKeyboardInput(out Input input, char charCode, bool isKeyUp, uint timestampMillis = 0)
        {
            input = new Input
                    {
                        Type = InputType.INPUT_KEYBOARD,
                        Packet = new InputPacket
                                 {
                                     KeyboardInput =
                                     {
                                         Time = timestampMillis,
                                         Flags = KeyboardInputFlags.KEYEVENTF_UNICODE,
                                         ScanCode = charCode,
                                         VirtualKeyCode = 0
                                     }
                                 }
                    };
            if (isKeyUp) input.Packet.KeyboardInput.Flags |= KeyboardInputFlags.KEYEVENTF_KEYUP;
        }

        public static void InitKeyboardInput(out Input input, VirtualKey key, bool isKeyUp,
                                             uint timestampMillis = 0)
        {
            input = new Input
                    {
                        Type = InputType.INPUT_KEYBOARD,
                        Packet = new InputPacket
                                 {
                                     KeyboardInput =
                                     {
                                         Time = timestampMillis,
                                         Key = key,
                                         ScanCode = 0,
                                         Flags = 0
                                     }
                                 }
                    };
            if (isKeyUp) input.Packet.KeyboardInput.Flags |= KeyboardInputFlags.KEYEVENTF_KEYUP;
        }

        public static void InitMouseInput(out Input input, int x, int y, MouseInputFlags flags, uint data = 0,
                                          uint timestampMillis = 0)
        {
            input = new Input
                    {
                        Type = InputType.INPUT_MOUSE,
                        Packet = new InputPacket
                                 {
                                     MouseInput =
                                     {
                                         Time = timestampMillis,
                                         X = x,
                                         Y = y,
                                         Data = data,
                                         Flags = flags
                                     }
                                 }
                    };
        }
    }
}