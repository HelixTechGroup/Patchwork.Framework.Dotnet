using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.Kernel32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct FileTime
    {
        public uint Low;
        public uint High;

        public ulong Value => ((ulong) this.High << 32) | this.Low;
    }
}