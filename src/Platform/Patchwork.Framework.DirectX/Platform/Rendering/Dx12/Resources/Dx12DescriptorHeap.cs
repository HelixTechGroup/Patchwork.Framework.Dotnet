#region Usings
using System;
using Patchwork.Framework.Platform.Rendering.Dx12.Resources.Runtime;
using Shin.Framework;
using VD3D12 = Vortice.Direct3D12;
#endregion


namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    public class Dx12DescriptorHeap : Dx12ObjectAllocator<VD3D12.ID3D12DescriptorHeap, Dx12DescriptorHeapSlot> 
    {
        #region Members
        protected readonly DirectXDescriptorHeapType m_heapType;
        protected int m_count  = 4096;
        protected VD3D12.CpuDescriptorHandle m_cpuHandle;
        protected VD3D12.DescriptorHeapDescription m_description;
        protected VD3D12.GpuDescriptorHandle m_gpuHandle;
        protected int m_slotSize;
        #endregion

        internal VD3D12.CpuDescriptorHandle CpuHandle
        {
            get { return m_cpuHandle; }
        }

        /// <inheritdoc />
        public Dx12DescriptorHeap(INRenderDevice device, DirectXDescriptorHeapType heapType) : base(device)
        {
            m_heapType = heapType;
        }

        public DirectXDescriptorHeapType HeapType
        {
            get { return m_heapType; }
        }

        public int SlotSize
        {
            get { return m_slotSize; }
        }

        #region Methods
        public void Create(int count)
        {
            m_count = count;
            Create(true);
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            m_description = new VD3D12.DescriptorHeapDescription((VD3D12.DescriptorHeapType)m_heapType, m_count);
            Throw.If(!(m_device as DX12RenderDevice).D3D12Device
                                                    .CreateDescriptorHeap(m_description,
                                                                          out m_resource).Success 
                   && m_resource is not null)
                 .InvalidOperationException();

            m_slotSize = ((DX12RenderDevice)m_device).D3D12Device
                                                                .GetDescriptorHandleIncrementSize((VD3D12.DescriptorHeapType)m_heapType);
            m_cpuHandle = m_resource.GetCPUDescriptorHandleForHeapStart();
            //m_gpuHandle = m_resource.GetGPUDescriptorHandleForHeapStart();

            return base.CreateResources(force);
        }

        /// <inheritdoc />
        //protected override void InitializeResources()
        //{
        //    base.InitializeResources();
            
        //}

        /// <inheritdoc />
        protected override Dx12DescriptorHeapSlot AllocateResources()
        {
            //var v = base.AllocateResources();
            var v = new Dx12DescriptorHeapSlot(this, m_allocatedCount);
            v.Create();
            //v.Initialize();

            return v;
        }

        protected override void DeallocateResources(Dx12DescriptorHeapSlot obj)
        {
        }

        protected override void ExpandResources(int count)
        {
            
        }
        #endregion

        /// <inheritdoc />
        //protected override object PlatformClone()
        //{
        //    throw new NotImplementedException();
        //}
    }
}