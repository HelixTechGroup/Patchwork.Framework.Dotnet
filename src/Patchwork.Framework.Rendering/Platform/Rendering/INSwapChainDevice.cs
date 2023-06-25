using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INSwapChainDevice : INRenderDevice
    {
        INSwapChain CurrentSwapChain { get; }

        INSwapChain CreateSwapChain(INWindow window);
    }
}
