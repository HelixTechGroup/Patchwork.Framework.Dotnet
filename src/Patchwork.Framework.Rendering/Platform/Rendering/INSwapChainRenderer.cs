using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Rendering.Resources;

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INSwapChainRenderer : INRender
    {
        INSwapChain SwapChain { get; }
    }
}
