using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Windowing;

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INContextDevice : INRenderDevice
    {
        INRenderContext CurrentContext { get; }

        INRenderContext CreateContext(INWindow window);
    }
}
