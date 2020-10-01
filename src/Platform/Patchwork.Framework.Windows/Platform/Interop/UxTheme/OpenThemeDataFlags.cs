using System;

namespace Patchwork.Framework.Platform.Interop.UxTheme
{
    [Flags]
    public enum OpenThemeDataFlags
    {
        OTD_FORCE_RECT_SIZING = 0x00000001, // make all parts size to rect,
        OTD_NONCLIENT = 0x00000002 // set if hTheme to be used for nonclient area,
    }
}