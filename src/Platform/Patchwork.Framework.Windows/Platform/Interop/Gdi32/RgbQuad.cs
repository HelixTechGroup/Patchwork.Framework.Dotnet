#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RgbQuad
    {
        #region Members
        public byte Blue;
        public byte Green;
        public byte Red;
        private readonly byte Reserved;
        #endregion
    }
}