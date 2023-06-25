using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Rendering.Resources;

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NDisposableRenderResource<T> : NDisposableResource<T>, INRenderResource where T : IDisposable
    {
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

        protected NDisposableRenderResource(INRenderDevice device)
        {
            m_device = device;
        }

        internal NDisposableRenderResource(INRenderDevice device, T resource) : base(resource)
        {
            m_device = device;
        }
    }
}
