#region Usings
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NcCalcSizeInput
    {
        #region Members
        public Rectangle CurrentClientRect;
        public Rectangle CurrentWindowRect;
        public Rectangle TargetWindowRect;
        #endregion
    }
}