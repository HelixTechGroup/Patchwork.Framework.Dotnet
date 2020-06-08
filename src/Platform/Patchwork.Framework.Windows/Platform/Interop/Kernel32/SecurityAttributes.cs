using System;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.Kernel32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityAttributes
    {
        public uint Length;
        public IntPtr SecurityDescriptor;
        public uint IsHandleInheritedValue;

        public bool IsHandleInherited => this.IsHandleInheritedValue > 0;
    }
}