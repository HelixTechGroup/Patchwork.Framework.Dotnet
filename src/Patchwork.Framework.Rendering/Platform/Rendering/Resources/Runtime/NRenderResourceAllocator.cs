using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Runtime;

namespace Patchwork.Framework.Platform.Rendering.Runtime
{
    public abstract class NRenderResourceAllocator<T> : NResourceAllocator<T> where T : class, INRenderResource
    {
        protected readonly INRenderDevice m_device;

        /// <inheritdoc />
        public INRenderDevice Device
        {
            get { return m_device; }
        }

        protected NRenderResourceAllocator(INRenderDevice device)
        {
            m_device = device;
        }
    }

    public abstract class NRenderResourceAllocator<TNative, TResource> : NResourceAllocator<TNative, TResource>, INRenderResource<TNative>
        where TResource : class, INRenderResource
    {
        protected readonly INRenderDevice m_device;

        /// <inheritdoc />
        public INRenderDevice Device
        {
            get { return m_device; }
        }

        protected NRenderResourceAllocator(INRenderDevice device)
        {
            m_device = device;
        }
    }
}
