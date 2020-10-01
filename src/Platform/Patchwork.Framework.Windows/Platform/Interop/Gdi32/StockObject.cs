namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    public enum StockObject
    {
        /// <summary>
        ///     Black brush.
        /// </summary>
        BLACK_BRUSH = 4,

        /// <summary>
        ///     Dark gray brush.
        /// </summary>
        DKGRAY_BRUSH = 3,

        /// <summary>
        ///     Solid color brush. The default color is white. The color can be changed by using the SetDCBrushColor function. For
        ///     more information, see the Remarks section.
        /// </summary>
        DC_BRUSH = 18,

        /// <summary>
        ///     Gray brush.
        /// </summary>
        GRAY_BRUSH = 2,

        /// <summary>
        ///     Hollow brush (equivalent to NULL_BRUSH).
        /// </summary>
        HOLLOW_BRUSH = NULL_BRUSH,

        /// <summary>
        ///     Light gray brush.
        /// </summary>
        LTGRAY_BRUSH = 1,

        /// <summary>
        ///     Null brush (equivalent to HOLLOW_BRUSH).
        /// </summary>
        NULL_BRUSH = 5,

        /// <summary>
        ///     White brush.
        /// </summary>
        WHITE_BRUSH = 0,

        /// <summary>
        ///     Black pen.
        /// </summary>
        BLACK_PEN = 7,

        /// <summary>
        ///     Solid pen color. The default color is white. The color can be changed by using the SetDCPenColor function. For more
        ///     information, see the Remarks section.
        /// </summary>
        DC_PEN = 19,

        /// <summary>
        ///     Null pen. The null pen draws nothing.
        /// </summary>
        NULL_PEN = 8,

        /// <summary>
        ///     White pen.
        /// </summary>
        WHITE_PEN = 6,

        /// <summary>
        ///     Windows fixed-pitch (monospace) system font.
        /// </summary>
        ANSI_FIXED_FONT = 11,

        /// <summary>
        ///     Windows variable-pitch (proportional space) system font.
        /// </summary>
        ANSI_VAR_FONT = 12,

        /// <summary>
        ///     Device-dependent font.
        /// </summary>
        DEVICE_DEFAULT_FONT = 14,

        /// <summary>
        ///     Default font for user interface objects such as menus and dialog boxes. It is not recommended that you use
        ///     DEFAULT_GUI_FONT or SYSTEM_FONT to obtain the font used by dialogs and windows; for more information, see the
        ///     remarks section.
        ///     The default font is Tahoma.
        /// </summary>
        DEFAULT_GUI_FONT = 17,

        /// <summary>
        ///     Original equipment manufacturer (OEM) dependent fixed-pitch (monospace) font.
        /// </summary>
        OEM_FIXED_FONT = 10,

        /// <summary>
        ///     System font. By default, the system uses the system font to draw menus, dialog box controls, and text. It is not
        ///     recommended that you use DEFAULT_GUI_FONT or SYSTEM_FONT to obtain the font used by dialogs and windows; for more
        ///     information, see the remarks section.
        ///     The default system font is Tahoma.
        /// </summary>
        SYSTEM_FONT = 13,

        /// <summary>
        ///     Fixed-pitch (monospace) system font. This stock object is provided only for compatibility with 16-bit Windows
        ///     versions earlier than 3.0.
        /// </summary>
        SYSTEM_FIXED_FONT = 16,

        /// <summary>
        ///     Default palette. This palette consists of the static colors in the system palette.
        /// </summary>
        DEFAULT_PALETTE = 15
    }
}