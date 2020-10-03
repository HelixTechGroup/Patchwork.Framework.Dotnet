#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.KernelBase
{
    public static class Kernel32Methods
    {
        #region Members
        public const string LibraryName = "kernelbase";
        #endregion

        #region Methods
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool CompareObjectHandles(IntPtr hFirstObjectHandle, IntPtr hSecondObjectHandle);
        #endregion
    }
}