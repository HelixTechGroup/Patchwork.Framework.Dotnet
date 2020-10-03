#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BlendFunction
    {
        #region Members
        public AlphaFormat AlphaFormat;
        public byte BlendFlags;
        public byte BlendOp;
        public byte SourceConstantAlpha;
        #endregion
    }
}