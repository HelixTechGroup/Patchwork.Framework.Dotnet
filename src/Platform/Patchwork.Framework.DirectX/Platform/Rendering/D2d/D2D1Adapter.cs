namespace Patchwork.Framework.Platform.Rendering.D2d
{
    public class D2D1Adapter : NRenderAdapter
    {
        /// <inheritdoc />
        public D2D1Adapter(INRenderDevice device) : base(device) { }

        #region Methods
        /// <inheritdoc />
        protected override void PlatformFlush() { }

        /// <inheritdoc />
        protected override void PlatformSwapBuffers() { }
        #endregion
    }
}