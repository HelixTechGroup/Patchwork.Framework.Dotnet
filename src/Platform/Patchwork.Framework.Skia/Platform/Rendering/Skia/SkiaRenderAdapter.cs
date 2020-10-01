using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Platform.Rendering.Skia
{
    public class SkiaRenderAdapter : NRenderAdapter
    {
        /// <inheritdoc />
        protected override void PlatformFlush()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformSwapBuffers()
        {
            throw new NotImplementedException();
        }
    }
}
