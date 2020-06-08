namespace Patchwork.Framework.Platform.Interop.User32 {
    public enum LayeredWindowAttributeFlag
    {
        /// <summary>
        ///     Use bAlpha to determine the opacity of the layered window.
        /// </summary>
        LWA_ALPHA = 0x00000002,

        /// <summary>
        ///     Use crKey as the transparency color.
        /// </summary>
        LWA_COLORKEY = 0x00000001
    }
}