using System;
using System.Drawing;
using Patchwork.Framework.Platform.Interop.User32;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public abstract class NRenderer : Initializable, INRenderer
    {
        protected INRenderDevice m_device;
        protected INScreen m_screen;
        protected Size m_size;
        protected Size m_virutalSize;

        /// <inheritdoc />
        public event EventHandler<Rectangle> Paint;

        /// <inheritdoc />
        public event EventHandler Painted;

        /// <inheritdoc />
        public event EventHandler Painting;

        /// <inheritdoc />
        public INScreen Screen
        {
            get { return m_screen; }
        }

        /// <inheritdoc />
        public Size Size
        {
            get { return m_size; }
        }

        /// <inheritdoc />
        public Size VirutalSize
        {
            get { return m_virutalSize; }
        }

        public bool Invalidate()
        {
            return PlatformInvalidate();
        }

        public bool Validate()
        {
            return PlatformValidate();
        }

        protected abstract bool PlatformValidate();

        protected abstract bool PlatformInvalidate();
    }
}