#region Usings
using System.Runtime.InteropServices;
#endregion

// ReSharper disable InconsistentNaming

namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapInfoHeader
    {
        #region Members
        public ushort BitCount;
        public uint ClrImportant;
        public uint ClrUsed;
        public BitmapCompressionMode CompressionMode;
        public int Height;
        public ushort Planes;
        public uint Size;
        public uint SizeImage;
        public int Width;
        public int XPxPerMeter;
        public int YPxPerMeter;
        #endregion
    }
}