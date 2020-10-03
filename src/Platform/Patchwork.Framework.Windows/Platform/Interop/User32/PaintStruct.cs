#region Usings
using System;
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PaintStruct
    {
        #region Members
        public int EraseBackgroundValue;
        public IntPtr HandleDC;
        public Rectangle PaintRect;
        private readonly int ReservedInternalIncUpdate;
        private readonly int ReservedInternalRestore;
        private fixed byte ReservedInternalRgb[32];
        #endregion

        #region Properties
        public bool ShouldEraseBackground
        {
            get { return EraseBackgroundValue > 0; }
            set { EraseBackgroundValue = value ? 1 : 0; }
        }
        #endregion
    }
}