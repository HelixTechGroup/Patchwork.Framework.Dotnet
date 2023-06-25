#region Usings
using System;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Windowing;
using SharpGen.Runtime;
using Shin.Framework;
using Shin.Framework.IoC.DependencyInjection;
using VDXGI = Vortice.DXGI;
//using DN = Vortice.Direct2D1;
using VD3D11 = Vortice.Direct3D11;
using VD3D12 = Vortice.Direct3D12;
using VD3D11on12 = Vortice.Direct3D11on12;

//using VD3D = Vortice.Direct3D;
#endregion

namespace Patchwork.Framework.Platform.Rendering.Dx11
{
    internal sealed class D3D11RenderDevice : NRenderDevice, ID3D11RenderDevice
    {
        #region Members
        private readonly INRenderDevice.INParentRenderDevice m_parent;

        //private VD3D11on12.ID3D11On12Device m_d3D11On12Device;
        private VD3D11.ID3D11Device m_d3D11Device;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INRenderDevice.INParentRenderDevice Parent
        {
            get { return m_parent; }
        }
        #endregion

        /// <inheritdoc />
        public D3D11RenderDevice(IDIChildContainer iocContainer, INRenderDevice.INParentRenderDevice parentDevice) : base(iocContainer)
        {
            m_parent = parentDevice;
        }

        #region Methods
        //protected void OnWindowCreated(object sender, INWindow window)
        //{
        //    CreateD3d11On12Device();
        //}
        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            base.CreateResources(force);

            PlatformCreateDevice(force);
            var context = m_iocContainer.Resolve<D3D11Context>();
            context.Create();

            return m_d3D11Device is not null;
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            var context = m_iocContainer.Resolve<D3D11Context>();
            context.Initialize();
        }

        /// <inheritdoc />
        protected override void OnProcessMessage(IPlatformMessage message)
        {
            switch (message.Id)
            {
                case MessageIds.Window:
                    var wm = message.RawData as IWindowMessageData;
                    switch (wm.MessageId)
                    {
                        case WindowMessageIds.Created:
                            //CreateDevice();
                            break;
                    }

                    break;
            }

            base.OnProcessMessage(message);
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override void PlatformCreateDevice(bool force)
        {
            if (m_d3D11Device is not null)
                return;

            var dx12Device = m_iocContainer.Resolve<VD3D12.ID3D12Device>();
            //var dx12Context = m_iocContainer.Resolve<DX12RenderContext>();
            var commandQueue = dx12Device.CreateCommandQueue(VD3D12.CommandListType.Direct);
            var res = VD3D11on12.Apis.D3D11On12CreateDevice(dx12Device,
                                                            VD3D11.DeviceCreationFlags.BgraSupport,
                                                            DirectXFeatureLevel.v11,
                                                            new[] {(IUnknown)commandQueue},
                                                            0,
                                                            out var m_dx11Device,
                                                            out var context,
                                                            out var flvl);

            Throw.If(res.Failure).InvalidOperationException();

            var dxgiDevice = m_dx11Device?.QueryInterface<VDXGI.IDXGIDevice>();

            m_dx11Device.QueryInterface<VD3D11.ID3D11Multithread>()?
               .SetMultithreadProtected(true);

            m_iocContainer.Register(dxgiDevice);
            
            //m_d3D11Device = dx11Device;
            //dxgiDevice.Release();
        }

        /// <inheritdoc />
        protected override void PlatformGetDpi(INWindow window)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformSetFrameBuffer(NFrameBuffer buffer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <inheritdoc />
        protected override void RegisterTypes()
        {
            base.RegisterTypes();

            m_iocContainer.Register<D3D11Context>();
            //var dxgiFactory = m_iocContainer.Resolve<VDXGI.IDXGIFactory>();
            //var dx12Device = m_iocContainer.Resolve<VD3D12.ID3D12Device>();
            //dxgiFactory.Adapters
            //var res = dxgiFactory.EnumAdapters(0, out var adapter);
            //Throw.If(res.Failure).InvalidOperationException();

            //var res = Vortice.Direct3D11on12.Apis.D3D11On12CreateDevice(dx12Device,
            //                                                  VD3D11.DeviceCreationFlags.None,
            //                                                  DirectXFeatureLevel.v11,
            //                                                  null,
            //                                                  0,
            //                                                  out var dx11Device,
            //                                                  out var context,
            //                                                  out var flvl);
            //res = VD3D11.D3D11.D3D11CreateDevice(adapter,
            //                                     VD3D.DriverType.Unknown,
            //                                     VD3D11.DeviceCreationFlags.BgraSupport | VD3D11.DeviceCreationFlags.Debug,
            //                                     DirectXFeatureLevel.v11,
            //                                     out var dxDevice);
            //Throw.If(res.Failure).InvalidOperationException();
            //var dxgiDevice = dx11Device?.QueryInterface<VDXGI.IDXGIDevice>();
            //m_iocContainer.Register(dxgiDevice);
        }

        /// <inheritdoc />
        protected override void RunManager() { }
        #endregion
    }
}