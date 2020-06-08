using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.Kernel32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct FileAttributeData
    {
        public FileAttributes Attributes;
        public FileTime CreationTime;
        public FileTime LastAccessTime;
        public FileTime LastWriteTime;

        public uint FileSizeHigh;
        public uint FileSizeLow;

        public ulong FileSize => ((ulong) this.FileSizeHigh << 32) | this.FileSizeLow;
    }
}