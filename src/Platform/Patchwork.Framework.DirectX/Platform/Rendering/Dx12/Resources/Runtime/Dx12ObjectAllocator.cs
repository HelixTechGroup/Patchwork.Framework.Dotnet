using System;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Rendering.Runtime;
using VD3D12 = Vortice.Direct3D12;


namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources.Runtime
{
    public abstract class Dx12ObjectAllocator<T> : NRenderResourceAllocator<T> where T : class, INRenderResource
    {
        /// <inheritdoc />
        protected Dx12ObjectAllocator(INRenderDevice device) : base(device) { }
    }

    public abstract class Dx12ObjectAllocator<TNative, TResource> : NRenderResourceAllocator<TNative, TResource> 
        where TNative : VD3D12.ID3D12Object
        where TResource : class, INRenderResource
    {
        /// <inheritdoc />
        protected Dx12ObjectAllocator(INRenderDevice device) : base(device) { }

        /// <inheritdoc />
        //protected override void InitializeResources()
        //{
        //    base.InitializeResources();
            
        //}
        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            m_handle = new NHandle(m_resource.NativePointer);
            return true;
        }
    }
}