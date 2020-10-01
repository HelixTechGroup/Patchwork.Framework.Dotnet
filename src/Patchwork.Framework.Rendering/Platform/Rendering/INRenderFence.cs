using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Platform
{
    public interface INRenderFence : INResource
    {
        bool Signaled { get; }

        void Reset();
    }
}
