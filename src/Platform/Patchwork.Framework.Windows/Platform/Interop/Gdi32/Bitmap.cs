#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Bitmap
    {
        #region Members
        /// <summary>
        ///     A pointer to the location of the bit values for the bitmap. The bmBits member must be a pointer to an array of
        ///     character (1-byte) values.
        /// </summary>
        public IntPtr Bits;

        /// <summary>
        ///     The number of bits required to indicate the color of a pixel.
        /// </summary>
        public ushort BitsPerPixel;

        /// <summary>
        ///     The height, in pixels, of the bitmap. The height must be greater than zero.
        /// </summary>
        public int Height;

        /// <summary>
        ///     The count of color planes.
        /// </summary>
        public ushort Planes;

        /// <summary>
        ///     The bitmap type. This member must be zero.
        /// </summary>
        public int Type;

        /// <summary>
        ///     The width, in pixels, of the bitmap. The width must be greater than zero.
        /// </summary>
        public int Width;

        /// <summary>
        ///     The number of bytes in each scan line. This value must be divisible by 2, because the system assumes that the bit
        ///     values of a bitmap form an array that is word aligned.
        /// </summary>
        public int WidthBytes;
        #endregion
    }
}