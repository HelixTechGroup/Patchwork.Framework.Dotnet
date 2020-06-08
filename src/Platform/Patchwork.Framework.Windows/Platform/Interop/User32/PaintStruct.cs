using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PaintStruct
    {
        public IntPtr HandleDC;
        public int EraseBackgroundValue;
        public Rectangle PaintRect;
        private int ReservedInternalRestore;
        private int ReservedInternalIncUpdate;
        private fixed byte ReservedInternalRgb [32];

        public bool ShouldEraseBackground
        {
            get { return this.EraseBackgroundValue > 0; }
            set { this.EraseBackgroundValue = value ? 1 : 0; }
        }
    }
}