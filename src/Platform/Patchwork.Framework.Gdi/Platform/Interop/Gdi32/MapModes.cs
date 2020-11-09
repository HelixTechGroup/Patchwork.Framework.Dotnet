using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    public enum MapModes : int

    {

        MM_TEXT = 1,
        MM_LOMETRIC = 2,
        MM_HIMETRIC = 3,
        MM_LOENGLISH = 4,
        MM_HIENGLISH = 5,
        MM_TWIPS = 6,
        MM_ISOTROPIC = 7,
        MM_ANISOTROPIC = 8,

        //Minimum and Maximum Mapping Mode values
        MM_MIN = MM_TEXT,
        MM_MAX = MM_ANISOTROPIC,
        MM_MAX_FIXEDSCALE = MM_TWIPS

    }
}
