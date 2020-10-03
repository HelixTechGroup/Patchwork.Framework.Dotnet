namespace Patchwork.Framework.Platform.Interop.User32
{
    public enum ClipboardFormat : uint
    {
        /// <summary>
        /// A handle to a bitmap (HBITMAP)
        /// </summary>
        CF_BITMAP = 2u,

        /// <summary>
        /// Flag for text-format
        /// </summary>
        CF_TEXT = 1u,

        /// <summary>
        /// Flag for unicode-text-format
        /// </summary>
        CF_UNICODETEXT = 13u,

        /// <summary>
        /// no format
        /// </summary>
        CF_ZERO = 0u
    }
}