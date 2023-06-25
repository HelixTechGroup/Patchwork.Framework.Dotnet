#region Usings
using System.Drawing;
using Patchwork.Framework.Platform.Rendering.Resources;
using VD3D12 = Vortice.Direct3D12;
#endregion

namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    public class Dx12CommandList<T> : Dx12Object<T>,
                                      INRenderCommandList<T>
        where T : VD3D12.ID3D12CommandList
    {
        #region Members
        protected VD3D12.ID3D12CommandAllocator m_commandAllocator;
        protected bool m_ownsAllocator;
        protected INRenderCommandQueue m_queue;
        protected NRenderCommandType m_type;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INRenderCommandQueue Queue
        {
            get { return m_queue; }
        }

        public NRenderCommandType Type
        {
            get { return m_type; }
        }
        #endregion

        /// <inheritdoc />
        public Dx12CommandList(INRenderDevice device, NRenderCommandType type) : base(device)
        {
            m_type = type;
        }

        public Dx12CommandList(INRenderCommandQueue queue) : this(queue.Device, queue.Type)
        {
            m_queue = queue;
        }

        /// <inheritdoc />
        internal Dx12CommandList(INRenderDevice device, NRenderCommandType type, VD3D12.ID3D12CommandAllocator commandAllocator) : this(device, type)
        {
            m_commandAllocator = commandAllocator.QueryInterface<VD3D12.ID3D12CommandAllocator>();
        }

        internal Dx12CommandList(INRenderCommandQueue queue, T resource) : base(queue.Device, resource)
        {
            m_queue = queue;
            m_type = m_queue.Type;
        }

        internal Dx12CommandList(INRenderCommandQueue queue, T resource, VD3D12.ID3D12CommandAllocator commandAllocator) :
            base(queue.Device, resource)
        {
            m_queue = queue;
            m_type = m_queue.Type;
            m_commandAllocator = commandAllocator.QueryInterface<VD3D12.ID3D12CommandAllocator>();
        }

        #region Methods
        /// <inheritdoc />
        public void Begin()
        {
            lock(m_lock)
            {
                PlatformBegin();
            }
        }

        /// <inheritdoc />
        public void Execute()
        {
            lock(m_lock)
            {
                m_queue?.Execute(this);
            }
        }

        /// <inheritdoc />
        public void End()
        {
            lock(m_lock)
            {
                PlatformEnd();
            }
        }

        /// <inheritdoc />
        public void Clear(Color color)
        {
            lock(m_lock)
            {
                PlatformClear(color);
            }
        }

        protected virtual void PlatformBegin()
        {
            //PlatformReset();
        }

        protected virtual void PlatformClear(Color color) { }

        protected virtual void PlatformEnd()
        {
        }

        protected override void PlatformReset()
        {
            m_commandAllocator?.Reset();
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            var d = (DX12RenderDevice)m_device;
            m_queue ??= d.CreateCommandQueue(m_type);
            m_ownsAllocator = m_commandAllocator is null;
            m_commandAllocator ??= d.D3D12Device.CreateCommandAllocator((VD3D12.CommandListType)m_type)
                                    .QueryInterface<VD3D12.ID3D12CommandAllocator>();
            m_commandAllocator.Name ??= $"{m_name}-CommandAllocator";
            if (d.D3D12Device.CreateCommandList(0,
                                                (VD3D12.CommandListType)m_type,
                                                m_commandAllocator,
                                                null,
                                                out m_resource).Success)
                m_resource.Name = m_name;
            else
                return false;

            return base.CreateResources(force);
        }

        /// <inheritdoc />
        //protected override bool PlatformClone(out object clone)
        //{
        //    clone = new Dx12CommandList<T>(m_device, m_type, m_commandAllocator);
        //    return true;
        //}

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            if (m_ownsAllocator)
                m_commandAllocator?.Dispose();
            
            m_resource?.Dispose();

            base.DisposeManagedResources();
        }
        #endregion
    }
}