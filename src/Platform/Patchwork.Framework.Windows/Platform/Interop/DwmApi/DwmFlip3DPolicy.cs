#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.DwmApi
{
    [Flags]
    public enum DwmFlip3DPolicy
    {
        /// <summary>
        ///     Use the window's style and visibility settings to determine whether to hide or include the window in Flip3D
        ///     rendering.
        /// </summary>
        DWMFLIP3D_DEFAULT,

        /// <summary>
        ///     Exclude the window from Flip3D and display it below the Flip3D rendering
        /// </summary>
        DWMFLIP3D_EXCLUDEBELOW,

        /// <summary>
        ///     Exclude the window from Flip3D and display it above the Flip3D rendering
        /// </summary>
        DWMFLIP3D_EXCLUDEABOVE,

        /// <summary>
        ///     The maximum recognized DWMFLIP3DWINDOWPOLICY value, used for validation purposes
        /// </summary>
        DWMFLIP3D_LAST
    }
}