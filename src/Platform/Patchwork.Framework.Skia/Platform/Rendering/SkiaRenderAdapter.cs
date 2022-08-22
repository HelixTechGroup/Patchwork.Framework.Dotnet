#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class SkiaRenderAdapter : NRenderAdapter
    {
        /// <inheritdoc />
        public SkiaRenderAdapter(INRenderDevice device, INResourceFactory factory) : base(device, factory) { }

        #region Methods
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
        #endregion
    }
}