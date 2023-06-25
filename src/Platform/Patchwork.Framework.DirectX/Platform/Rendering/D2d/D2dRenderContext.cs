#region Usings
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Platform.Rendering.Dxgi.Resources;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Color = System.Drawing.Color;
using VD2D1 = Vortice.Direct2D1;
using VD3D11 = Vortice.Direct3D11;
using VD3D12 = Vortice.Direct3D12;
using VDXGI = Vortice.DXGI;
//using SharpGen.Runtime;
//using Shin.Framework.Extensions;
//using Vortice.Direct3D12;
using VDCommon = Vortice.DCommon;
using VD3D11on12 = Vortice.Direct3D11on12;
using Vortice;
#endregion

namespace Patchwork.Framework.Platform.Rendering.D2d
{
    public sealed class D2dRenderContext : NRenderContext<VD2D1.ID2D1DeviceContext6>, 
                                      INRenderContext2D
    {
        #region Members
        private static readonly object m_lock = new object();
        private readonly int m_bufferCount = 2;
        private readonly VD2D1.ID2D1Device6 m_d2d1Device;

        //private D3D12Context m_d3D12Context;
        private readonly VD2D1.ID2D1Factory7 m_d2d1Factory;
        private readonly VDXGI.IDXGIDevice4 m_dxgiDevice;
        private VD2D1.ID2D1CommandList m_commandList;
        private int m_currentBufferIndex = 0;

        private VDXGI.IDXGIFactory7 m_dxgiFactory;
        private VD2D1.ID2D1Image m_originalTarget;

        private VD2D1.ID2D1Image m_surfaceTarget;
        private ConcurrentDictionary<DxgiSwapChain, Tuple<VD3D11.ID3D11Resource[], VD2D1.ID2D1Bitmap[]>> m_swapChains;
        private INWindow m_window;
        #endregion

        public D2dRenderContext(INRenderDevice device, VDXGI.IDXGIDevice dxgiDevice, VD2D1.ID2D1Device d2d1Device) :
            base(device)
        {
            m_dxgiDevice = dxgiDevice.QueryInterface<VDXGI.IDXGIDevice4>();
            m_d2d1Device = d2d1Device.QueryInterface<VD2D1.ID2D1Device6>();
        }

        #region Methods
        public void Bind(DxgiSwapChain swapChain, bool force = false)
        {
            lock(m_lock)
            {
                Throw.If(!m_isInitialized || !m_isCreated).InvalidOperationException();

                if (m_swapChains.TryGetValue(swapChain, out var buffers))
                    return;

                using var dx11Device = m_dxgiDevice.QueryInterface<VD3D11on12.ID3D11On12Device>();

                var tmpResources = new ConcurrentList<VD3D11.ID3D11Resource>();
                var tmpBuffers = new ConcurrentList<VD2D1.ID2D1Bitmap>();
                for (var i = 0; i < m_bufferCount; i++)
                {
                    using var buffer = swapChain.Resource.GetBuffer<VD3D12.ID3D12Resource>(i);
                    using var dx11Texture = dx11Device.CreateWrappedResource<VD3D11.ID3D11Texture2D1>(buffer,
                                                                                                      new VD3D11on12.ResourceFlags
                                                                                                      {
                                                                                                          BindFlags = VD3D11.BindFlags.RenderTarget
                                                                                                      },
                                                                                                      VD3D12.ResourceStates.RenderTarget,
                                                                                                      VD3D12.ResourceStates.Present);
                    //tmpBuffers.Add(dx11Texture);
                    tmpResources.Add(dx11Texture.QueryInterface<VD3D11.ID3D11Resource>());
                    tmpBuffers.Add(m_resource.CreateSharedBitmap(dx11Texture
                                                                    .QueryInterface<VDXGI.IDXGISurface>(),
                                                                 new VD2D1.BitmapProperties(new VDCommon.PixelFormat(VDXGI.Format.B8G8R8A8_UNorm,
                                                                                                                     VDCommon.AlphaMode.Ignore),
                                                                                            96f,
                                                                                            96f)));
                }

                m_swapChains.TryAdd(swapChain, 
                                    new Tuple<VD3D11.ID3D11Resource[], VD2D1.ID2D1Bitmap[]>(tmpResources.ToArray(), tmpBuffers.ToArray()));
                //wapChain.Resource.GetBuffer<VD3D11.ID3D11Texture2D>(0);
                //dx11Device.createw

                //var bitProp = new VD2D1.BitmapProperties1(new VDCommon.PixelFormat(VDXGI.Format.B8G8R8A8_UNorm, 
                //                                                                   VDCommon.AlphaMode.Ignore),
                //                                          96f,
                //                                          96f,
                //                                          VD2D1.BitmapOptions.Target | VD2D1.BitmapOptions.CannotDraw);


                //var context = m_d2d1Device.CreateDeviceContext(VD2D1.DeviceContextOptions.EnableMultithreadedOptimizations);
                //using var context = VD2D1.D2D1.D2D1CreateDeviceContext(m_dxgiDevice, new VD2D1.CreationProperties()
                //{
                //    DebugLevel = VD2D1.DebugLevel.Information,
                //    Options = VD2D1.DeviceContextOptions.EnableMultithreadedOptimizations,
                //    ThreadingMode = VD2D1.ThreadingMode.MultiThreaded
                //});


                //m_resource = context.QueryInterface<VD2D1.ID2D1DeviceContext6>();

                //Throw.If(!context.IsDxgiFormatSupported(VDXGI.Format.B8G8R8A8_UNorm)).InvalidOperationException();
                //m_originalTarget = context.Target;

                //var sbmp = m_resource.CreateSharedBitmap(dx11Texture, bitProp);

                m_currentBufferIndex = swapChain.BackBufferIndex;
                //m_resource.Target = m_swapChains[0].Item2[m_currentBufferIndex];

                m_window = swapChain.Window;
            }
        }

        /// <inheritdoc />
        /// <inheritdoc />
        protected override bool PlatformBind(INWindow window, bool force = false)
        {
            lock(m_lock)
            {
                //Throw.If(!m_isInitialized /*|| !m_isCreated*/)
                //     .InvalidOperationException();

                //Throw.If(!m_swapChains.TryGetValue(window, out var s))
                //     .InvalidOperationException();
                //    Throw.If(!InitializeWindow(window, out var buffer)).InvalidOperationException();
                m_window = window;
                //m_window.
                //m_resource.SetTarget(s.Item2);

                //D2D1.D2D1CreateDevice(device,);

                return true;
            }
        }

        /// <inheritdoc />
        protected override bool PlatformUnbind(INWindow window, bool force = false)
        {
            lock(m_lock)
            {
                //if (!m_swapChains.ContainsKey(window))
                //    return;
                //m_swapChains.Remove(window, out var s);
                //if (m_resource.Target == s?.Item2)
                m_resource.Target = m_originalTarget;

                //s?.Item2.Dispose();
                //s?.Item1.Dispose();
                return true;
            }
        }

        public void Unbind(DxgiSwapChain swapChain, bool force = false) { }

        protected override void PlatformResize(INWindow window, Size size)
        {
            //var t = m_swapChains[window];
            //var r = t.Item1.ResizeTarget(new ModeDescription(size.Width, size.Height));
            //var r = t.Item1.ResizeBuffers(0, 
            //                          size.Width, 
            //                          size.Height, 
            //                          Format.B8G8R8A8_UNorm, 
            //                          SwapChainFlags.None);
            //var surface = t.Item1.GetBuffer<IDXGISurface1>(0);
            //var bitProp = new BitmapProperties1(new VDCommon.PixelFormat(Format.B8G8R8A8_UNorm, VDCommon.AlphaMode.Ignore),
            //                                          96f,
            //                                          96f,
            //                                          BitmapOptions.Target | BitmapOptions.CannotDraw);
            //var buffer = m_resource.CreateBitmapFromDxgiSurface(surface, bitProp);

            //m_swapChains[window] = new Tuple<IDXGISwapChain1, ID2D1Bitmap>(t.Item1, buffer);
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            //var surface = m_dxgiDevice.CreateSurface(new VDXGI.SurfaceDescription()
            //                                         {
            //                                             Format = VDXGI.Format.B8G8R8A8_UNorm
            //                                         },
            //                                         1,
            //                                         VDXGI.Usage.Backbuffer | VDXGI.Usage.RenderTargetOutput);

            using var context = m_d2d1Device.CreateDeviceContext();
            //using var context = VD2D1.D2D1.D2D1CreateDeviceContext(surface, new VD2D1.CreationProperties()
            //                                                                {
            //                                                                    DebugLevel = VD2D1.DebugLevel.Information,
            //                                                                    ThreadingMode = VD2D1.ThreadingMode.SingleThreaded
            //                                                                });
            ////m_d2d1Device.CreateDeviceContext(VD2D1.DeviceContextOptions.EnableMultithreadedOptimizations);
            m_resource = context.QueryInterface<VD2D1.ID2D1DeviceContext6>();
            Throw.If(!m_resource.IsDxgiFormatSupported(VDXGI.Format.B8G8R8A8_UNorm)).InvalidOperationException();
            m_originalTarget = m_resource.Target;

            return true;
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            foreach (var s in m_swapChains?.Values)
            {
                //if (m_resource.Target == s)
                //    m_resource.Target = m_originalTarget;

                //s?.Item1.Dispose();
            }

            m_commandList?.Dispose();
            //m_dxgiFactory?.Dispose();

            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_swapChains = new ConcurrentDictionary<DxgiSwapChain, Tuple<VD3D11.ID3D11Resource[], VD2D1.ID2D1Bitmap[]>>();
        }

        /// <inheritdoc />
        protected override bool PlatformBegin()
        {
            m_commandList = m_resource.CreateCommandList();
            m_resource.Target = m_commandList;
            m_resource.BeginDraw();
            m_resource.Transform = Matrix3x2.Identity;

            return true;
        }

        /// <inheritdoc />
        protected override void PlatformClear(Color color)
        {
            m_resource.Clear(color.ToColor4());
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
        protected override bool PlatformEnd()
        {
            m_resource.EndDraw();
            m_commandList.Close();
            
            var buffers = m_swapChains.First().Value;
            using var dx11Device = m_dxgiDevice.QueryInterface<VD3D11on12.ID3D11On12Device>();
            dx11Device.AcquireWrappedResources(new[] { buffers.Item1[m_currentBufferIndex] });
            m_resource.Target = m_swapChains.First().Value.Item2[m_currentBufferIndex] ?? m_originalTarget;
            m_resource.BeginDraw();
            m_resource.DrawImage(m_commandList);
            m_resource.EndDraw();
            dx11Device.ReleaseWrappedResources(new[] {buffers.Item1[m_currentBufferIndex]});

            //dx11Device.QueryInterface<VD3D11.ID3D11Device>().ImmediateContext.Flush();
            //swap?.Item1.Present(0, PresentFlags.None);

            //foreach (var (w, (c, b)) in m_swapChains)
            //{
            //    c.Present(0, PresentFlags.AllowTearing);
            //}

            return true;
        }

        /// <inheritdoc />
        protected override void PlatformFlush()
        {
            m_resource.Flush(out var tag1, out var tag2);
        }

        /// <inheritdoc />
        private bool InitializeWindow(INWindow window, out VD2D1.ID2D1Bitmap buffer)
        {
            //var dev = factory.CreateDevice(new IDXGIDevice4(IntPtr.Zero));
            //var con = dev.CreateDeviceContext(DeviceContextOptions.EnableMultithreadedOptimizations);

            //VD3D12.D3D12.D3D12CreateDevice(null, FeatureLevel.Level_1_0_CORE, out var device);

            //try
            {
                buffer = null;
                //if (!Core.Dispatcher.CurrentThreadIsLoopThread)
                //var t = Core.Dispatcher.InvokeAsync<ID2D1Bitmap>(() =>
                // {
                //if (m_swapChains.ContainsKey(window))
                //    buffer = m_swapChains[window];

                //var surface = chain.GetBuffer<IDXGISurface2>(0);
                //var bitProp = new D2D1_BITMAP_PROPERTIES();
                //bitProp.dpiX = bitProp.dpiY = 96f;
                //bitProp.pixelFormat = new D2D1_PIXEL_FORMAT();
                //var bitProp = new BitmapProperties1(new VDCommon.PixelFormat(Format.B8G8R8A8_UNorm, VDCommon.AlphaMode.Ignore),
                //                                          96f,
                //                                          96f,
                //                                          BitmapOptions.Target | BitmapOptions.CannotDraw);
                //buffer = m_resource.CreateBitmapFromDxgiSurface(surface, bitProp);
                //var t = new Tuple<IDXGISwapChain1, ID2D1Bitmap>(chain, buffer);
                //if (m_swapChains.TryAdd(window, t))
                //{
                //    f.MakeWindowAssociation(window.Handle.Pointer, WindowAssociationFlags.Valid);
                //}


                //buffer.Dispose();
                //chain.Dispose();

                //a.Dispose();
                //f.Dispose();
                return true;

                //t.Wait();
                //buffer = t.Result;
                //if (buffer is not null)
                //{
                //buffer = new ID2D1Bitmap(IntPtr.Zero);
                //return false;
                //}
            }
            //finally
            //{

            //}
        }
        #endregion

        //public D2D1Context(VDXGI.IDXGIDevice dxgiDevice, VD2D1.ID2D1Device d2d1Device)
        //{
        //    m_d2d1Device = d2d1Device;
        //    //m_dxgiFactory = dxgiFactory.QueryInterface<VDXGI.IDXGIFactory7>();
        //    //m_d2dFactory = d2d1Factory;
        //    m_dxgiDevice = dxgiDevice;
        //}
    }
}