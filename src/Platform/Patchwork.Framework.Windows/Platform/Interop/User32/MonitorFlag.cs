namespace Patchwork.Framework.Platform.Interop.User32 {
    public enum MonitorFlag
    {
        /// <summary>
        ///     Returns NULL.
        /// </summary>
        MONITOR_DEFAULTTONULL = 0,

        /// <summary>
        ///     Returns a handle to the primary display monitor.
        /// </summary>
        MONITOR_DEFAULTTOPRIMARY = 1,

        /// <summary>
        ///     Returns a handle to the display monitor that is nearest to the window.
        /// </summary>
        MONITOR_DEFAULTTONEAREST = 2
    }
}