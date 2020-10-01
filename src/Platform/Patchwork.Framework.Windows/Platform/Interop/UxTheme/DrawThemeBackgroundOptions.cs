using System.Drawing;

namespace Patchwork.Framework.Platform.Interop.UxTheme
{
    /// <summary>
    ///     Defines the options for the DrawThemeBackgroundEx function.
    /// </summary>
    public struct DrawThemeBackgroundOptions
    {
        /// <summary>
        ///     Size of the structure. Set this to sizeof(DTBGOPTS).
        /// </summary>
        public uint Size;

        public DrawThemeBackgroundFlags Flags;

        /// <summary>
        ///     A RECT that specifies the bounding rectangle of the clip region.
        /// </summary>
        public Rectangle ClipRect;
    }
}