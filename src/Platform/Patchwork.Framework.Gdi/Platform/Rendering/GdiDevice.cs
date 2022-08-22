#region Usings
using System;
using System.ComponentModel;
using System.Drawing;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.Threading;
using SkiaSharp;
using IContainer = Shin.Framework.IoC.DependencyInjection.IContainer;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Helpers;
using static Patchwork.Framework.Platform.Interop.Utilities;
using Patchwork.Framework.Platform.Interop.Gdi32;
using Patchwork.Framework.Platform.Interop.GdiPlus;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public sealed class GdiDevice : NRenderDevice<GdiAdapter, GdiContext>, INFrameBufferDevice
    {
        protected ConcurrentList<NFrameBuffer> m_buffers;
        protected IntPtr m_memHdc;
        protected IntPtr m_hdc;

        #region Methods
        /// <inheritdoc />
        protected override void RegisterRenderers()
        {
            m_iocContainer.Register<GdiHdcManager>();
            m_iocContainer.Register<INRenderDevice>(this);
            m_iocContainer.Register<INRenderAdapter, GdiAdapter>();
            m_iocContainer.Register<INResourceFactory, GdiResourceFactory>();
            m_iocContainer.Register<INWindowRenderer, GdiWindowRenderer>(false);
            m_supportedRenderers.AddRange(new[] { typeof(INWindowRenderer), typeof(INFrameBufferRenderer)});
            //Core.IoCContainer.Register<INRenderDevice>(this);
        }

        /// <inheritdoc />
        protected override void PlatformSetFrameBuffer(NFrameBuffer buffer)
        {
            //if (m_buffers.Count == 0)
            m_buffers.Add(buffer);

            
            //foreach (var renderer in m_iocContainer.ResolveAll<INFrameBufferRenderer>())
            //{
                //renderer.
            //}
        }

        //protected override TRenderer PlatformCreateRenderer<TRenderer>(params object[] parameters)
        //{
        //    var renderer = m_iocContainer.Resolve<TRenderer>();

        //    return renderer;
        //}
        #endregion

        /// <inheritdoc />
        public GdiDevice(IContainer iocContainer) : base(iocContainer) { }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_buffers = new ConcurrentList<NFrameBuffer>();
            m_dpiScale = new PointF(1f,1f);
            m_context = m_iocContainer.Resolve<GdiContext>();
            m_adapter = m_iocContainer.Resolve<GdiAdapter>();
            m_context.Initialize();
            m_adapter.Initialize();

            Core.Window.WindowCreated += OnWindowCreated;
        }

        protected void OnWindowCreated(object sender, INWindow e)
        {
            PlatformGetDpi(e);
            e.Resize(new Size((int)(e.Size.Width * m_dpiScale.X), 
                              (int)(e.Size.Height * m_dpiScale.Y)));
            e.Move(new Point((int)(e.Position.X * m_dpiScale.X),
                             (int)(e.Position.Y * m_dpiScale.Y)));
            //MoveWindow(e.Handle.Pointer,)
        }

        protected override void PlatformGetDpi(INWindow window)
        {
            var hdc = GetDC(window.Handle.Pointer);
            var dpiX = GetDeviceCaps(hdc, DeviceCap.LOGPIXELSX);
            var dpiY = GetDeviceCaps(hdc, DeviceCap.LOGPIXELSY);

            m_dpiScale.X = dpiX/96f;
            m_dpiScale.Y = dpiY / 96f;

            ReleaseDC(window.Handle.Pointer, hdc);
        }
    }
}