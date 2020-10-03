#region Usings
using System;
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CursorInfo
    {
        #region Members
        public IntPtr CursorHandle;
        public CursorInfoFlags Flags;
        public Point ScreenPosition;
        public uint Size;
        #endregion
    }
}