using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INRenderContext : IInitialize, IDispose
    {
        INHandle CurrentContext { get; }

        INHandle this[INWindow window] { get; }

        INHandle Create(INWindow window);

        void Destroy(INWindow window);

        INHandle Clone(INWindow window);
    }
}
