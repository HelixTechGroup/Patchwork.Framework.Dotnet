#region Usings
using Patchwork.Framework.Platform.Rendering.D2d.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.IoC.DependencyInjection;
using VDXGI = Vortice.DXGI;
using VD2D1 = Vortice.Direct2D1;
//using VD3D11 = Vortice.Direct3D11;

//using VD3D = Vortice.Direct3D;
#endregion

namespace Patchwork.Framework.Platform.Rendering.D2d
{
    internal sealed class D2dRenderDevice : NRenderDevice, ID2D1RenderDevice
    {
        #region Members
        //private VDXGI.IDXGIFactory7 m_dxgiFactory;
        //private VDXGI.IDXGIDevice m_dxgiDevice;
        private VD2D1.ID2D1Device m_d2D1Device;
        private INRenderDevice.INParentRenderDevice m_parent;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INRenderDevice.INParentRenderDevice Parent
        {
            get { return m_parent; }
        }
        #endregion

        /// <inheritdoc />
        public D2dRenderDevice(IDIChildContainer iocContainer, INRenderDevice.INParentRenderDevice parentDevice) : base(iocContainer)
        {
            m_parent = parentDevice;
        }

        #region Methods
        /// <inheritdoc />
        protected override void PlatformGetDpi(INWindow window) { }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override void PlatformCreateDevice(bool force)
        {
            if (m_d2D1Device is not null)
                return;

            try
            {
                var dxgiDevice = m_iocContainer.Resolve<VDXGI.IDXGIDevice>();
                m_d2D1Device = VD2D1.D2D1.D2D1CreateDevice(dxgiDevice,
                                                                   new VD2D1.CreationProperties()
                                                                   {
                                                                       DebugLevel = VD2D1.DebugLevel.Information,
                                                                       ThreadingMode = VD2D1.ThreadingMode.SingleThreaded
                                                                   });

                m_iocContainer.Register(m_d2D1Device);
            }
            catch
            {

            }
            
            //var d2d1Factory = m_iocContainer.Resolve<VD2D1.ID2D1Factory>();
            //
            //d2d1Factory.cr
            //var d2d1Device = d2d1Factory.CreateDevice(dxgiDevice);
            //var d2d1Device = VD2D1.D2D1.D2D1CreateDevice(dxgiDevice,
            //                                               new VD2D1.CreationProperties()
            //                                               {
            //                                                   ThreadingMode = VD2D1.ThreadingMode.MultiThreaded
            //                                               });
            //Throw.If(d2d1Device is null).InvalidOperationException();

            //d2d1Device?.Release();
        }

        /// <inheritdoc />
        protected override void PlatformSetFrameBuffer(NFrameBuffer buffer) { }

        /// <inheritdoc />
        /// <inheritdoc />
        protected override void RegisterTypes()
        {
            base.RegisterTypes();

            var d2D1Factory = VD2D1.D2D1.D2D1CreateFactory<VD2D1.ID2D1Factory>();
            //Throw.If(res.Failure).InvalidOperationException();
            m_iocContainer.Register(d2D1Factory);

            //res = DXGI.CreateDXGIFactory(out IDXGIFactory dxgiFactory);
            //Throw.If(res.Failure).InvalidOperationException();
            //m_iocContainer.Register<IDXGIFactory>(dxgiFactory);

            //var dxgiFactory = m_iocContainer.Resolve<IDXGIFactory7>();
            //var res = dxgiFactory.EnumAdapters(0, out var adapter);
            //Throw.If(res.Failure).InvalidOperationException();

            //res = VD3D11.D3D11.D3D11CreateDevice(adapter,
            //                                     VD3D.DriverType.Unknown,
            //                                     VD3D11.DeviceCreationFlags.BgraSupport | VD3D11.DeviceCreationFlags.Debug,
            //                                     DxFeatureLevel.v11,
            //                                     out var dxDevice);
            //Throw.If(res.Failure).InvalidOperationException();
            //var dxgiDevice = dxDevice?.QueryInterface<IDXGIDevice>();
            //m_iocContainer.Register<IDXGIDevice>(dxgiDevice);

            //var dxgiDevice = m_iocContainer.Resolve<D3D12Context>();

            //var props = new VD2D1.CreationProperties()
            //{
            //    Options = VD2D1.DeviceContextOptions.EnableMultithreadedOptimizations,
            //    ThreadingMode = VD2D1.ThreadingMode.MultiThreaded
            //};


            m_iocContainer.Register<D2D1RendererFactory>();
            m_iocContainer.Register<D2D1ResourceFactory>();
            m_iocContainer.Register<D2D1Adapter>();
            m_iocContainer.Register<D2dRenderContext>();
        }

        /// <inheritdoc />
        protected override void RunManager() { }
        #endregion

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            base.CreateResources(force);

            PlatformCreateDevice(force);
            //var context = m_iocContainer.Resolve<D2dRenderContext>();
            //context.Create();

            return m_d2D1Device is not null;
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            //var context = m_iocContainer.Resolve<D2dRenderContext>();
            //context.Initialize();
        }
    }
}