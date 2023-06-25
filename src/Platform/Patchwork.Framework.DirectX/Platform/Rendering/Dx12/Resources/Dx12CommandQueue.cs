#region Usings
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Patchwork.Framework.Platform.Rendering.Dx12.Resources.Collections;
using Patchwork.Framework.Platform.Rendering.Resources;
using Shin.Framework.Extensions;
using VD3D12 = Vortice.Direct3D12;
#endregion

namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    public sealed class Dx12CommandQueue : Dx12Object<VD3D12.ID3D12CommandQueue>,
                                           INRenderCommandQueue
    {
        #region Members
        //private INRenderCo
        private VD3D12.ID3D12CommandAllocator m_allocator;
        private Dx12GraphicsCommandListSet m_commandLists;
        private readonly NRenderCommandType m_type;
        protected bool m_hasBegun;
        #endregion

        #region Properties
        /// <inheritdoc />
        public IEnumerable<INRenderCommandList> CommandLists
        {
            get { return m_commandLists.CommandLists; }
        }

        public NRenderCommandType Type
        {
            get { return m_type; }
        }
        #endregion

        /// <inheritdoc />
        public Dx12CommandQueue(INRenderDevice device, NRenderCommandType type) : base(device)
        {
            m_type = type;
            m_commandLists = new Dx12GraphicsCommandListSet(this);
            m_commandLists.CreateCommandList(this);
        }

        internal Dx12CommandQueue(INRenderDevice device, VD3D12.ID3D12CommandQueue resource) : base(device, resource)
        {
            m_type = (NRenderCommandType)resource.GetDescription().Type;
        }

        #region Methods
        public INRenderCommandList CreateCommandList()
        {
            return CreateCommandList<Dx12GraphicsCommandList>();
        }

        /// <inheritdoc />
        public void Begin()
        {
            if ((!m_isCreated || !m_isInitialized) || m_hasBegun)
                return;

            lock(m_lock)
            {
                //m_resource.BeginEvent("");
                m_commandLists.Begin();
                m_hasBegun = true;   
            }
        }

        /// <inheritdoc />
        public void End()
        {
            if ((!m_isCreated || !m_isInitialized) || !m_hasBegun)
                return;

            lock(m_lock)
            {
                //m_resource.EndEvent();
                m_commandLists.End();
                m_hasBegun = false;
            }
        }

        /// <inheritdoc />
        protected override void PlatformReset()
        {
            m_commandLists.Reset();
            //m_commandLists.Keys
            //m_commandLists.Clear();
            //m_commandLists.CreateCommandList(this);
        }

        /// <inheritdoc />
        public void Execute(params INRenderCommandList[] commandLists)
        {
            if ((!m_isCreated || !m_isInitialized) || m_hasBegun)
                return;

            //var l = commandLists as IDx12GraphicsCommandList[];
            //var c = m_commandLists.Last().Value;
            //c.AddRange(commandLists.OfType<Dx12GraphicsCommandList>());
            var last = commandLists.Length - 1;
            //if (l?.Length > 0)
                m_commandLists.TryAdd(last, commandLists);
            //else
            //{
            //    var i = (Dx12CommandListSet<VD3D12.ID3D12GraphicsCommandList>.CommandListSetItem) commandLists
            //    m_commandLists.TryAdd(last, commandLists)
            //}
            var lc = m_commandLists.Compile();
            m_resource.ExecuteCommandLists(lc);
            //m_commandLists.Clear();
            
        }

        /// <inheritdoc />
        public void Execute(IEnumerable<INRenderCommandList> commandLists)
        {
            Execute(commandLists.ToArray());
        }

        /// <inheritdoc />
        public bool Wait(INRenderFence fence)
        {
            if (!m_isCreated || !m_isInitialized)
                return false;

            return m_resource.Wait(fence.Resource as VD3D12.ID3D12Fence, (ulong)fence.CompletedValue).Success;
        }

        /// <inheritdoc />
        public bool Signal(INRenderFence fence)
        {
            if (!m_isCreated || !m_isInitialized)
                return false;

            return m_resource.Signal(fence.Resource as VD3D12.ID3D12Fence, (ulong)fence.CompletedValue).Success;
        }

        /// <inheritdoc />
        public INRenderCommandList CreateCommandList<T>() where T : INRenderCommandList
        {
            if (!m_isCreated || !m_isInitialized)
                return null;

            var list = m_commandLists.CreateCommandList();
            //list.Initialize();
            //m_commandLists[0].Add(list);
            //((IDX12RenderDevice)m_device).CreateCommandList<T>();
            return list;
        }

        //public static implicit operator DX12CommandQueue(VD3D12.ID3D12CommandQueue queue)
        //{
        //    return new DX12CommandQueue(m_device, queue);
        //}

        //protected DX12CommandQueue(INRenderDevice device, VD3D12.ID3D12CommandQueue queue) : base(device)
        //{
        //    m_resource = queue;
        //    m_isCreated = true;
        //}

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            var desc = new VD3D12.CommandQueueDescription((VD3D12.CommandListType)m_type,
                                                          VD3D12.CommandQueuePriority.Normal);
            return ((DX12RenderDevice)m_device).D3D12Device
                                               .CreateCommandQueue(desc,
                                                                   out m_resource).Success && 
                   base.CreateResources(force);
            //m_resource = ((DX12RenderDevice)m_device).D3D12Device
            //                                         .CreateCommandQueue((VD3D12.CommandListType)m_type);
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_commandLists = new Dx12GraphicsCommandListSet(this);
            m_commandLists.CreateCommandList();
        }

        /// <inheritdoc />
        //protected override object PlatformClone()
        //{
        //    return new Dx12CommandQueue(m_device, m_type);
        //}
        #endregion
    }
}