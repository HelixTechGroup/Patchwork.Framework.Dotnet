#region Usings
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NcCalcSizeOutput
    {
        #region Members
        public Rectangle DestRect;
        public Rectangle SrcRect;
        public Rectangle TargetClientRect;
        #endregion
    }
}