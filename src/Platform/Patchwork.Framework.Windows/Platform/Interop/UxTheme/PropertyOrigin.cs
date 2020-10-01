namespace Patchwork.Framework.Platform.Interop.UxTheme
{
    public enum PropertyOrigin
    {
        /// <summary>
        ///     Property was found in the state section.
        /// </summary>
        PO_STATE = 0,

        /// <summary>
        ///     Property was found in the part section.
        /// </summary>
        PO_PART = 1,

        /// <summary>
        ///     Property was found in the class section.
        /// </summary>
        PO_CLASS = 2,

        /// <summary>
        ///     Property was found in the list of global variables.
        /// </summary>
        PO_GLOBAL = 3,

        /// <summary>
        ///     Property was not found.
        /// </summary>
        PO_NOTFOUND = 4
    }
}