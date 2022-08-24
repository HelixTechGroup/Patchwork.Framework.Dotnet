using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Rendering;

namespace Patchwork.Framework.Platform.Rendering
{
    public class D2D1Adapter : NRenderAdapter
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

        /// <inheritdoc />
        public D2D1Adapter(INRenderDevice device, INResourceFactory factory) : base(device, factory) { }
    }
}
