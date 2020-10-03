#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Explicit)]
    public struct NcCalcSizeRegionUnion
    {
        #region Members
        [FieldOffset(0)]
        public NcCalcSizeInput Input;

        [FieldOffset(0)]
        public NcCalcSizeOutput Output;
        #endregion
    }
}