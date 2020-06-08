namespace Patchwork.Framework.Platform.Interop.User32 {
    public enum SystemParametersMenuInfo
    {
        /// <summary>
        ///     Determines whether pop-up menus are left-aligned or right-aligned, relative to the corresponding menu-bar item. The
        ///     pvParam parameter must point to a BOOL variable that receives TRUE if right-aligned, or FALSE otherwise.
        /// </summary>
        SPI_GETMENUDROPALIGNMENT = 0x001B,

        /// <summary>
        ///     Determines whether menu fade animation is enabled. The pvParam parameter must point to a BOOL variable that
        ///     receives TRUE when fade animation is enabled and FALSE when it isdisabled. If fade animation is disabled, menus use
        ///     slide animation. This flag is ignored unless menu animation is enabled, which you can do using the
        ///     SPI_SETMENUANIMATION flag. For more information, see AnimateWindow.
        /// </summary>
        SPI_GETMENUFADE = 0x1012,

        /// <summary>
        ///     Retrieves the time, in milliseconds, that the system waits before displaying a shortcut menu when the mouse cursor
        ///     is over a submenu item. The pvParam parameter must point to a DWORD variable that receives the time of the delay.
        /// </summary>
        SPI_GETMENUSHOWDELAY = 0x006A,

        /// <summary>
        ///     Sets the alignment value of pop-up menus. The uiParam parameter specifies TRUE for right alignment, or FALSE for
        ///     left alignment.
        /// </summary>
        SPI_SETMENUDROPALIGNMENT = 0x001C,

        /// <summary>
        ///     Enables or disables menu fade animation. Set pvParam to TRUE to enable the menu fade effect or FALSE to disable it.
        ///     If fade animation is disabled, menus use slide animation. he The menu fade effect is possible only if the system
        ///     has a color depth of more than 256 colors. This flag is ignored unless SPI_MENUANIMATION is also set. For more
        ///     information, see AnimateWindow.
        /// </summary>
        SPI_SETMENUFADE = 0x1013,

        /// <summary>
        ///     Sets uiParam to the time, in milliseconds, that the system waits before displaying a shortcut menu when the mouse
        ///     cursor is over a submenu item.
        /// </summary>
        SPI_SETMENUSHOWDELAY = 0x006B
    }
}