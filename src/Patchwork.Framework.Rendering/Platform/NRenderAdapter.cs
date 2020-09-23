using System;
using System.Drawing;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public partial class NRenderAdapter : Initializable, INRenderAdapter
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
        public void DrawLine(Point p1, Point p2)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Clear(Color color)
        {
            throw new NotImplementedException();
        }
    }
}
