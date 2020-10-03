#region Usings
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPlacement
    {
        #region Members
        public WindowPlacementFlags Flags;
        public Point MaxPosition;
        public Point MinPosition;
        public Rectangle NormalPosition;
        public ShowWindowCommands ShowCmd;
        public uint Size;
        #endregion
    }
}