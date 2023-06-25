namespace Patchwork.Framework.Platform.Rendering.Dxgi.Resources
{
    public abstract class DxgiObject<T> : NDisposableRenderResource<T> where T : Vortice.DXGI.IDXGIObject
    {
        /// <inheritdoc />
        protected DxgiObject(INRenderDevice device) : base(device) { }

        #region Methods
        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_handle = new NHandle(m_resource.NativePointer);
        }
        #endregion
    }
}