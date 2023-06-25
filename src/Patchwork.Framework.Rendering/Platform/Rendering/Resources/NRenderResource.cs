#region Usings
#endregion

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public abstract class NRenderResource : NResource, INRenderResource
    {
        #region Events
        protected NRenderResource(INRenderDevice device)
        {
            m_device = device;
        }

        //public void Create(INWindow window)
        //{
        //    //m_device = device as GdiDevice;
        //    m_window = window;
        //    m_isCreated = false;
        //    m_isInitialized = false;
        //    m_isDisposed = false;
        //    Create();
        //}
        ///// <inheritdoc />
        //public event EventHandler Rendered;

        ///// <inheritdoc />
        //public event EventHandler Rendering;
        #endregion

        #region Members
        protected INRenderDevice m_device;
        //private Size m_renderSize;
        //private Size m_size;
        #endregion

        #region Properties
        public INRenderDevice Device
        {
            get { return m_device; }
        }

        ///// <inheritdoc />
        //public Size RenderSize
        //{
        //    get { return m_renderSize; }
        //}

        ///// <inheritdoc />
        //public Size Size
        //{
        //    get { return m_size; }
        //}
        //#endregion

        //#region Methods
        ///// <inheritdoc />
        //public void Render()
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }

    public abstract class NRenderResource<TNative> : NResource<TNative>, INRenderResource<TNative>
    {
        #region Events
        ///// <inheritdoc />
        //public event EventHandler Rendered;

        ///// <inheritdoc />
        //public event EventHandler Rendering;
        #endregion

        #region Members
        protected INRenderDevice m_device;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INRenderDevice Device
        {
            get { return m_device; }
        }
        #endregion

        protected NRenderResource(INRenderDevice  device)
        {
            m_device = device;
        }

        protected NRenderResource(INRenderDevice device, TNative resource) : base(resource)
        {
            m_device = device;
        }

        #region Methods
        ///// <inheritdoc />
        //public void Render()
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }

    //public abstract class NRenderResource<TNDevice> : NResource, INRenderResource<TNDevice>
    //where TNDevice : INRenderDevice
    //{

    //}
}