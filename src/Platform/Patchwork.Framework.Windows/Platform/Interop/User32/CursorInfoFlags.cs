#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [Flags]
    public enum CursorInfoFlags
    {
        /// <summary>
        ///     The cursor is showing.
        /// </summary>
        CURSOR_SHOWING = 0x00000001,

        /// <summary>
        ///     Windows 8: The cursor is suppressed. This flag indicates that the system is not drawing the cursor because the user
        ///     is providing input through touch or pen instead of the mouse.
        /// </summary>
        CURSOR_SUPPRESSED = 0x00000002
    }
}