#region Usings
using System;
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        #region Members
        public IntPtr Hwnd;
        public IntPtr LParam;
        public Point Point;
        public uint Time;
        public WindowsMessageIds Value;
        public IntPtr WParam;
        #endregion
    }
}