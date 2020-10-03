#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemInfo
    {
        #region Members
        public IntPtr ActiveProcessorMask;
        public uint AllocationGranularity;
        public IntPtr MaximumApplicationAddress;
        public IntPtr MinimumApplicationAddress;
        public uint NumberOfProcessors;
        public uint PageSize;
        public ushort ProcessorArchitecture;
        public ushort ProcessorLevel;
        public ushort ProcessorRevision;
        public uint ProcessorType;
        readonly ushort Reserved;
        #endregion

        #region Properties
        public uint OemId
        {
            get { return ((uint)ProcessorArchitecture << 8) | Reserved; }
        }
        #endregion
    }
}