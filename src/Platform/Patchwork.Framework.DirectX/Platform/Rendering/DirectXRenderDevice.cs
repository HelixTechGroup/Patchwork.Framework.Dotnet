#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Patchwork.Framework.Platform.Rendering.D2d;
using Patchwork.Framework.Platform.Rendering.Dx11;
using Patchwork.Framework.Platform.Rendering.Dx12;
using Patchwork.Framework.Platform.Rendering.Dxgi;
using Patchwork.Framework.Platform.Rendering.Dxgi.Resources;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.IoC.DependencyInjection;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public sealed class DirectXRenderDevice : NRenderDevice,
                                              IDirectXRenderDevice
    {
        #region Properties
        public INRenderContext CurrentContext
        {
            get { return m_iocContainer.Resolve<DirectXRenderContext>(strategy: DIResolutionStrategy.SelfOnly); }
        }

        /// <inheritdoc />
        public INSwapChain CurrentSwapChain
        {
            get { return m_iocContainer.Resolve<DXGIRenderDevice>(strategy: DIResolutionStrategy.SelfOnly).CurrentSwapChain; }
        }

        /// <inheritdoc />
        public INSwapChain CreateSwapChain(INWindow window)
        {
            Throw.If(!m_isInitialized || !m_isCreated).InvalidOperationException();
            var dxgi = m_iocContainer.Resolve<DXGIRenderDevice>();
            return dxgi.CreateSwapChain(window);
        }

        /// <inheritdoc />
        IEnumerable<INRenderDevice.INChildRenderDevice> INRenderDevice.INParentRenderDevice.Children
        {
            get { return m_iocContainer.ResolveAll<INRenderDevice.INChildRenderDevice>(DIResolutionStrategy.SelfOnly); }
        }
        #endregion

        /// <inheritdoc />
        public DirectXRenderDevice(IDIChildContainer iocContainer) : base(iocContainer) { }

        #region Methods
        /// <inheritdoc />
        //public INSwapChain CreateSwapChain(INWindow window)
        //{
        //    Throw.If(!m_isInitialized || !m_isCreated).InvalidOperationException();

        //    var d12 = m_iocContainer.Resolve<DX12RenderDevice>();
        //    var dxgi = m_iocContainer.Resolve<DXGIRenderDevice>();
        //    var chain = dxgi.CreateSwapChain(window) as DXGISwapChain;
        //    chain?.Bind(d12.CommandQueues.Single(q => q.Type == NRenderCommandType.Direct));
        //    return chain;
        //}

        /// <inheritdoc />
        public INRenderContext CreateContext(INWindow window)
        {
            Throw.If(!m_isInitialized || !m_isCreated).InvalidOperationException();

            var context = m_iocContainer.Resolve<DirectXRenderContext>();
            context.Create();
            //context.Initialize();
            context.Bind(window);

            return context;
        }

        //private ConcurrentList<INRenderDevice.INChildRenderDevice> m_children;
        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            base.CreateResources(force);

            //var dxgi = m_iocContainer.Resolve<DXGIRenderDevice>();
            //var d12 = m_iocContainer.Resolve<DX12RenderDevice>();
            //var d11 = m_iocContainer.Resolve<D3D11RenderDevice>();
            //var d2d = m_iocContainer.Resolve<D2dRenderDevice>();
            //dxgi.Create();
            //d12.Create();
            //d11.Create();
            //d2d.Create();

            PlatformCreateDevice(force);

            return true;
            //return (dxgi.IsInitialized 
            //     && d12.IsInitialized 
            //     && d11.IsInitialized
            //        && d2d.IsInitialized);
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            //foreach (var dev in m_iocContainer.ResolveAll<INRenderDevice.INChildRenderDevice>())
            //{
            //    dev?.Initialize();
            //}

            //PlatformCreateDevice();

            //var context = m_iocContainer.Resolve<DirectXRenderContext>();
            //context.Create();
            //context.Initialize();

            var dxgi = m_iocContainer.Resolve<DXGIRenderDevice>();
            var d12 = m_iocContainer.Resolve<DX12RenderDevice>();
            //var d11 = m_iocContainer.Resolve<D3D11RenderDevice>();
            //var d2d = m_iocContainer.Resolve<D2dRenderDevice>();
            dxgi.Initialize();
            d12.Initialize();
            //d11.Initialize();
            //d2d.Initialize();
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override void PlatformCreateDevice(bool force)
        {
            base.PlatformCreateDevice(force);
            //var d = m_iocContainer.ResolveAll<INRenderDevice.INChildRenderDevice>();
            //foreach (var dev in d)
            //{
            //    dev?.Create();
            //}

            //var context = m_iocContainer.Resolve<INRenderContext>();

            //m_iocContainer.Resolve<DXGIDevice>()?.Initialize();
            //m_iocContainer.Resolve<D3D12Device>()?.Initialize();
            //m_iocContainer.Resolve<D3D11Device>()?.Initialize();
            //m_iocContainer.Resolve<D2D1Device>()?.Initialize();

            var dxgi = m_iocContainer.Resolve<DXGIRenderDevice>();
            var d12 = m_iocContainer.Resolve<DX12RenderDevice>();
            //var d11 = m_iocContainer.Resolve<D3D11RenderDevice>();
            //var d2d = m_iocContainer.Resolve<D2dRenderDevice>();
            d12.Create(force);
            dxgi.Create(force);
            //d11.Create();
            //d2d.Create();
            //dxgi?.Initialize();

            //d12?.Initialize();
            //d12?.Context?.Create();
            //d12?.Context?.Initialize();

            //d11?.Initialize();
            //d11?.Context?.Create();
            //d11?.Context?.Initialize();

            //d2d?.Initialize();
            //d2d?.Context?.Create();
            //d2d?.Context?.Initialize();
        }

        /// <inheritdoc />
        protected override void PlatformGetDpi(INWindow window)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformSetFrameBuffer(NFrameBuffer buffer) { }

        /// <inheritdoc />
        protected override void RegisterTypes()
        {
            base.RegisterTypes();

            //var res = DXGI.CreateDXGIFactory2(true, out IDXGIFactory2 factory);
            //Throw.If(res != Result.Ok).InvalidOperationException();
            //m_iocContainer.Register<IDXGIFactory>(factory);

            //m_iocContainer.Register<D2dRenderDevice>();
            //m_iocContainer.Register<D3D11RenderDevice>();
            m_iocContainer.Register<DX12RenderDevice>();
            m_iocContainer.Register<DXGIRenderDevice>();

            m_iocContainer.Register<DirectXDeviceConfiguration>();
            m_iocContainer.Register<DirectXRendererFactory>();
            m_iocContainer.Register<DirectXRenderContext>();

            //m_iocContainer.Resolve<DXGIDevice>()?.Initialize();
            //m_iocContainer.Resolve<D3D12Device>()?.Initialize();
            //m_iocContainer.Resolve<D3D11Device>()?.Initialize();
            //m_iocContainer.Resolve<D2D1Device>()?.Initialize();

            //m_iocContainer.Resolve<DirectXRendererFactory>()?.Initialize();
            //m_iocContainer.Resolve<DirectXContext>()?.Initialize();
        }

        /// <inheritdoc />
        protected override void RunManager()
        {
            var devs = m_iocContainer.ResolveAll<INRenderDevice.INRenderDevicePump>();
            foreach (var device in devs)
                device.Pump(CancellationToken.None);
        }
        #endregion
    }
}