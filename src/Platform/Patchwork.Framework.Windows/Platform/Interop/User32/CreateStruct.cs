using System;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct CreateStruct
    {
        public IntPtr CreateParams;
        public IntPtr InstanceHandle;
        public IntPtr MenuHandle;
        public IntPtr ParentHwnd;
        public int Height;
        public int Width;
        public int Y;
        public int X;
        public WindowStyles Styles;
        public IntPtr Name;
        public IntPtr ClassName;
        public WindowExStyles ExStyles;
    }
}