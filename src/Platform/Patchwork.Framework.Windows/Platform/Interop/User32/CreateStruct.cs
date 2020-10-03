#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CreateStruct
    {
        #region Members
        public IntPtr ClassName;
        public IntPtr CreateParams;
        public WindowExStyles ExStyles;
        public int Height;
        public IntPtr InstanceHandle;
        public IntPtr MenuHandle;
        public IntPtr Name;
        public IntPtr ParentHwnd;
        public WindowStyles Styles;
        public int Width;
        public int X;
        public int Y;
        #endregion
    }
}