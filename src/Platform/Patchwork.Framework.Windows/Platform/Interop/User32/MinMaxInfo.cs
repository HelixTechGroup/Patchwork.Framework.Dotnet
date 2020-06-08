using System.Drawing;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct MinMaxInfo
    {
        Point Reserved;

        /// <summary>
        ///     The maximized width (x member) and the maximized height (y member) of the window. For top-level windows, this value
        ///     is based on the width of the primary monitor.
        /// </summary>
        public Point MaxSize;

        /// <summary>
        ///     The position of the left side of the maximized window (x member) and the position of the top of the maximized
        ///     window (y member). For top-level windows, this value is based on the position of the primary monitor.
        /// </summary>
        public Point MaxPosition;

        /// <summary>
        ///     The minimum tracking width (x member) and the minimum tracking height (y member) of the window. This value can be
        ///     obtained programmatically from the system metrics SM_CXMINTRACK and SM_CYMINTRACK (see the GetSystemMetrics
        ///     function).
        /// </summary>
        public Point MinTrackSize;

        /// <summary>
        ///     The maximum tracking width (x member) and the maximum tracking height (y member) of the window. This value is based
        ///     on the size of the virtual screen and can be obtained programmatically from the system metrics SM_CXMAXTRACK and
        ///     SM_CYMAXTRACK (see the GetSystemMetrics function).
        /// </summary>
        public Point MaxTrackSize;
    }
}