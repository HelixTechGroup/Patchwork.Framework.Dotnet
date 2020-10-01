using System;
using System.Collections.Generic;
using System.Linq;
using Shin.Framework;

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderDevice<TAdapter> : Initializable, INRenderDevice<TAdapter> where TAdapter : INRenderAdapter
    {
        protected INRenderAdapter m_adapter;
        protected Priority m_priority;
        protected IEnumerable<Type> m_supportedRenderers;

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceLost;

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceReset;

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceResetting;

        /// <inheritdoc />
        public event EventHandler<ResourceCreatedEventArgs> ResourceCreated;

        /// <inheritdoc />
        public event EventHandler<ResourceDestroyedEventArgs> ResourceDestroyed;

        /// <inheritdoc />
        public INRenderAdapter Adapter
        {
            get { return m_adapter; }
        }

        /// <inheritdoc />
        public Priority Priority
        {
            get { return m_priority; }
        }

        /// <inheritdoc />
        public IEnumerable<Type> SupportedRenderers
        {
            get { return m_supportedRenderers; }
        }

        /// <inheritdoc />
        public TRenderer GetRenderer<TRenderer>() where TRenderer : INRenderer
        {
            Throw.IfNot<NotSupportedException>(m_supportedRenderers.Contains(typeof(TRenderer)));
            return PlatformCreateRenderer<TRenderer>();
        }

        protected abstract TRenderer PlatformCreateRenderer<TRenderer>() where TRenderer : INRenderer;
    }
}