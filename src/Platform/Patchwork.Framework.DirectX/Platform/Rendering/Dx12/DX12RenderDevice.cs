#region Header
// solution:Patchwork.Framework.Dotnet
// project:	Net6.Windows
// file:	D:\Users\Bryan\Documents\Projects\HelixTechGroup\Design\Patchwork\dotnet\src\Platform\Patchwork.Framework.DirectX\Platform\Rendering\Dx12\DX12RenderDevice.cs
// summary:	Implements the dx 12 render device class
//			Copyright (c) 2023 HelixDesign, llc. All rights reserved.
#endregion

#region Usings
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Patchwork.Framework.Platform.Rendering.Dx12.Resources;
using Patchwork.Framework.Platform.Rendering.Dx12.Resources.Runtime;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.IoC.DependencyInjection;
using VD3D12Debug = Vortice.Direct3D12.Debug;
using VDXGI = Vortice.DXGI;
using VD3D12 = Vortice.Direct3D12;
using Vortice;
#endregion

namespace Patchwork.Framework.Platform.Rendering.Dx12
{
    /// <summary>A dx 12 render device. This class cannot be inherited.</summary>
    /// <seealso cref="Patchwork.Framework.Platform.Rendering.NRenderDevice"/>
    /// <seealso cref="Patchwork.Framework.Platform.Rendering.Dx12.IDX12RenderDevice"/>
    internal sealed class DX12RenderDevice : NRenderDevice,
                                             IDX12RenderDevice
    {
        #region Members
        private readonly INRenderDevice.INParentRenderDevice m_parent;  ///< The parent

        //private ConcurrentList<INRenderCommandList> m_commandLists;
        private ConcurrentDictionary<NRenderCommandType, INRenderCommandQueue> m_commandQueues; ///< The command queues
        private VD3D12.ID3D12Device m_d3d12Device;  ///< The 016`12 device
        private Dx12DescriptorHeapAllocator m_rtvHeapAllocator; ///< The rtv heap allocator
        #endregion

        #region Properties
        /// <inheritdoc/>
        public IEnumerable<INRenderCommandList> CommandLists
        {
            get
            {
                var result = new ConcurrentList<INRenderCommandList>();
                var lists = m_commandQueues.Values.Select(q => q.CommandLists);
                foreach (var l in lists)
                    result.AddRange(l);

                return result;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<INRenderCommandQueue> CommandQueues
        {
            get { return m_commandQueues.Values; }
        }

        /// <inheritdoc/>
        public INRenderDevice.INParentRenderDevice Parent
        {
            get { return m_parent; }
        }

        /// <summary>Gets the rtv heap allocator.</summary>
        /// <value>The rtv heap allocator.</value>
        public Dx12DescriptorHeapAllocator RtvHeapAllocator
        {
            get { return m_rtvHeapAllocator; }
        }

        /// <summary>Gets the 016`12 device.</summary>
        /// <value>The d 016`12 device.</value>
        internal VD3D12.ID3D12Device D3D12Device
        {
            get { return m_d3d12Device; }
        }
        #endregion

        //internal DX12Device(IDIChildContainer iocContainer, VD3D12.ID3D12Device d3d12Device) : base(iocContainer)
        //{
        //    m_d3d12Device = d3d12Device;
        //}

        /// <inheritdoc/>
        [InjectConstructor]
        internal DX12RenderDevice(IDIChildContainer iocContainer, INRenderDevice.INParentRenderDevice parentDevice) : base(iocContainer)
        {
            m_parent = parentDevice;
            m_iocContainer.Register(parentDevice.Configuration);
            m_commandQueues = new ConcurrentDictionary<NRenderCommandType, INRenderCommandQueue>();

            using var debug = VD3D12.D3D12.D3D12GetDebugInterface<VD3D12Debug.ID3D12Debug6>();
            debug.SetEnableAutoName(true);
            debug.EnableDebugLayer();


            //m_iocContainer.Resolve<IPlatformWindowManager>().WindowCreated += OnWindowCreated;
        }

        #region Methods
        /// <inheritdoc/>
        public INRenderCommandList CreateCommandList(NRenderCommandType type)
        {
            return m_commandQueues[type].CreateCommandList();
        }

        /// <inheritdoc/>
        public INRenderCommandQueue CreateCommandQueue(NRenderCommandType type)
        {
            lock(m_lock)
            {
                if (!m_commandQueues.TryGetValue(type, out var queue))
                {
                    queue = m_iocContainer.Resolve<Dx12CommandQueue>(this, type);
                    queue.Create();
                    //queue.Initialize();
                    m_commandQueues.TryAdd(type, queue);
                }


                return queue;
            }
        }

        //public void Release()
        //{
        //    m_commandQueue.Resource.Release();
        //    m_d3d12Device.Release();
        //}

        /// <summary>Implicit cast that converts the given DX12RenderDevice to an ID3D12Device.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The result of the operation.</returns>
        public static implicit operator VD3D12.ID3D12Device(DX12RenderDevice obj)
        {
            return obj.m_d3d12Device;
        }

        //private INRenderCommandQueue m_commandQueue1;

        //public static implicit operator D3D12Device(VD3D12.ID3D12Device obj)
        //{

        //}
        /// <param name="force"></param>
        /// <inheritdoc/>
        protected override bool CreateResources(bool force)
        {
            base.CreateResources(force);

            PlatformCreateDevice(force);
            //CreateCommandQueue(NRenderCommandType.Direct);
            //var context = m_iocContainer.Resolve<DX12RenderContext>();
            //context.Create();
            //context.Initialize();

            return m_d3d12Device is not null;
        }

        /// <inheritdoc/>
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_commandQueues = new ConcurrentDictionary<NRenderCommandType, INRenderCommandQueue>();
            //var context = m_iocContainer.Resolve<DX12RenderContext>();
            //context.Initialize();
            //PlatformCreateDevice();
        }

        //protected void OnWindowCreated(object sender, INWindow window)
        //{
        //    CreateD3d12Device();
        //}

        /// <summary>Platform create device.</summary>
        /// <param name="force"></param>
        protected override void PlatformCreateDevice(bool force)
        {
            if (m_d3d12Device is not null)
                return;

            using var dxgiFactory = m_iocContainer.Resolve<VDXGI.IDXGIFactory>()
                                                  .QueryInterface<VDXGI.IDXGIFactory4>();
            //VDXGI.DXGI.CreateDXGIFactory2<VDXGI.IDXGIFactory4>(true, out var dxgiFactory);
            //VD3D12.ID3D12Device dxDevice = null;
            for (var i = 0; dxgiFactory.EnumAdapters1(i, out var adapter).Success; i++)
            {
                try
                {
                    if (adapter.Description1.Flags.HasFlag(VDXGI.AdapterFlags.Software) /*||
                            !VD3D12.D3D12.IsSupported(adapter)*/)
                        continue;

                    var r = VD3D12.D3D12.D3D12CreateDevice(adapter, out m_d3d12Device);
                    if (r.Failure) 
                        throw Marshal.GetExceptionForHR(r.Code);
                    //if (VD3D12.D3D12.D3D12CreateDevice(adapter, out m_d3d12Device).Success)
                    //    break;
                }
                //catch (Exception ex)
                //{
                //    Core.Logger.LogException(ex);
                //}
                finally
                {
                    adapter.Release();
                }
            }
            //m_d3d12Device.CreateDescriptorHeap(new VD3D12.DescriptorHeapDescription(VD3D12.DescriptorHeapType.RenderTargetView,
            //                                                                                           Configuration.BufferCount));
                                                                                                       
            Throw.IfNull(m_d3d12Device).InvalidOperationException();
            m_iocContainer.Register(m_d3d12Device);

            m_rtvHeapAllocator = m_iocContainer.Resolve<Dx12DescriptorHeapAllocator>(this, DirectXDescriptorHeapType.RenderTargetView);
            m_rtvHeapAllocator.Create(force);
            //m_rtvHeapAllocator.Initialize();

            CreateCommandQueue(NRenderCommandType.Direct);

            //var context = m_iocContainer.Resolve<D3D12Context>();
            //context.Create();
            //context.Initialize();
        }

        /// <inheritdoc/>
        protected override void PlatformGetDpi(INWindow window)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override void PlatformSetFrameBuffer(NFrameBuffer buffer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <inheritdoc/>
        protected override void RegisterTypes()
        {
            base.RegisterTypes();

            //var dxgiFactory = m_iocContainer.Resolve<VDXGI.IDXGIFactory>().QueryInterface<VDXGI.IDXGIFactory1>();
            //var res = dxgiFactory.EnumAdapters(0, out var adapter);
            //Throw.If(res.Failure).InvalidOperationException();

            //var task = Core.Dispatcher.InvokeAsync(() =>
            //{

            //return dxDevice;

            //});

            //task.Wait();

            //var dxgiDevice = dxDevice?.QueryInterface<IDXGIDevice>();
            //var dx12Device = dxDevice.QueryInterface<VD3D12.ID3D12Device11>();
            //m_iocContainer.Register(dxDevice?.QueryInterface<VD3D12.ID3D12Device>());

            m_iocContainer.Register<Dx12CommandQueue>();
            m_iocContainer.Register<Dx12DescriptorHeapAllocator>();
            m_iocContainer.Register<DX12RenderContext>();

            //dxgiFactory.Release();
            //dxDevice?.Release();
            //dxgiDevice.Release();
        }

        /// <inheritdoc/>
        protected override void RunManager() { }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            var result = m_d3d12Device.DeviceRemovedReason;

            foreach (var q in m_commandQueues.Values)
            {
                q.Dispose();
            }
            
            m_rtvHeapAllocator.Dispose();
            m_d3d12Device.Release();
            m_d3d12Device = null;
            
            base.DisposeManagedResources();
        }
        #endregion

        //internal VD3D12.ID3D12DescriptorHeap D3D12Heap
        //{
        //    get { return m_rtvDescriptorHeap; }
        //}
    }
}