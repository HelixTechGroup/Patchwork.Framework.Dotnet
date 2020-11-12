using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Platform.Rendering
{
    public enum RenderLevel
    {
        PreRender,
        Hardware,
        Os,
        Hal,
        Framework,
        Application,
        PostRender
    }
}
