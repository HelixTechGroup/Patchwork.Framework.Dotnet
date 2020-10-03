#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct NcCalcSizeParams
    {
        #region Members
        public WindowPosition* Position;
        public NcCalcSizeRegionUnion Region;
        #endregion
    }
}