using System;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CwpRetStruct
    {
        public IntPtr LResult;
        public IntPtr LParam;
        public IntPtr WParam;
        public uint Message;
        public IntPtr Hwnd;
    }
}
