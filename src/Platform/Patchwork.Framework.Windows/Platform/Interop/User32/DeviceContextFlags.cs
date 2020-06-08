using System;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [Flags]
    public enum DeviceContextFlags
    {
        /// <summary>
        ///     Returns a DC that corresponds to the window rectangle rather than the client rectangle.
        /// </summary>
        DCX_WINDOW = 0x0000000,

        /// <summary>
        ///     Returns a DC from the cache, rather than the OWNDC or CLASSDC window. Essentially overrides CS_OWNDC and
        ///     CS_CLASSDC.
        /// </summary>
        DCX_CACHE = 0x0000000,

        /// <summary>
        ///     Does not reset the attributes of this DC to the default attributes when this DC is released.
        /// </summary>
        DCX_NORESETATTRS = 0x0000000,

        /// <summary>
        ///     Excludes the visible regions of all child windows below the window identified by hWnd.
        /// </summary>
        DCX_CLIPCHILDREN = 0x0000000,

        /// <summary>
        ///     Excludes the visible regions of all sibling windows above the window identified by hWnd.
        /// </summary>
        DCX_CLIPSIBLINGS = 0x0000001,

        /// <summary>
        ///     Uses the visible region of the parent window. The parent's WS_CLIPCHILDREN and CS_PARENTDC style bits are ignored.
        ///     The origin is set to the upper-left corner of the window identified by hWnd.
        /// </summary>
        DCX_PARENTCLIP = 0x0000002,

        /// <summary>
        ///     The clipping region identified by hrgnClip is excluded from the visible region of the returned DC.
        /// </summary>
        DCX_EXCLUDERGN = 0x0000004,

        /// <summary>
        ///     The clipping region identified by hrgnClip is intersected with the visible region of the returned DC.
        /// </summary>
        DCX_INTERSECTRGN = 0x0000008,

        /// <summary>
        ///     Undocumented flag
        /// </summary>
        DCX_EXCLUDEUPDATE = 0x0000010,

        /// <summary>
        ///     Reserved; do not use.
        /// </summary>
        DCX_INTERSECTUPDATE = 0x0000020,

        /// <summary>
        ///     Allows drawing even if there is a LockWindowUpdate call in effect that would otherwise exclude this window. Used
        ///     for drawing during tracking.
        /// </summary>
        DCX_LOCKWINDOWUPDATE = 0x0000040,

        /// <summary>
        ///     Reserved; do not use.
        /// </summary>
        DCX_VALIDATE = 0x0020000
    }
}