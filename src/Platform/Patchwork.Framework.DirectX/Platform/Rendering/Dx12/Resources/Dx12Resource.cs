#region Usings
using System;
using VD3D12 = Vortice.Direct3D12;
#endregion

namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    public abstract class Dx12Resource<T> : Dx12Object<T> where T : VD3D12.ID3D12Resource
    {
        #region Members
        INHandle m_cpuHandle;
        INHandle m_gpuHandle;
        #endregion

        /// <inheritdoc />
        protected Dx12Resource(INRenderDevice device) : base(device) { }

        internal Dx12Resource(INRenderDevice device, T resource) : base(device, resource) { }

        #region Methods
        public INHandle Map(int subResource)
        {
            
            var range = new VD3D12.Range();
            unsafe
            {
                var data = IntPtr.Zero;
                m_resource.Map(subResource, (void*)data);
                return m_cpuHandle = new NHandle(data);
            }
        }

        /// <inheritdoc />
        //protected override void InitializeResources()
        //{
        //    base.InitializeResources();
        //    m_gpuHandle = new NHandle(new IntPtr((long)m_resource.GPUVirtualAddress));
        //}
        #endregion
    }
}