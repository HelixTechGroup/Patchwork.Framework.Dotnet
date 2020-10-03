#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPosition
    {
        #region Members
        public WindowPositionFlags Flags;
        public int Height;
        public IntPtr Hwnd;
        public IntPtr HwndZOrderInsertAfter;
        public int Width;
        public int X;
        public int Y;
        #endregion
    }
}