using System;
using Shin.Framework.Extensions;
using VD3D12 = Vortice.Direct3D12;

namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    public abstract class Dx12Object<T> : NDisposableRenderResource<T>, 
                                          IReset 
        where T : VD3D12.ID3D12Object
    {
        protected bool m_isReset;

        /// <inheritdoc />
        protected Dx12Object(INRenderDevice device) : base(device)
        {
            m_postInitialize = true;
            m_preInitialize = false;
        }

        internal Dx12Object(INRenderDevice device, T resource) : base(device, resource)
        {
            m_postInitialize = true;
            m_preInitialize = false;
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            
            if (m_resource is not null)
                m_handle = new NHandle(m_resource.NativePointer);
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            if (m_resource is not null)
                m_handle = new NHandle(m_resource.NativePointer);
            return true;
        }

        /// <inheritdoc />
        public event EventHandler Resetting;

        /// <inheritdoc />
        public bool IsReset
        {
            get { return m_isReset; }
        }

        /// <inheritdoc />
        public void Reset()
        {
            //if (m_isReset)
            //    return;
            
            lock(m_lock)
            {
                Resetting.Raise(this, EventArgs.Empty);
                PlatformReset();
                m_isReset = true;
            }
        }
        
        protected abstract void PlatformReset();
    }
}