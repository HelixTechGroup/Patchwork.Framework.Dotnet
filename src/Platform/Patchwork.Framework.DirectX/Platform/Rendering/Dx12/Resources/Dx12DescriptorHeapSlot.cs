using System;
using Patchwork.Framework.Platform.Rendering.Dx12.Extensions;
using Patchwork.Framework.Platform.Rendering.Resources;
using VD3D12 = Vortice.Direct3D12;


namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    public class Dx12DescriptorHeapSlot : NRenderResource<VD3D12.CpuDescriptorHandle>, 
                                          IEquatable<Dx12DescriptorHeapSlot>
    {
        protected readonly Dx12DescriptorHeap m_heap;
        protected readonly int m_index;

        /// <inheritdoc />
        public Dx12DescriptorHeapSlot(Dx12DescriptorHeap heap, int index) : base(heap.Device)
        {
            m_heap = heap;
            m_index = index;
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            //if (force)
            //m_heap.Allocate();
            m_resource = m_heap.CpuHandle + (m_heap.SlotSize * m_index);
            return true;
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_handle = new NHandle(m_resource.ToIntPtr());
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_heap.Deallocate(this);
            m_resource = VD3D12.CpuDescriptorHandle.Default;
            base.DisposeManagedResources();
        }
        
        /// <inheritdoc />
        //protected override object PlatformClone()
        //{
        //    return new Dx12DescriptorHeapSlot(m_heap, m_index);
        //}

        /// <inheritdoc />
        public bool Equals(Dx12DescriptorHeapSlot other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return m_index == other.m_index &&
                   m_heap.CpuHandle == other.m_heap.CpuHandle;
            //Equals(m_heap.CpuHandle, other.m_heap.CpuHandle) && m_index == other.m_index;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Dx12DescriptorHeapSlot)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(m_heap, m_index);
        }
    }
}