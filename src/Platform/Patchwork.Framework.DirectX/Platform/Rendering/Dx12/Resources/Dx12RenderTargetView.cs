#region Usings
using System;
using System.Threading;
using Shin.Framework;
using Vortice.DXGI;
using VD3D12 = Vortice.Direct3D12;
#endregion


namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    internal class Dx12RenderTargetView : Dx12Resource<VD3D12.ID3D12Resource>
    {
        #region Members
        //private VD3D12.CpuDescriptorHandle m_cpuHandle;
        //private readonly int m_offset;
        //private int m_size;
        protected Dx12DescriptorHeapSlot m_slot;
        protected bool m_ownsSlot;
        #endregion

        public Dx12RenderTargetView(Dx12DescriptorHeap heap) :
            base(heap.Device)
        {
            m_slot = heap.Allocate();
            m_ownsSlot = true;
        }

        /// <inheritdoc />
        public Dx12RenderTargetView(Dx12DescriptorHeapSlot heapSlot) :
            base(heapSlot.Device)
        {
            m_slot = heapSlot;
        }

        internal Dx12RenderTargetView(Dx12DescriptorHeapSlot heapSlot, VD3D12.ID3D12Resource d3D12Resource) :
            base(heapSlot.Device, d3D12Resource)
        {
            m_slot = heapSlot;
        }

        #region Methods
        public void Create(VD3D12.ID3D12Resource buffer)
        {
            lock(m_lock)
            {
                if (m_resource is not null && 
                    m_resource?.NativePointer != IntPtr.Zero)
                {
                    m_resource?.Dispose(); 
                }

                Interlocked.Exchange(ref m_resource, 
                                     buffer.QueryInterface<VD3D12.ID3D12Resource>());
                //if (m_resource.)
            }

            m_isCreated = m_isInitialized = false;
            Create();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            if (m_ownsSlot)
                m_slot.Dispose();
            
            m_resource.Dispose();
            base.DisposeManagedResources();
        }

        //internal DX12RenderTargetView(INRenderDevice dx12Device, 
        //                              VD3D12.CpuDescriptorHandle start, 
        //                              int size, 
        //                              int offset) : base(dx12Device)
        //{
        //    m_startHandle = start;
        //    m_size = size;
        //    m_offset = offset;
        //}

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            Throw.If(m_resource is null ||
                     m_resource.NativePointer == IntPtr.Zero).InvalidOperationException();

            //var handle = m_descriptorHeap.GetCPUDescriptorHandleForHeapStart();
            //var sHeapSize = m_dx12Device.GetDescriptorHandleIncrementSize(VD3D12.DescriptorHeapType.RenderTargetView);
            //var heapSize = (sHeapSize * m_backBufferIndex);

            //var dx12Device = m_device as DX12RenderDevice;
            //m_cpuHandle = dx12Device.D3D12Heap.GetCPUDescriptorHandleForHeapStart();
            //m_size = dx12Device.D3D12Device.GetDescriptorHandleIncrementSize(VD3D12.DescriptorHeapType.RenderTargetView) * (m_offset + 1);

            //m_cpuHandle = m_offset >= 0 ? m_cpuHandle : (m_cpuHandle += m_size);
            ((DX12RenderDevice)m_device).D3D12Device
                                        .CreateRenderTargetView(m_resource,
                                                                new VD3D12.RenderTargetViewDescription
                                                                {
                                                                    Format = Format.B8G8R8A8_UNorm,
                                                                    ViewDimension = VD3D12.RenderTargetViewDimension.Texture2D,
                                                                },
                                                                m_slot.Resource);

            //m_resource = new VD3D12.CpuDescriptorHandle(m_startHandle, m_offset, m_size);

            return base.CreateResources(force);
        }

        /// <inheritdoc />
        protected override void PlatformReset()
        {
            m_resource.Dispose();
            m_slot.Dispose();
            m_slot.Create();
        }

        /// <inheritdoc />
        //protected override object PlatformClone()
        //{
        //    return new Dx12RenderTargetView(m_slot);
        //}
        #endregion

        internal VD3D12.CpuDescriptorHandle CpuHandle
        {
            get { return m_slot.Resource; }
        }

        //internal DX12RenderTargetView(INRenderDevice dx12Device, int offset) : base(dx12Device)
        //{
        //    m_offset = offset;
        //}
    }
}