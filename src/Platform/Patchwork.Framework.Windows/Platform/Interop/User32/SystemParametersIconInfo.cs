namespace Patchwork.Framework.Platform.Interop.User32 {
    public enum SystemParametersIconInfo
    {
        /// <summary>
        ///     Retrieves the metrics associated with icons. The pvParam parameter must point to an ICONMETRICS structure that
        ///     receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(ICONMETRICS).
        /// </summary>
        SPI_GETICONMETRICS = 0x002D,

        /// <summary>
        ///     Retrieves the logical font information for the current icon-title font. The uiParam parameter specifies the size of
        ///     a LOGFONT structure, and the pvParam parameter must point to the LOGFONT structure to fill in.
        /// </summary>
        SPI_GETICONTITLELOGFONT = 0x001F,

        /// <summary>
        ///     Determines whether icon-title wrapping is enabled. The pvParam parameter must point to a BOOL variable that
        ///     receives TRUE if enabled, or FALSE otherwise.
        /// </summary>
        SPI_GETICONTITLEWRAP = 0x0019,

        /// <summary>
        ///     Sets or retrieves the width, in pixels, of an icon cell. The system uses this rectangle to arrange icons in large
        ///     icon view.
        ///     To set this value, set uiParam to the new value and set pvParam to NULL. You cannot set this value to less than
        ///     SM_CXICON.
        ///     To retrieve this value, pvParam must point to an integer that receives the  current value.
        /// </summary>
        SPI_ICONHORIZONTALSPACING = 0x000D,

        /// <summary>
        ///     Sets or retrieves the height, in pixels, of an icon cell.
        ///     To set this value, set uiParam to the new value and set pvParam to NULL. You cannot set this value to less than
        ///     SM_CYICON.
        ///     To retrieve this value, pvParam must point to an integer that receives the  current value.
        /// </summary>
        SPI_ICONVERTICALSPACING = 0x0018,

        /// <summary>
        ///     Sets the metrics associated with icons. The pvParam parameter must point to an ICONMETRICS structure that contains
        ///     the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(ICONMETRICS).
        /// </summary>
        SPI_SETICONMETRICS = 0x002E,

        /// <summary>
        ///     Reloads the system icons. Set the uiParam parameter to zero and the pvParam parameter to NULL.
        /// </summary>
        SPI_SETICONS = 0x0058,

        /// <summary>
        ///     Sets the font that is used for icon titles. The uiParam parameter specifies the size of a LOGFONT structure, and
        ///     the pvParam parameter must point to a LOGFONT structure.
        /// </summary>
        SPI_SETICONTITLELOGFONT = 0x0022,

        /// <summary>
        ///     Turns icon-title wrapping on or off. The uiParam parameter specifies TRUE for on, or FALSE for off.
        /// </summary>
        SPI_SETICONTITLEWRAP = 0x001A
    }
}