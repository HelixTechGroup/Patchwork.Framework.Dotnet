using System;
using System.Collections.Generic;
using Patchwork.Framework.Platform.Windowing;

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public interface INSwapChain : INRenderResource
    {
        event EventHandler Presenting;

        event EventHandler Presented;

        INWindow Window { get; }

        IEnumerable<INRenderResource> Buffers { get; }

        int BackBufferIndex { get; }

        void Bind(INWindow window);

        void Unbind(INWindow window);

        void Present();
    }
}