#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FileTime
    {
        #region Members
        public uint High;
        public uint Low;
        #endregion

        #region Properties
        public ulong Value
        {
            get { return ((ulong)High << 32) | Low; }
        }
        #endregion
    }
}