using System;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
        public int X;
        public int Y;
        public uint Data;
        public MouseInputFlags Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }
}