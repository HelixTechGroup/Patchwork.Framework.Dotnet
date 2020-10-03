#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [Flags]
    public enum ElementSystemStates
    {
        /// <summary>
        ///     The element can accept the focus.
        /// </summary>
        STATE_SYSTEM_FOCUSABLE = 0x00100000,

        /// <summary>
        ///     The element is invisible.
        /// </summary>
        STATE_SYSTEM_INVISIBLE = 0x00008000,

        /// <summary>
        ///     The element has no visible representation.
        /// </summary>
        STATE_SYSTEM_OFFSCREEN = 0x00010000,

        /// <summary>
        ///     The element is unavailable.
        /// </summary>
        STATE_SYSTEM_UNAVAILABLE = 0x00000001,

        /// <summary>
        ///     The element is in the pressed state.
        /// </summary>
        STATE_SYSTEM_PRESSED = 0x00000008
    }
}