using System;
using Shin.Framework;

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract partial class NRenderAdapter : Initializable, INRenderAdapter
    {
        private INRenderAdapterConfiguration m_configuration;
        private INScreen m_screen;
        private INRenderDevice m_device;
        private INResourceFactory m_resourceFactory;

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

        /// <inheritdoc />
        public INRenderDevice Device
        {
            get { return m_device; }
        }

        /// <inheritdoc />
        public INRenderAdapterConfiguration Configuration
        {
            get { return m_configuration; }
        }

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

        protected abstract void PlatformFlush();

        protected abstract void PlatformSwapBuffers();
    }
}
