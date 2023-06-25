#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using SharpGen.Runtime;
using Shin.Framework;
using Shin.Framework.Extensions;
using Vortice.Direct3D12;
//using Vortice.Direct2D1;
//using Vortice.DXGI;
using VDXGI = Vortice.DXGI;
#endregion

namespace Patchwork.Framework.Platform.Rendering.Dxgi.Resources
{
    public sealed class DxgiSwapChain : NSwapChain<VDXGI.IDXGISwapChain4>
    {
        #region Members
        private readonly VDXGI.IDXGIFactory m_dxgiFactory;
        private INRenderCommandQueue m_commandQueue;
        private bool m_reset;

        //private IDXGIDevice m_dxgiDevice;
        //private DX12RenderDevice m_d3D12Device;
        #endregion

        #region Properties
        public override int BackBufferIndex
        {
            get { return m_resource.CurrentBackBufferIndex; }
        }
        #endregion

        internal DxgiSwapChain(INWindow window, VDXGI.IDXGIFactory dxgiFactory, INRenderCommandQueue commandQueue) :
            this(commandQueue.Device, window, dxgiFactory)
        {
            m_commandQueue = commandQueue;
        }

        internal DxgiSwapChain(INRenderDevice device, INWindow window, VDXGI.IDXGIFactory dxgiFactory) : this(device, dxgiFactory)
        {
            m_window = window;
        }

        internal DxgiSwapChain(INRenderDevice device, VDXGI.IDXGIFactory dxgiFactory /*, DX12RenderDevice d3d12Device*/)
            : base(device)
        {
            m_dxgiFactory = dxgiFactory;
            //m_d3D12Device = d3d12Device;
            //m_window = window;
        }

        #region Methods
        /// <inheritdoc />
        protected override void PlatformUnbind(INWindow window)
        {
            window = null;
            Dispose();
        }

        /// <inheritdoc />
        protected override void PlatformPresent()
        {
            //try
            //{
                var res = m_resource.Present(0,
                                             VDXGI.PresentFlags.None,
                                             new VDXGI.PresentParameters());
                Throw.If(res.Failure).InvalidOperationException();
            //}
            //catch (SharpGenException sgEx)
            //{
            //}
        }

        protected override void PlatformBind(INWindow window)
        {
            m_window = window;
            Create(true);
        }

        public void Bind(INRenderCommandQueue queue)
        {
            m_commandQueue = queue;
            Create(true);
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            if (m_window is null || m_commandQueue is null)
                return false;

            //if (m_swapChains.ContainsKey(window))
            //    buffer = m_swapChains[window].Item2;
            //var d = m_dxgiFactory.EnumAdapters()[0];

            var swapDesc = new VDXGI.SwapChainDescription1(m_window.ClientArea.Width, 
                                                           m_window.ClientArea.Height)
                           {
                               BufferCount = m_device.Configuration.BufferCount,
                               Scaling = VDXGI.Scaling.None,
                               Format = VDXGI.Format.B8G8R8A8_UNorm,
                               SwapEffect = VDXGI.SwapEffect.FlipDiscard,
                               BufferUsage = VDXGI.Usage.RenderTargetOutput |
                                             VDXGI.Usage.Backbuffer //|
                                             //VDXGI.Usage.Shared
                           };
            //var fullDesc = new VDXGI.SwapChainFullscreenDescription
            //               {
            //                   Scaling = VDXGI.ModeScaling.Centered,
            //                   Windowed = true
            //               };

            // using var a = m_dxgiDevice.GetAdapter();
            //using var f = a.GetParent<IDXGIFactory>();
            using var f = m_dxgiFactory.QueryInterface<VDXGI.IDXGIFactory7>();
            using var tmp = f.CreateSwapChainForHwnd((ID3D12CommandQueue)m_commandQueue.Resource,
                                                                 m_window.Handle.Pointer,
                                                                 swapDesc);

            m_window.SizeChanged += (sender, e) =>
                                    {
                                        var win = sender as INWindow;
                                        if (!win.IsInitialized)
                                            return;

                                        var size = e.RequestedValue;

                                        //m_resource.ResizeTarget(new VDXGI.ModeDescription(size.Width, size.Height));
                                        //m_resource.ResizeBuffers(m_device.Configuration.BufferCount, size.Width, size.Height, VDXGI.Format.B8G8R8A8_UNorm, VDXGI.SwapChainFlags.None);
                                        //Resize(win, e.RequestedValue);
                                        //var b = m_swapChains[win];
                                        //var surface = b.Item1.GetBuffer<IDXGISurface1>(0);
                                        //var cSize = new Size(surface., b.Item1.Description1.Height);
                                        Core.Logger.LogDebug("=== DXGI window changed event ===");
                                        Core.Logger.LogDebug($"---Window Client Area: {m_window.ClientSize}");
                                        //Core.Logger.LogDebug(@$"---D2D1 RenderTarget Size: {m_device..Resource.Size}");
                                        Core.Logger.LogDebug($"---Context Client Area: {m_resource.Description1.Height}");
                                        //Core.Logger.LogDebug($"---Buffer Client Area: {b.Item2.Size}");
                                        Core.Logger.LogDebug($"---Event Client Area: {e.RequestedValue}");
                                    };
            //m_window.Rendered += (sender, args) =>
            //                     {
            //                         Core.Logger.LogDebug($"=== D2D1 window rendered event ===");
            //                         m_resource.Present(0); 

            //                     };

            m_dxgiFactory.MakeWindowAssociation(m_window.Handle.Pointer, VDXGI.WindowAssociationFlags.Valid);
            m_resource = tmp.QueryInterface<VDXGI.IDXGISwapChain4>();
            m_resource.BackgroundColor = Color.Coral.ToColor4();
            return m_resource is not null;
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            //m_window = null;
            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            if (!m_reset)
                m_resource.Dispose();
            
            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        //protected override bool PlatformClone(out object clone)
        //{
        //    clone = new DXGISwapChain(m_device, m_window, m_dxgiFactory, m_commandQueue);
        //    return true;
        //}
        #endregion
    }
}