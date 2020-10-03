#region Usings
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MonitorInfo
    {
        #region Members
        /// <summary>
        ///     A set of flags that represent attributes of the display monitor.
        /// </summary>
        public MonitorInfoFlag Flags;

        /// <summary>
        ///     A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates. Note that
        ///     if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
        /// </summary>
        public Rectangle MonitorRect;

        /// <summary>
        ///     The size of the structure, in bytes.
        /// </summary>
        public uint Size;

        /// <summary>
        ///     A RECT structure that specifies the work area rectangle of the display monitor, expressed in virtual-screen
        ///     coordinates. Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may
        ///     be negative values.
        /// </summary>
        public Rectangle WorkRect;
        #endregion
    }
}