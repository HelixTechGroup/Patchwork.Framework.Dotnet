#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
        #region Members
        public uint Data;
        public IntPtr ExtraInfo;
        public MouseInputFlags Flags;
        public uint Time;
        public int X;
        public int Y;
        #endregion
    }
}