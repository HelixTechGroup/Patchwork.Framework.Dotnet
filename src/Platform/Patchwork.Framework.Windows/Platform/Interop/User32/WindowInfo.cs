#region Usings
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowInfo
    {
        #region Members
        public uint BorderX;
        public uint BorderY;
        public Rectangle ClientRect;
        public ushort CreatorVersion;
        public WindowExStyles ExStyles;
        public uint Size;
        public WindowStyles Styles;
        public Rectangle WindowRect;
        public uint WindowStatus;
        public ushort WindowType;
        #endregion
    }
}