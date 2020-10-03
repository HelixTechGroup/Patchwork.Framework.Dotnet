#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    /// <summary>
    ///     Note: Marshalled
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Properties.BuildCharSet)]
    public struct WindowClassEx
    {
        #region Members
        public IntPtr BackgroundBrushHandle;
        public int ClassExtraBytes;
        public string ClassName;
        public IntPtr CursorHandle;
        public IntPtr IconHandle;
        public IntPtr InstanceHandle;
        public string MenuName;
        public uint Size;
        public IntPtr SmallIconHandle;
        public WindowClassStyles Styles;
        public int WindowExtraBytes;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WindowProc WindowProc;
        #endregion
    }
}