#region Usings
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Patchwork.Framework.Platform.Rendering.Dx12.Resources;
using Patchwork.Framework.Platform.Rendering.Dx12.Resources.Collections;
using Patchwork.Framework.Platform.Rendering.Dx12.Resources.Runtime;
using Patchwork.Framework.Platform.Rendering.Dxgi.Resources;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Vortice.DXGI;
using VD3D12 = Vortice.Direct3D12;
#endregion


namespace Patchwork.Framework.Platform.Rendering.Dx12
{
    public class DX12RenderContext : NRenderContext<VD3D12.ID3D12CommandQueue>,
                               INRenderContext3D
    {
        #region Members
        private readonly int m_bufferCount;
        //private readonly VD3D12.ID3D12Device4 m_dx12Device;
        //private ConcurrentList<VD3D12.ID3D12CommandAllocator> m_allocators;
        private int m_backBufferIndex;
        private ConcurrentDictionary<int, Dx12RenderFrame> m_bufferFrames;
        private Dx12CommandQueue m_queue;
        private Dx12RenderFrame m_currentBufferFrame;
        private Dx12DescriptorHeap m_rtvHeap;
        private INSwapChainDevice m_swapChainDevice;
        private int m_fenceValue;
        private Dx12Fence m_fence;
        //private ConcurrentList<VD3D12.ID3D12Resource> m_buffers;
        //private ConcurrentList<VD3D12.ID3D12GraphicsCommandList> m_commandLists;
        //private VD3D12.ID3D12CommandQueue m_commandQueue;
        //private VD3D12.ID3D12Resource m_currentBuffer;
        private int m_currentBufferIndex;

        //private VD3D12.ID3D12CommandAllocator m_currentCommandAllocator;
        //private VD3D12.ID3D12GraphicsCommandList4 m_currentCommandList;

        //private DXGIDevice m_dxgiDevice;
        //private ConcurrentList<VD3D12.ID3D12Fence> m_fences;
        //private ConcurrentList<int> m_fenceValues;

        private VD3D12.ID3D12PipelineLibrary m_pipelineLibrary;
        private Size m_size;

        //private ConcurrentList<DXGISwapChain> m_swapChains;
        #endregion

        //public D3D12Context()
        //{
        //    //CreateD3D12Device();
        //}

        internal DX12RenderContext(IDX12RenderDevice device, INSwapChainDevice swapDevice) : base(device)
        {
            m_bufferCount = device.Configuration.BufferCount;
            m_swapChainDevice = swapDevice;
            m_bufferFrames = new ConcurrentDictionary<int, Dx12RenderFrame>();
            //m_dx12Device = dx12Device.QueryInterface<VD3D12.ID3D12Device10>();
        }

        #region Methods
        private bool PlatformBind(INSwapChain swapChain, bool force = false)
        {
            Throw.IfNull(swapChain).ArgumentNullException(nameof(swapChain));
            //if (!m_isCreated || !m_isInitialized)
            //{
            //    Create();
            //    //Initialize();
            //}

            //lock(m_lock)
            //{
                InitializePipeline();

                //for (var i = 0; i < m_bufferCount; i++)
                //{
                //    m_bufferFrames[i].Bind(swapChain.Resource., force);
                //}
                foreach (var frame in m_bufferFrames.Values)
                    frame.Bind(swapChain);

                if (!force)
                {
                    //swapChain.Window.SizeChanged += (sender, args) =>
                    //                                {
                    //                                    PlatformBind(swapChain, true);
                    //                                };

                    swapChain.Presented += OnSwapChainOnPresented;
                }
                
                //m_currentBufferFrame.Bind(swapChain);
                //m_swapChains.Add(swapChain);
                m_size = swapChain.Window.ClientSize;
                return true;
            //}
        }

        private void ResizeBuffers(Size size)
        {
            if (size == m_size)
                return;
            
            var swapChain = m_swapChainDevice.CurrentSwapChain as DxgiSwapChain;
            if (swapChain is null)
                return;
            
            //m_currentBufferFrame.Target.Dispose();
            foreach (var frame in m_bufferFrames.Values)
            {
                frame?.Target?.Dispose();
            }

            var dxgichain = swapChain.Resource as IDXGISwapChain3;
            Core.Logger.LogDebug("=== D3D12 Context Resize ===");
            Core.Logger
                .LogDebug($"---Swapchain Client Area Pre-resize: {dxgichain.Description1.Width}, {dxgichain.Description1.Height}");
            Core.Logger
                .LogDebug($"---Swapchain Backbuffer index Pre-resize: {dxgichain.CurrentBackBufferIndex}");
            var r = dxgichain.ResizeBuffers(m_bufferCount,
                                            size.Width,
                                            size.Height,
                                            Format.B8G8R8A8_UNorm);
            //SwapChainFlags.None,
            //new[] {0},
            //new[] {m_resource});
            
            Core.Logger.LogDebug(@$"Resize Result: {r.Description}");
            Core.Logger
                .LogDebug($"---Window Client Area: {m_swapChainDevice.CurrentSwapChain.Window.ClientSize}");
            //Core.Logger.LogDebug(@$"---D2D1 RenderTarget Size: {m_device..Resource.Size}");
            Core.Logger
                .LogDebug($"---Swapchain Client Area: {dxgichain.Description1.Width}, {dxgichain.Description1.Height}");
            Core.Logger
                .LogDebug($"---Swapchain Backbuffer index: {dxgichain.CurrentBackBufferIndex}");
            //Core.Logger.LogDebug($"---Buffer Client Area: {b.Item2.Size}");
            Core.Logger
                .LogDebug($"---Event Client Area: {size}");
            
            if (m_currentBufferIndex != dxgichain.CurrentBackBufferIndex)
            {
                Interlocked.Exchange(ref m_backBufferIndex, dxgichain.CurrentBackBufferIndex);
                ChangeFrame();   
            }
            //m_currentBufferFrame.Bind(swapChain);
            foreach (var frame in m_bufferFrames.Values)
            {
                frame.Bind(swapChain);
            }
        }
        /// <inheritdoc />
        protected override void PlatformResize(INWindow window, Size size)
        {
            var swapchain = m_swapChainDevice.CreateSwapChain(window);
            //m_currentBufferFrame.End();
            //m_currentBufferFrame.Reset();
            var completedValue = m_fence.Resource.CompletedValue;
            m_currentBufferFrame.FenceValue = m_fenceValue;
            m_fenceValue++;
            m_queue.Signal(m_fence);
            m_fence.Wait();
            m_fence.Signal();
            //var chain = sender as INSwapChain;
            if (m_currentBufferIndex != swapchain.BackBufferIndex)
            {
                Interlocked.Exchange(ref m_backBufferIndex, swapchain.BackBufferIndex);
                ChangeFrame();
            }
            //Wait();
            ResizeBuffers(size);
            PlatformBind(swapchain);
            m_size = size;
        }

        /// <inheritdoc />
        protected override bool PlatformBind(INWindow window, bool force = false)
        {
            var swapchain = m_swapChainDevice.CreateSwapChain(window);
            return PlatformBind(swapchain, force);
        }

        /// <inheritdoc />
        protected override bool PlatformUnbind(INWindow window, bool force = false)
        {
            return true;
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            m_queue = ((INRenderCommandQueueDevice)m_device).CreateCommandQueue(NRenderCommandType.Direct) as Dx12CommandQueue;
            m_resource = m_queue?.Resource.QueryInterface<VD3D12.ID3D12CommandQueue>();
            //m_dx12Device.CreateCommandQueue<VD3D12.ID3D12CommandQueue>(VD3D12.CommandListType.Direct);

            //CreateD3D12Device();
            //using var context = m_dx12Device.create;
            //m_resource = context.QueryInterface<VD3D12.ID3D12Dev>();
            //m_originalTarget = m_resource.Target;
            m_rtvHeap = ((DX12RenderDevice)m_device).RtvHeapAllocator.Allocate();
            m_rtvHeap.Create(m_bufferCount);
            //m_rtvHeap.Initialize();

            return true;
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            WaitForCpu();

            m_fence.Dispose();
            foreach (var b in m_bufferFrames.Values)
                b.Dispose();

            m_rtvHeap.Dispose();

            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            WaitForCpu();

            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            //m_allocators = new ConcurrentList<VD3D12.ID3D12CommandAllocator>();
            //m_commandLists = new ConcurrentList<VD3D12.ID3D12GraphicsCommandList>();
            //m_fences = new ConcurrentList<VD3D12.ID3D12Fence>();
            //m_fenceValues = new ConcurrentList<int>();
            //m_buffers = new ConcurrentList<VD3D12.ID3D12Resource>();
            //m_bufferFrames = new ConcurrentList<Dx12RenderFrame>(m_bufferCount);
            //m_rtvHeap?.Initialize();
            //m_swapChains = new ConcurrentList<DXGISwapChain>();
            //InitializePipeline();
        }

        /// <inheritdoc />
        protected override bool PlatformBegin()
        {
            //using var dev = m_dx12Device.QueryInterfaceOrNull<VD3D12.ID3D12Device2>();
            //var pipeline = dev?.CreatePipelineState<VD3D12.ID3D12PipelineState>(pipeDescription);
            //var barrier = new VD3D12.ResourceBarrier(new VD3D12.ResourceTransitionBarrier(m_renderTargetViews[m_backBufferIndex],
            //                                                                              VD3D12.ResourceStates.Present,
            //                                                                              VD3D12.ResourceStates.RenderTarget));
            //m_currentCommandList.ResourceBarrier(barrier);

            //WaitForCpu();
            //WaitForGpu();
            //if (!m_fence.WasSignaled)
            //    m_fence.Wait();
            //else
            //    m_fence.Reset();

            m_queue.Reset();
            m_currentBufferFrame.Reset();
            //ResizeBuffers();
            //m_currentBufferFrame.Reset();
            //m_currentBufferFrame.Bind(m_swapChainDevice.CurrentSwapChain, true);
            m_currentBufferFrame.Begin();
            //m_currentCommandAllocator = m_allocators[m_backBufferIndex];
            //m_currentCommandAllocator.Reset();
            //m_currentCommandList.Reset(m_currentCommandAllocator);

            //m_currentCommandList = m_commandLists[m_backBufferIndex];
            //m_currentBuffer = backFrame.Resource;
            //m_currentCommandList.ResourceBarrierTransition(m_currentBuffer,
            //                                               VD3D12.ResourceStates.Present,
            //                                               VD3D12.ResourceStates.RenderTarget);

            //var handle = m_descriptorHeap.GetCPUDescriptorHandleForHeapStart();
            //var sHeapSize = m_dx12Device.D3D12Device.GetDescriptorHandleIncrementSize(VD3D12.DescriptorHeapType.RenderTargetView);
            //var heapSize = (sHeapSize * m_backBufferIndex);

            //var rtvHandle = new VD3D12.CpuDescriptorHandle(handle, m_backBufferIndex, sHeapSize);
            //handle.Ptr += (nuint)(m_currentBufferIndex > 1 ? heapSize : 0);
            //m_currentCommandList.Reset(a, pipeline);
            //m_currentCommandList.ResourceBarrier(barrier);

            //m_currentCommandList.OMSetRenderTargets(rtvHandle);
            //m_currentCommandList.ClearRenderTargetView(rtvHandle, new Color4()
            //
            return true;
        }

        /// <inheritdoc />
        protected override void PlatformClear(Color color)
        {
            //var handle = m_dx12Device.D3D12Heap.GetCPUDescriptorHandleForHeapStart();
            //var sHeapSize = m_dx12Device.D3D12Device.GetDescriptorHandleIncrementSize(VD3D12.DescriptorHeapType.RenderTargetView);
            //var heapSize = (sHeapSize * m_backBufferIndex);
            ////handle.Ptr += (nuint)(m_currentBufferIndex > 1 ? heapSize : 0);

            //var rtvHandle = new VD3D12.CpuDescriptorHandle(handle, m_backBufferIndex, sHeapSize);
            //m_currentCommandList.Reset(a, pipeline);
            //m_currentCommandList.ResourceBarrier(barrier);

            m_currentBufferFrame.Clear(color);
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
        //protected override object PlatformClone()
        //{
        //    return new DX12RenderContext(m_device as IDX12RenderDevice, m_swapChainDevice);
        //}

        /// <inheritdoc />
        protected override bool PlatformEnd()
        {
            m_currentBufferFrame.End();
            //m_currentBufferFrame.Reset();
            //m_currentBufferFrame.Flush();

            //using var buffer = m_swapChains[0]
            //                 .Resource.GetBuffer<VD3D12.ID3D12Resource>(m_backBufferIndex);

            //m_resource.ExecuteCommandLists();
            //m_currentBuffer.Release();
            //m_commandQueue.Signal(m_fences[m_backBufferIndex],
            //                      (ulong)++m_fenceValues[m_backBufferIndex]);

            //WaitForFrame();
            return true;
        }

        /// <inheritdoc />
        protected override void PlatformFlush()
        {
            m_currentBufferFrame.Flush();
            //m_currentBufferFrame.FenceValue = m_fenceValue;
            //m_queue.Signal(m_fence);
            //m_fenceValue++;
            //m_fence.Signal(m_fenceValue);

            //WaitForCpu();
        }

        private void OnSwapChainOnPresented(object sender, EventArgs args)
        {
            //lock(m_lock)
            {
                var completedValue = m_fence.Resource.CompletedValue;
                m_currentBufferFrame.FenceValue = m_fenceValue;
                m_fenceValue++;
                m_queue.Signal(m_fence);
                m_fence.Wait();
                m_fence.Signal();
                var chain = sender as INSwapChain;
                if (m_currentBufferIndex != chain.BackBufferIndex)
                {
                    Interlocked.Exchange(ref m_backBufferIndex, chain.BackBufferIndex);
                    ChangeFrame();
                }
                //Interlocked.Exchange(ref m_backBufferIndex, chain.BackBufferIndex);
                //m_currentBufferFrame = m_bufferFrames[m_backBufferIndex];
                //m_currentBufferFrame.Bind(chain, true);
                //if (m_currentBufferFrame.FenceValue < completedValue)
                //    m_currentBufferFrame.
                //Flush();
                //m_backBufferIndex = chain.BackBufferIndex; 
                //WaitForCpu();
            }
        }

        private void InitializePipeline()
        {
            //using var cl = m_dx12Device.CreateCommandList<VD3D12.ID3D12GraphicsCommandList>(VD3D12.CommandListType.Direct);
            //m_currentCommandList = cl.QueryInterface<VD3D12.ID3D12GraphicsCommandList4>();
            //m_descriptorHeap = m_dx12Device
            //   .CreateDescriptorHeap(new VD3D12.DescriptorHeapDescription(VD3D12.DescriptorHeapType.RenderTargetView,
            //                                                              m_bufferCount));

            if (m_rtvHeap is null || m_rtvHeap.IsDisposed)
                return;

            //foreach (var frame in m_bufferFrames.Values)
            //{
            //    frame.Reset();
            //}
            //m_bufferFrames.Clear();


            for (var i = 0; i < m_bufferCount; i++)
            {
                m_bufferFrames.TryAdd(i, new Dx12RenderFrame(m_queue, m_rtvHeap, i));
                m_bufferFrames[i].Create(true);
                //m_bufferFrames[i].Initialize();

                //ca.Reset();
                //ca.Release();
                //cl.Release();
                //fence.Release();
            }
            m_currentBufferFrame = m_bufferFrames[m_currentBufferIndex];
            m_fence?.Reset();
            m_fenceValue = 0;
            m_fence ??= new Dx12Fence(m_device);
            m_fence?.Create();
            //m_currentBufferFrame.Reset();
            //m_currentCommandAllocator = m_allocators[m_currentBufferIndex];

            //m_currentCommandAllocator.Reset();
            //m_currentCommandList.Reset(m_currentCommandAllocator);

            //var fence = m_dx12Device.CreateFence(0);
            //m_fences.Add(fence.QueryInterface<VD3D12.ID3D12Fence1>());
            //m_fenceValues.Add((int)fence.CompletedValue);

            //m_currentCommandList = m_commandLists[m_currentFrameIndex];
            //m_currentCommandList.Close();
        }

        private void WaitForCpu()
        {
            lock(m_lock)
            {
                while (!m_fence.WasSignaled)
                {
                    m_fence.Wait();
                }
                
                m_fence.Reset();
                m_fenceValue = 0;
                //ChangeFrame();
                
                //m_currentBufferFrame = m_bufferFrames[m_backBufferIndex];
                //if ((ulong)m_currentBufferFrame.FenceValue < completedValue)
                //{
                //    m_fence.Wait();
                //}

                // = m_swapChains[0].BackBufferIndex;
                //.First(c => c.Window
                //             .Equals(Core.Window.CurrentWindow)).BackBufferIndex;
                //m_currentFrameIndex++;
                //Interlocked.Exchange(ref m_currentBufferFrame, m_bufferFrames[m_backBufferIndex]);
                //m_currentBufferFrame.Bind(m_swapChainDevice.CurrentSwapChain, true);   
            }
        }

        private void ChangeFrame()
        {
            if (m_hasBegun)
                return;
            
            Interlocked.Exchange(ref m_currentBufferIndex,
                                 (m_currentBufferFrame.FrameId + 1) % m_bufferCount);
            //m_currentBufferFrame ??= m_bufferFrames[m_currentBufferIndex];
            //if ((ulong)m_currentBufferFrame.FenceValue < completedValue)

            //m_currentBufferFrame = m_bufferFrames[m_backBufferIndex];
            //if ((ulong)m_currentBufferFrame.FenceValue < completedValue)
            //{
            //    m_fence.Wait();
            //}

            // = m_swapChains[0].BackBufferIndex;
            //.First(c => c.Window
            //             .Equals(Core.Window.CurrentWindow)).BackBufferIndex;
            //m_currentFrameIndex++;
            Interlocked.Exchange(ref m_currentBufferFrame, m_bufferFrames[m_backBufferIndex]);
        }
        
        private void WaitForGpu()
        {
            lock(m_lock)
            {
                m_resource.Wait(m_fence.Resource as VD3D12.ID3D12Fence,
                                (ulong)m_fence.CompletedValue);   
            }
        }
        #endregion

        //internal VD3D12.ID3D12Device D3D12Device
        //{
        //    get { return m_dx12Device; }
        //}
        public void Wait()
        {
            //WaitForGpu();
            WaitForCpu();
        }
    }
}