#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityAttributes
    {
        #region Members
        public uint IsHandleInheritedValue;
        public uint Length;
        public IntPtr SecurityDescriptor;
        #endregion

        #region Properties
        public bool IsHandleInherited
        {
            get { return IsHandleInheritedValue > 0; }
        }
        #endregion
    }
}