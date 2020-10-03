#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CwpRetStruct
    {
        #region Members
        public IntPtr Hwnd;
        public IntPtr LParam;
        public IntPtr LResult;
        public uint Message;
        public IntPtr WParam;
        #endregion
    }
}