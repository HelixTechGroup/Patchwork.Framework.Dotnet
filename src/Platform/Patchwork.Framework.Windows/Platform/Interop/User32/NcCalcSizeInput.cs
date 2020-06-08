using System.Drawing;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct NcCalcSizeInput
    {
        public Rectangle TargetWindowRect;
        public Rectangle CurrentWindowRect;
        public Rectangle CurrentClientRect;
    }
}