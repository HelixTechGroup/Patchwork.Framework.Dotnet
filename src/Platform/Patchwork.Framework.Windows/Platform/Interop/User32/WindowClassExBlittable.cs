#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowClassExBlittable
    {
        #region Members
        public IntPtr BackgroundBrushHandle;
        public int ClassExtraBytes;
        public IntPtr ClassName;
        public IntPtr CursorHandle;
        public IntPtr IconHandle;
        public IntPtr InstanceHandle;
        public IntPtr MenuName;
        public uint Size;
        public IntPtr SmallIconHandle;
        public WindowClassStyles Styles;
        public int WindowExtraBytes;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WindowProc WindowProc;
        #endregion
    }
}