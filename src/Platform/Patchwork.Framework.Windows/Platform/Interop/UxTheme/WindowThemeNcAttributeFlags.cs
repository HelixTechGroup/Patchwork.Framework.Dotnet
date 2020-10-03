#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.UxTheme
{
    [Flags]
    public enum WindowThemeNcAttributeFlags
    {
        /// <summary>
        ///     Prevents the window caption from being drawn.
        /// </summary>
        WTNCA_NODRAWCAPTION = 0x00000001,

        /// <summary>
        ///     Prevents the system icon from being drawn.
        /// </summary>
        WTNCA_NODRAWICON = 0x00000002,

        /// <summary>
        ///     Prevents the system icon menu from appearing.
        /// </summary>
        WTNCA_NOSYSMENU = 0x00000004,

        /// <summary>
        ///     Prevents mirroring of the question mark, even in right-to-left (RTL) layout.
        /// </summary>
        WTNCA_NOMIRRORHELP = 0x00000008,

        /// <summary>
        ///     All valid bits
        /// </summary>
        WTNCA_VALIDBITS = WTNCA_NODRAWCAPTION | WTNCA_NODRAWICON | WTNCA_NOMIRRORHELP | WTNCA_NOSYSMENU
    }
}