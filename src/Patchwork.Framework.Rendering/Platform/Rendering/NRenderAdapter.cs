#region Usings
using System;
using Patchwork.Framework.Platform.Rendering.Resources;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderAdapter : Initializable, INRenderAdapter
    {
        #region Members
        protected INRenderAdapterConfiguration m_configuration;
        protected INRenderDevice m_device;
        //protected INResourceFactory m_resourceFactory;
        protected INScreen m_screen;
        //private INRenderContext m_context;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INRenderAdapterConfiguration Configuration
        {
            get { return m_configuration; }
        }

        ///// <inheritdoc />
        //public INRenderContext Context
        //{
        //    get { return m_device.Context; }
        //}

        /// <inheritdoc />
        public INRenderDevice Device
        {
            get { return m_device; }
        }

        /// <inheritdoc />
        public INRenderResourceFactory Factory
        {
            get { return m_device.Resource; }
        }

        /// <inheritdoc />
        public INScreen Screen
        {
            get { return m_screen; }
        }
        #endregion

        protected NRenderAdapter(INRenderDevice device)
        {
            m_device = device;
            //m_resourceFactory = factory;
            //m_context = 
        }

        #region Methods
        /// <inheritdoc />
        public void SwapBuffers()
        {
            PlatformSwapBuffers();
        }

        /// <inheritdoc />
        public void Flush()
        {
            PlatformFlush();
        }

        ///// <inheritdoc />
        //public TResource CreateResource<TResource>(params object[] parameters) where TResource : class, INRenderResource
        //{
        //    return m_resourceFactory.Create<TResource>(parameters);
        //}

        protected abstract void PlatformFlush();

        protected abstract void PlatformSwapBuffers();
        #endregion
    }
}