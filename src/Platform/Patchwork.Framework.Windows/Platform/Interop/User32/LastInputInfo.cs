#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LastInputInfo
    {
        #region Members
        public uint Size;
        public uint Time;
        #endregion
    }
}