using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RgbQuad
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        private byte Reserved;
    }
}