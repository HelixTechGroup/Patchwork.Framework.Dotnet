#region Usings
using System;
using System.Linq;
using Patchwork.Framework.Platform.Rendering.Dx12;
using Patchwork.Framework.Platform.Rendering.Dxgi.Resources;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.IoC.DependencyInjection;
using Vortice.Direct3D12;
using VDXGI = Vortice.DXGI;

//using DN = Vortice.Direct2D1;
//using VD3D11 = Vortice.Direct3D11;
//using VD3D12 = Vortice.Direct3D12;
//using VD3D = Vortice.Direct3D;
#endregion

namespace Patchwork.Framework.Platform.Rendering.Dxgi
{
    internal sealed class DXGIRenderDevice : NRenderDevice, IDXGIRenderDevice
    {
        #region Members
        //[ThreadStatic]
        private DxgiSwapChain m_swapChain;
        private readonly INRenderDevice.INParentRenderDevice m_parent;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INRenderDevice.INParentRenderDevice Parent
        {
            get { return m_parent; }
        }

        /// <inheritdoc />
        public INSwapChain CurrentSwapChain
        {
            get { return m_swapChain; }
        }
        #endregion

        /// <inheritdoc />
        internal DXGIRenderDevice(IDIChildContainer iocContainer, INRenderDevice.INParentRenderDevice parentDevice) : base(iocContainer)
        {
            m_parent = parentDevice;
        }

        #region Methods
        /// <inheritdoc />
        public INSwapChain CreateSwapChain(INWindow window)
        {
            lock(m_lock)
            {
                if (m_swapChain is not null && m_swapChain.Window.Equals(window))
                    return m_swapChain;

                    //if (m_swapChains.Count(c => c.IsDisposed) > 0)
                //{
                //    chain = m_swapChains.First(c => c.IsDisposed);
                //    chain.Bind(window);
                //}
                //else
                {

                    var f = m_iocContainer.Resolve<VDXGI.IDXGIFactory>();
                    var cq = m_iocContainer.Resolve<DX12RenderDevice>();

                    var rf = m_iocContainer
                       .Resolve<DxgiResourceFactory>(strategy: DIResolutionStrategy.SelfOnly);
                    m_swapChain = rf.Create<DxgiSwapChain>(this, window, f, cq.CommandQueues.Single(q => q.Type == NRenderCommandType.Direct));
                    //chain.Initialize();
                    //chain.Bind(window);
                    //m_swapChains.Add(chain);
                }

                return m_swapChain;
            }
        }

        public void Release()
        {
            m_swapChain.Resource.Release();
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override void PlatformCreateDevice(bool force) { }

        /// <inheritdoc />
        protected override void PlatformGetDpi(INWindow window) { }

        /// <inheritdoc />
        protected override void PlatformSetFrameBuffer(NFrameBuffer buffer) { }

        /// <inheritdoc />
        /// <inheritdoc />
        protected override void RegisterTypes()
        {
            base.RegisterTypes();

            m_iocContainer.Register<DxgiResourceFactory>();
        }

        /// <inheritdoc />
        protected override void RunManager()
        {
            //m_swapChain.Present();
        }

        #endregion

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            return base.CreateResources(force);
        //    //Initialize();
        //    return true;
        }
    }
}