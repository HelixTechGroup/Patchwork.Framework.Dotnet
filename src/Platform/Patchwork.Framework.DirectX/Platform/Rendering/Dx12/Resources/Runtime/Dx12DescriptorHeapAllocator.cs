namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources.Runtime
{
    internal sealed class Dx12DescriptorHeapAllocator : Dx12ObjectAllocator<Dx12DescriptorHeap>
    {
        private const int m_maxDescriptors = 256;
        private readonly DirectXDescriptorHeapType m_heapType;
        private int m_currentIndex;

        internal Dx12DescriptorHeapAllocator(IDX12RenderDevice d3D12Device, DirectXDescriptorHeapType heapType)
        : base(d3D12Device)
        {
            m_heapType = heapType;
            m_capacity = m_maxDescriptors;
            m_isExpandable = false;
        }

        /// <inheritdoc />
        protected override Dx12DescriptorHeap AllocateResources()
        {
            var heap = new Dx12DescriptorHeap(m_device, m_heapType);
            heap.Create();
            //heap.Initialize();
            m_currentIndex++;

            return heap;
        }

        protected override bool CreateResources(bool force)
        {
            return true;
        }

        protected override void DeallocateResources(Dx12DescriptorHeap obj)
        {
            
        }

        protected override void ExpandResources(int count)
        {
            
        }
    }
}
