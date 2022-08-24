using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Vortice.Direct2D1;

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public class D2D1RenderTarget : NResource<ID2D1HwndRenderTarget>, INRenderResource<ID2D1HwndRenderTarget>
    {
        /// <inheritdoc />
        protected override object PlatformClone()
        {
            return new D2D1RenderTarget();
        }
    }
}
