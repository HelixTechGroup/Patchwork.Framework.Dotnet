using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Explicit)]
    public struct NcCalcSizeRegionUnion
    {
        [FieldOffset(0)] public NcCalcSizeInput Input;
        [FieldOffset(0)] public NcCalcSizeOutput Output;
    }
}