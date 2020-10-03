namespace Patchwork.Framework.Platform.Interop.User32
{
    public enum WindowActivateFlag
    {
        /// <summary>
        ///     Activated by some method other than a mouse click (for example, by a call to the SetActiveWindow function or by use
        ///     of the keyboard interface to select the window).
        /// </summary>
        WA_ACTIVE = 1,

        /// <summary>
        ///     Activated by a mouse click.
        /// </summary>
        WA_CLICKACTIVE = 2,

        /// <summary>
        ///     Deactivated.
        /// </summary>
        WA_INACTIVE = 0
    }
}