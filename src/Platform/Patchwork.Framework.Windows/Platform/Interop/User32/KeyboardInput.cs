using System;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput
    {
        public ushort VirtualKeyCode;
        public ushort ScanCode;
        public KeyboardInputFlags Flags;
        public uint Time;
        public IntPtr ExtraInfo;

        public VirtualKey Key
        {
            get { return (VirtualKey) this.VirtualKeyCode; }
            set { this.VirtualKeyCode = (ushort) value; }
        }
    }
}