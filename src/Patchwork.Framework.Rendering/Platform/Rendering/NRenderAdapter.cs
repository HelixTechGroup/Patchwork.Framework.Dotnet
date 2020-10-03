#region Usings
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderAdapter : Initializable, INRenderAdapter
    {
        #region Members
        private INRenderAdapterConfiguration m_configuration;
        private INRenderDevice m_device;
        private INResourceFactory m_resourceFactory;
        private INScreen m_screen;
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

        protected abstract void PlatformFlush();

        protected abstract void PlatformSwapBuffers();
        #endregion
    }
}