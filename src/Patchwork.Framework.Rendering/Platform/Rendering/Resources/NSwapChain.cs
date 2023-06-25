using System;
using System.Collections.Generic;
using Patchwork.Framework.Platform.Rendering.Dxgi.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.Extensions;
using Shin.Framework;

using Vortice.DXGI;

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public abstract class NSwapChain<TNative> : NRenderResource<TNative>, INSwapChain
    {
        protected INWindow m_window;
        protected IEnumerable<INRenderResource> m_buffers;
        protected int m_backBufferIndex;

        protected NSwapChain(INRenderDevice device) : base(device) { }

        /// <inheritdoc />
        public event EventHandler Presenting;

        /// <inheritdoc />
        public event EventHandler Presented;

        /// <inheritdoc />
        public INWindow Window
        {
            get { return m_window; }
        }

        /// <inheritdoc />
        public IEnumerable<INRenderResource> Buffers
        {
            get { return m_buffers; }
        }

        /// <inheritdoc />
        public virtual int BackBufferIndex
        {
            get { return m_backBufferIndex; }
        }

        /// <inheritdoc />
        public void Bind(INWindow window)
        {
            PlatformBind(window);
        }

        protected abstract void PlatformBind(INWindow window);

        /// <inheritdoc />
        public void Unbind(INWindow window)
        {
            PlatformUnbind(window);
        }

        protected abstract void PlatformUnbind(INWindow window);

        /// <inheritdoc />
        public void Present()
        {
            Presenting.Raise(this, null);
            PlatformPresent();
            Presented.Raise(this, null);
        }

        protected abstract void PlatformPresent();
    }

    public abstract class NSwapChain : NRenderResource, INSwapChain
    {
        protected INWindow m_window;
        protected IEnumerable<INRenderResource> m_buffers;
        protected int m_backBufferIndex;

        protected NSwapChain(INRenderDevice device) : base(device) { }

        /// <inheritdoc />
        public event EventHandler Presenting;

        /// <inheritdoc />
        public event EventHandler Presented;

        /// <inheritdoc />
        public INWindow Window
        {
            get { return m_window; }
        }

        /// <inheritdoc />
        public IEnumerable<INRenderResource> Buffers
        {
            get { return m_buffers; }
        }

        /// <inheritdoc />
        public int BackBufferIndex
        {
            get { return m_backBufferIndex; }
        }

        /// <inheritdoc />
        public void Bind(INWindow window)
        {
            PlatformBind(window);
        }

        protected abstract void PlatformBind(INWindow window);

        /// <inheritdoc />
        public void Unbind(INWindow window)
        {
            PlatformUnbind(window);
        }

        protected abstract void PlatformUnbind(INWindow window);

        /// <inheritdoc />
        public void Present()
        {
            PlatformPresent();
        }

        protected abstract void PlatformPresent();
    }
}