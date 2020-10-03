#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class SkiaRenderAdapter : NRenderAdapter
    {
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