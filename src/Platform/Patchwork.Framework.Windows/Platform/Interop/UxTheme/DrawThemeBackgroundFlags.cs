using System;

namespace Patchwork.Framework.Platform.Interop.UxTheme
{
    [Flags]
    public enum DrawThemeBackgroundFlags
    {
        DTBG_CLIPRECT = 0x00000001, // rcClip has been specified,
        DTBG_DRAWSOLID = 0x00000002, // DEPRECATED: draw transparent/alpha images as solid,
        DTBG_OMITBORDER = 0x00000004, // don't draw border of part,
        DTBG_OMITCONTENT = 0x00000008, // don't draw content area of part,
        DTBG_COMPUTINGREGION = 0x00000010, // TRUE if calling to compute region,
        DTBG_MIRRORDC = 0x00000020, // assume the hdc is mirrorred and,
        DTBG_NOMIRROR = 0x00000040, // don't mirror the output, overrides everything else ,

        DTBG_VALIDBITS =
            DTBG_CLIPRECT |
            DTBG_DRAWSOLID |
            DTBG_OMITBORDER |
            DTBG_OMITCONTENT |
            DTBG_COMPUTINGREGION |
            DTBG_MIRRORDC |
            DTBG_NOMIRROR
    }
}