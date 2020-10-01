using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapInfoHeader
    {
        public uint Size;
        public int Width;
        public int Height;
        public ushort Planes;
        public ushort BitCount;
        public BitmapCompressionMode CompressionMode;
        public uint SizeImage;
        public int XPxPerMeter;
        public int YPxPerMeter;
        public uint ClrUsed;
        public uint ClrImportant;
    }
}