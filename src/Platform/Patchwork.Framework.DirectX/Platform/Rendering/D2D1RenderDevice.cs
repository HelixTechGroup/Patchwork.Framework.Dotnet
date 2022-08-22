using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.IoC.DependencyInjection;

namespace Patchwork.Framework.Platform.Rendering
{
    public class D2D1RenderDevice : NRenderDevice<D2D1RenderAdapter>
    {
        /// <inheritdoc />
        protected override void RegisterRenderers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformSetFrameBuffer(NFrameBuffer buffer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public D2D1RenderDevice(IContainer iocContainer) : base(iocContainer) { }
    }
}
