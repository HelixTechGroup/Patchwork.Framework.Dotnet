#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential, CharSet = Properties.BuildCharSet)]
    public struct WindowClassEx
    {
        public uint Size;
        public WindowClassStyles Styles;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WindowProc WindowProc;

        public int ClassExtraBytes;
        public int WindowExtraBytes;
        public IntPtr InstanceHandle;
        public IntPtr IconHandle;
        public IntPtr CursorHandle;
        public IntPtr BackgroundBrushHandle;
        public string MenuName;
        public string ClassName;
        public IntPtr SmallIconHandle;
    }
}