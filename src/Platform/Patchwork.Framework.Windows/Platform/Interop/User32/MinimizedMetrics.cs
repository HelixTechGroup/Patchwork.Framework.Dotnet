#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MinimizedMetrics
    {
        #region Members
        public ArrangeFlags Arrange;
        public int HorizontalGap;
        public uint Size;
        public int VerticalGap;
        public int Width;
        #endregion
    }
}