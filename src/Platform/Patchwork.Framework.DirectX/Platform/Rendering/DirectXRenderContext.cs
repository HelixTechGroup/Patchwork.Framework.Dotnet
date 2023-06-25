#region Usings
using System;
using System.Drawing;
using System.Linq;
using Patchwork.Framework.Platform.Rendering.D2d;
using Patchwork.Framework.Platform.Rendering.Dx11;
using Patchwork.Framework.Platform.Rendering.Dx12;
using Patchwork.Framework.Platform.Rendering.Dxgi;
using Patchwork.Framework.Platform.Rendering.Dxgi.Resources;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using SharpGen.Runtime;
using VDXGI = Vortice.DXGI;
//using DN = Vortice.Direct2D1;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class DirectXRenderContext : NRenderContext
    {
        #region Members
        //[ThreadStatic]
        //private static INRenderContext m_currentContext;
        protected int m_currentFrameIndex;
        //private readonly D2dRenderContext m_d2d1Context;
        //private readonly D3D11Context m_d3D11Context;

        protected DX12RenderContext m_d3D12Context;

        internal IDXGIRenderDevice m_dxgiDevice;
        //private readonly VDXGI.IDXGIFactory7 m_dxgiFactory;
        protected DxgiSwapChain m_swapChain;
        #endregion

        #region Properties
        //public INRenderContext2D D2D1
        //{
        //    get { return m_d2d1Context; }
        //}

        public INRenderContext3D D3D12
        {
            get { return m_d3D12Context; }
        }
        #endregion

        internal DirectXRenderContext(IDirectXRenderDevice device, /*VDXGI.IDXGIFactory dxgiFactory,*/
                                IDXGIRenderDevice dxgiDevice,
                                //D2dRenderContext d2d1Context,
                                //D3D11Context d3D11Context,
                                DX12RenderContext d3D12Context) : base(device)
        {
            m_dxgiDevice = dxgiDevice;
            //m_d2d1Context = d2d1Context;
            //m_d3D11Context = d3D11Context;
            m_d3D12Context = d3D12Context;
            //m_dxgiFactory = dxgiFactory.QueryInterface<VDXGI.IDXGIFactory7>();
            m_instance = this;
        }

        #region Methods
        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            m_d3D12Context?.Create();

            //m_d2d1Context?.Create();

            return true;
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_d3D12Context?.Initialize();

            //m_d2d1Context?.Initialize();
            //m_swapChains = new ConcurrentList<DXGISwapChain>();
        }

        /// <inheritdoc />
        protected override bool PlatformBegin()
        {
            //foreach (var chain in ))
            {
                //var chain = m_swapChains
                //   .First(c => c.Window
                //                .Equals(Core.Window.CurrentWindow));

                //m_d2d1Context.Resource.Target
                //var buffer = m_d2d1Context
                //                               .Resource
                //                               .CreateBitmapFromDxgiSurface(chain
                //                                                           .Resource

                // .GetBuffer<IDXGISurface>(m_currentFrameIndex));
                //m_d2d1Context.Begin();
            }

            if (!m_isBound)
                return false;

            m_d3D12Context?.Begin();
            //m_d2d1Context?.Begin();
            return true;
        }

        /// <inheritdoc />
        protected override void PlatformClear(Color color)
        {
            //m_d2d1Context?.Clear(color);
            m_d3D12Context?.Clear(color);
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
        //protected override bool PlatformClone(out object clone)
        //{
        //    clone = new DirectXRenderContext(m_device as IDirectXRenderDevice, /*m_d2d1Context, m_d3D11Context,*/  m_d3D12Context);
        //    return true;
        //}

        /// <inheritdoc />
        protected override bool PlatformEnd()
        {
            if (!m_isBound) 
                return false;

            //try
            {
                //m_d2d1Context?.End();
                m_d3D12Context?.End();
                //m_d3D12Context?.Flush();
                ////foreach (var chain in m_swapChains)
                //{
                //    //var chain = m_swapChains
                //    //   .First(c => c.Window
                //    //                .Equals(Core.Window.CurrentWindow));
                //    m_swapChain.Present();
                //    m_currentFrameIndex = m_swapChain.BackBufferIndex;
                //}
            }
            //catch (SharpGenException sgEx)
            //{
            //    if (sgEx.ResultCode.ApiCode == "DeviceRemoved")
            //    {
            //        m_device.Create(true);
            //        m_d3D12Context?.Bind(m_swapChain.Window, true);
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return true;
        }

        /// <inheritdoc />
        protected override void PlatformFlush()
        {
            //m_d3D12Context?.Flush();
            try
            {
                m_d3D12Context?.Flush();
                //foreach (var chain in m_swapChains)
                {
                    //var chain = m_swapChains
                    //   .First(c => c.Window
                    //                .Equals(Core.Window.CurrentWindow));
                    m_swapChain.Present();
                    m_currentFrameIndex = m_swapChain.BackBufferIndex;
                }
                
                m_d3D12Context?.Wait();
            }
            catch (SharpGenException sgEx)
            {
                if (sgEx.ResultCode.ApiCode == "DeviceRemoved")
                {
                    m_device.Create(true);
                    m_d3D12Context?.Bind(m_swapChain.Window, true);
                }
                else
                {
                    throw;
                }
            }
        }
        #endregion

        /// <inheritdoc />
        protected override void PlatformResize(INWindow window, Size size)
        {
            m_d3D12Context?.Resize(window, size);
            m_size = size;
        }

        /// <inheritdoc />
        protected override bool PlatformBind(INWindow window, bool force = false)
        {
            m_swapChain = (m_dxgiDevice.CurrentSwapChain ?? m_dxgiDevice.CreateSwapChain(window)) as DxgiSwapChain;

            m_d3D12Context?.Bind(window);

            //m_d2d1Context?.Bind(window);
            m_size = window.ClientSize;
            return true;
        }

        /// <inheritdoc />
        protected override bool PlatformUnbind(INWindow window, bool force = false)
        {
            m_d3D12Context?.Unbind(window);

            //m_d2d1Context?.Unbind(m_swapChain);
            m_swapChain.Dispose();
            return true;
        }
    }
}