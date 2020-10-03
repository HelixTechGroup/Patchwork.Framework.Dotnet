#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FileAttributeData
    {
        #region Members
        public FileAttributes Attributes;
        public FileTime CreationTime;

        public uint FileSizeHigh;
        public uint FileSizeLow;
        public FileTime LastAccessTime;
        public FileTime LastWriteTime;
        #endregion

        #region Properties
        public ulong FileSize
        {
            get { return ((ulong)FileSizeHigh << 32) | FileSizeLow; }
        }
        #endregion
    }
}