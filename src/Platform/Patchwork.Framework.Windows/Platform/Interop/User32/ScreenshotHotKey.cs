namespace Patchwork.Framework.Platform.Interop.User32
{
    public enum ScreenshotHotKey
    {
        IDHOT_NONE = 0,

        /// <summary>
        ///     The "snap desktop" hot key was pressed. /* PRINTSCRN */
        /// </summary>
        IDHOT_SNAPDESKTOP = -2,

        /// <summary>
        ///     The "snap window" hot key was pressed. /* SHIFT-PRINTSCRN  */,
        /// </summary>
        IDHOT_SNAPWINDOW = -1
    }
}