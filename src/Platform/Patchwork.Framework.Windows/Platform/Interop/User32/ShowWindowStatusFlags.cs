using System;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [Flags]
    public enum ShowWindowStatusFlags
    {
        /// <summary>
        ///     The 'ShowWindow' function was used, that sent the message.
        /// </summary>
        SW_USER_CALL = 0,

        /// <summary>
        ///     The window is being uncovered because a maximize window was restored or minimized.
        /// </summary>
        SW_OTHERUNZOOM = 4,

        /// <summary>
        ///     The window is being covered by another window that has been maximized.
        /// </summary>
        SW_OTHERZOOM = 2,

        /// <summary>
        ///     The window's owner window is being minimized.
        /// </summary>
        SW_PARENTCLOSING = 1,

        /// <summary>
        ///     The window's owner window is being restored.
        /// </summary>
        SW_PARENTOPENING = 3
    }
}