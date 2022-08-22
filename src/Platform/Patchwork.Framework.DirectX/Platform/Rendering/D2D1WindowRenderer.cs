using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Platform.Rendering
{
    public class D2D1WindowRenderer : NWindowRenderer
    {
        /// <inheritdoc />
        public D2D1WindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window) { }

        /// <inheritdoc />
        protected override bool PlatformValidate()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override bool PlatformInvalidate()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformRender()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformRendering()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformRendered()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void OnSizeChanged(object sender, PropertyChangedEventArgs<Size> e)
        {
            throw new NotImplementedException();
        }
    }
}
