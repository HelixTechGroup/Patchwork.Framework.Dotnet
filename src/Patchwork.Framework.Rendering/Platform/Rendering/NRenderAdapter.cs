#region Usings
using System;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderAdapter : Initializable, INRenderAdapter
    {
        #region Members
        protected INRenderAdapterConfiguration m_configuration;
        protected INRenderDevice m_device;
        protected INResourceFactory m_resourceFactory;
        protected INScreen m_screen;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INRenderAdapterConfiguration Configuration
        {
            get { return m_configuration; }
        }

        /// <inheritdoc />
        public INRenderDevice Device
        {
            get { return m_device; }
        }

        /// <inheritdoc />
        public INResourceFactory ResourceFactory
        {
            get { return m_resourceFactory; }
        }

        /// <inheritdoc />
        public INScreen Screen
        {
            get { return m_screen; }
        }
        #endregion

        protected NRenderAdapter(INRenderDevice device, INResourceFactory factory)
        {
            m_device = device;
            m_resourceFactory = factory;
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

        /// <inheritdoc />
        public TResource CreateResource<TResource>(params object[] parameters) where TResource : class, INRenderResource
        {
            return m_resourceFactory.Create<TResource>(parameters);
        }

        protected abstract void PlatformFlush();

        protected abstract void PlatformSwapBuffers();

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_resourceFactory.Initialize();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_resourceFactory.Dispose();

            base.DisposeManagedResources();
        }
        #endregion
    }
}