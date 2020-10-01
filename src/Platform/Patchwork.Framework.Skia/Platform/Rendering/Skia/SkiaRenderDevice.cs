using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shin.Framework;

namespace Patchwork.Framework.Platform.Rendering.Skia
{
    public sealed class SkiaRenderDevice : NRenderDevice<SkiaRenderAdapter>
    {
        protected override TRenderer PlatformCreateRenderer<TRenderer>()
        {
            var renderer = Core.IoCContainer.Resolve<TRenderer>();
            renderer.Paint += (sender, rectangle) => { };

            return renderer;
        }
    }
}
