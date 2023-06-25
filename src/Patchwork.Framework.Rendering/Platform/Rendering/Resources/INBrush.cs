using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public interface INBrush : INRenderResource
    {
        Color Color { get; set; }
    }
}
