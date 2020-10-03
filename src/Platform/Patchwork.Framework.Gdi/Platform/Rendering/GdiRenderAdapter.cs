#region Usings
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public sealed class GdiRenderAdapter : NRenderAdapter
    {
        #region Methods
        /// <inheritdoc />
        protected override void PlatformFlush() { }

        /// <inheritdoc />
        protected override void PlatformSwapBuffers() { }
        #endregion
    }
}