#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [Flags]
    public enum HotKeyInputState
    {
        /// <summary>
        ///     Either ALT key was held down.
        /// </summary>
        MOD_ALT = 0x0001,

        /// <summary>
        ///     Either CTRL key was held down.
        /// </summary>
        MOD_CONTROL = 0x0002,

        /// <summary>
        ///     Either SHIFT key was held down.
        /// </summary>
        MOD_SHIFT = 0x0004,

        /// <summary>
        ///     Either WINDOWS key was held down. These keys are labeled with the Windows logo. Hotkeys that involve the Windows
        ///     key are reserved for use by the operating system.
        /// </summary>
        MOD_WIN = 0x0008
    }
}