using System;

namespace Patchwork.Framework.Platform.Interop.DwmApi
{
    [Flags]
    public enum DwmNCRenderingPolicy
    {
        /// <summary>
        ///     The non-client rendering area is rendered based on the window style.
        /// </summary>
        DWMNCRP_USEWINDOWSTYLE,

        /// <summary>
        ///     The non-client area rendering is disabled; the window style is ignored.
        /// </summary>
        DWMNCRP_DISABLED,

        /// <summary>
        ///     The non-client area rendering is enabled; the window style is ignored.
        /// </summary>
        DWMNCRP_ENABLED,

        /// <summary>
        ///     The maximum recognized DWMNCRENDERINGPOLICY value, used for validation purposes.
        /// </summary>
        DWMNCRP_LAST
    }
}