#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Platform.Rendering.Dx12.Resources.Collections;
using Patchwork.Framework.Platform.Rendering.Dx12.Resources.Runtime;
using Patchwork.Framework.Platform.Rendering.Dxgi.Resources;
using Patchwork.Framework.Platform.Rendering.Resources;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Vortice.Mathematics;
using Color = System.Drawing.Color;
using VD3D12 = Vortice.Direct3D12;
#endregion

namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    internal sealed partial class Dx12RenderFrame : NRenderTarget, INRenderFrame, IReset
    {
        #region Members
        private static readonly ReaderWriterLockSlim m_lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        //private readonly DX12RenderDevice m_dx12Device;
        private readonly int m_frameId;
        private readonly Dx12DescriptorHeap m_heap;
        private readonly int m_stageCount = 3;

        private readonly int m_threadCount = 3;

        //private ConcurrentList<VD3D12.ID3D12CommandAllocator> m_allocators;
        private Color m_backgroundColor;

        //private ConcurrentList<VD3D12.ID3D12GraphicsCommandList> m_commandLists;
        private VD3D12.ID3D12CommandAllocator m_allocator;
        private readonly Dx12GraphicsCommandListSet m_commandListSet;
        private INRenderCommandList<VD3D12.ID3D12GraphicsCommandList> m_currentList;

        private int m_currentStage;

        //private VD3D12.ID3D12Fence m_fence;
        //private Dx12Fence m_fence;
        private int m_fenceValue;

        private Dx12RenderTargetView m_target;

        //private Dx12RenderTargetView m_rtv;
        private ConcurrentList<Dx12RenderThread> m_threads;
        private bool m_isReset;
        #endregion

        #region Properties
        /// <inheritdoc />
        public Color BackgroundColor
        {
            get { return m_backgroundColor; }
            set { m_backgroundColor = value; }
        }

        /// <inheritdoc />
        public INRenderDevice Device
        {
            get { return m_device; }
        }

        /// <inheritdoc />
        public int FrameId
        {
            get { return m_frameId; }
        }

        /// <inheritdoc />
        public INRenderResource Target
        {
            get { return m_target; }
            set { m_target = value as Dx12RenderTargetView; }
        }
        //private readonly DXGISwapChain m_dxgiSwapChain;

        internal IEnumerable<INRenderCommandList> CommandLists
        {
            get { return m_commandListSet.CommandLists; }
        }

        public int FenceValue
        {
            get { return m_fenceValue; }
            set { m_fenceValue = value; }
        }

        //internal INRenderFence Fence
        //{
        //    get { return m_fence; }
        //}
        #endregion

        internal Dx12RenderFrame(Dx12CommandQueue queue, Dx12DescriptorHeap heap, int frameId) : base(heap.Device)
        {
            m_heap = heap;
            m_frameId = frameId;
            //m_dxgiSwapChain = swapChain;
            //m_allocators = new ConcurrentList<VD3D12.ID3D12CommandAllocator>();
            m_commandListSet = new Dx12GraphicsCommandListSet(queue);
            m_threads = new ConcurrentList<Dx12RenderThread>();
        }

        #region Methods
        
        public void Bind(INSwapChain swapChain, bool force = false)
        {
            lock(m_lock)
            {
                using var buffer = (swapChain as DxgiSwapChain)?
                   .Resource.GetBuffer<VD3D12.ID3D12Resource>(m_frameId);
                if (m_target is not null && force)
                {
                    m_target.Reset();
                    //m_target = null;
                }
                
                if (m_target is null)
                {
                    var slot = m_heap.Allocate();
                    m_target = new Dx12RenderTargetView(slot);
                    //m_target.Create();
                    //m_rtv.Initialize();
                }

                m_target.Create(buffer);
                
                m_size = swapChain.Window.ClientSize;
                //m_rtv.Create(buffer);
            }
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_target.Dispose();
            m_commandListSet.Dispose();
            
            base.DisposeManagedResources();
        }

        //public void Bind(INRenderTarget renderTarget)
        //{
        //    lock(m_lock)
        //    {
        //        m_target = renderTarget;
        //        m_target.Initialize();
        //    }
        //}

        public void Flush(INRenderCommandQueue queue)
        {
            lock(m_lock)
            {
                m_commandListSet.End();
                queue.Execute(m_commandListSet.CommandLists);
            }
        }

        public void Execute(params INRenderCommandList[] commands)
        {
            lock(m_lock)
            {
                if (!m_hasBegun)
                    return;

                //m_commandListSet[m_currentStage].End();
                m_currentStage = 1;
                //m_commandListSet[m_currentStage].Reset();
                //m_commandListSet[m_currentStage].Begin();
                m_currentList = (INRenderCommandList<VD3D12.ID3D12GraphicsCommandList>)m_commandListSet[m_currentStage][0];
                m_commandListSet[m_currentStage].AddRange((Dx12GraphicsCommandList[])commands);
                //m_commandListSet[m_currentStage].End();
            }
        }

        /// <inheritdoc />
        protected override bool PlatformBegin()
        {
            CreateDefaultResources();
            
            m_currentStage = 0;
            //m_commandListSet.Reset();
            m_commandListSet.Begin();
            //m_commandListSet[m_currentStage].Reset();
            //m_commandListSet[m_currentStage].Begin();

            m_currentList = (INRenderCommandList<VD3D12.ID3D12GraphicsCommandList>)m_commandListSet[m_currentStage][0];
            m_currentList.Resource.ResourceBarrierTransition(m_target.Resource,
                                                             VD3D12.ResourceStates.Present,
                                                             VD3D12.ResourceStates.RenderTarget);

            m_currentList.Resource.OMSetRenderTargets(m_target.CpuHandle);
            //m_commandListSet[m_currentStage].End();

            return true;
        }

        /// <inheritdoc />
        protected override bool PlatformEnd()
        {
            //m_commandListSet[m_currentStage].End();

            m_currentStage = 2;
            //m_commandListSet[m_currentStage].Reset();
            //m_commandListSet[m_currentStage].Begin();

            m_currentList = (INRenderCommandList<VD3D12.ID3D12GraphicsCommandList>)m_commandListSet[m_currentStage][0];
            m_currentList.Resource.ResourceBarrierTransition(m_target.Resource,
                                                             VD3D12.ResourceStates.RenderTarget,
                                                             VD3D12.ResourceStates.Present);

            //m_commandListSet[m_currentStage].End();
            m_commandListSet.End();

            return true;
        }

        /// <inheritdoc />
        protected override void PlatformFlush()
        {
            //m_commandListSet.End();
            m_commandListSet.Execute();
            //m_commandListSet.Reset();
        }

        /// <inheritdoc />
        protected override void PlatformClear(Color color)
        {
            m_currentList.Resource.ClearRenderTargetView(m_target.CpuHandle,
                                                         color.ToColor4());
        }

        /// <inheritdoc />
        protected override INRenderResource PlatformBind(INRenderResource resource)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override INRenderResource PlatformUnbind(INRenderResource resource)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            
            m_handle = new NHandle();
        }

        /// <inheritdoc />
        public event EventHandler Resetting;

        /// <inheritdoc />
        public bool IsReset
        {
            get { return m_isReset; }
        }

        public void Reset()
        {
            if (m_hasBegun)
                return;

            lock(m_lock)
            {
                m_isReset = true;
                Resetting.Raise(this, EventArgs.Empty);
                //m_target.Reset();
                m_currentList?.Resource?.OMSetRenderTargets(VD3D12.CpuDescriptorHandle.Default);
                m_commandListSet.Reset();
                //m_target.Reset();
                //CreateDefaultResources();

                //for (var i = 0; i < m_stageCount; i++)
                //{
                //    //m_allocators[i].Reset();
                //    foreach (var list in m_commandListSet[i])
                //    {
                //        list.Resource.Reset(m_allocators[i]);
                //    }
                //}

                //foreach (var thread in m_threads) thread.Reset();
                m_isReset = false;
            }
        }

        /// <inheritdoc />
        //public void Wait()
        //{
        //    lock(m_lock)
        //    {
        //        m_fence.Wait();
        //        //using var fEvent = new AutoResetEvent(false);
        //        //m_fence.SetEventOnCompletion((ulong)m_fenceValue + 1,
        //        //                             fEvent.SafeWaitHandle.DangerousGetHandle());
        //        //fEvent.WaitOne();
        //    }
        //}

        /// <inheritdoc />
        //public void Signal()
        //{
        //    lock(m_lock)
        //    {
        //        //m_fence.Signal();
        //        m_fenceValue++;
        //    }
        //}

        private void CreateDefaultResources()
        {
            for (var i = 0; i < m_stageCount; i++)
            {
                //using var ca = dx12Device.D3D12Device
                //                         .CreateCommandAllocator(VD3D12.CommandListType.Direct);
                //m_allocators.Add(ca.QueryInterface<VD3D12.ID3D12CommandAllocator>());
                
                //cl.Initialize();

                //using var cl =
                //    dx12Device.D3D12Device.CreateCommandList<VD3D12.ID3D12GraphicsCommandList>(VD3D12.CommandListType.Direct, m_allocators[i]);
                //m_commandLists[i].Add(cl.QueryInterface<VD3D12.ID3D12GraphicsCommandList>());

                //if (m_commandListSet.ContainsKey(i))
                //    m_commandListSet[i].Add(cl);
                //else
                if (!m_commandListSet.ContainsKey(i))
                {
                    var cl = new Dx12GraphicsCommandList(m_device, NRenderCommandType.Direct);
                    cl.Create();

                    m_commandListSet.TryAdd(i,
                                            new Dx12CommandListSet<VD3D12.ID3D12GraphicsCommandList>.
                                                CommandListSetItem(cl));
                }

                //m_commandLists[i].End;
            }
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            //var dx12Device = m_device as DX12RenderDevice;
            CreateDefaultResources();

            //m_fence = new Dx12Fence(m_device);
            //m_fence.Create();
            //m_fence.Initialize();

            //using var fence = dx12Device.D3D12Device.CreateFence();
            //m_fence = fence.QueryInterface<VD3D12.ID3D12Fence1>();
            //m_fenceValue = (int)fence.CompletedValue;

            //for (var t = 0; t < m_threadCount; t++)
            //    m_threads.Add(new DX12RenderThread(dx12Device.D3D12Device));

            return true;
        }
        #endregion

        ///// <inheritdoc />
        //protected override void InitializeResources()
        //{
        //    base.InitializeResources();

        //    m_allocators = new ConcurrentList<VD3D12.ID3D12CommandAllocator>();
        //    m_commandLists = new ConcurrentList<VD3D12.ID3D12GraphicsCommandList>();
        //    m_threads = new ConcurrentList<DX12RenderThread>();
        //}
    }
}