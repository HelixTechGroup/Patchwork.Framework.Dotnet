using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Platform.Rendering
{
    public enum RenderStage : int
    {
        PreRender = 0,
        Hardware = 1,
        Os = 2,
        Hal = 3,
        Framework = 4,
        Application = 5,
        PostRender = 6
    }
}
