using System.Drawing;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using VD2D1 = Vortice.Direct2D1;
//using Vortice.Direct2D1;
//using Vortice.DXGI;
//using Vortice.Mathematics;

namespace Patchwork.Framework.Platform.Rendering.D2d.Resources
{
    public class D2D1HwndRenderTarget : D2D1Resource<VD2D1.ID2D1HwndRenderTarget>
    {
        protected INWindow m_window;

        protected D2D1HwndRenderTarget(INRenderDevice device, VD2D1.ID2D1Factory factory) : base(device, factory) { }

        protected D2D1HwndRenderTarget(INRenderDevice device, VD2D1.ID2D1Factory factory, INWindow window) : this(device, factory)
        {
            m_window = window;
        }

        protected D2D1HwndRenderTarget(INRenderDevice device, INWindow window) : this (device,null, window) { }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            Throw.If(m_window is null).InvalidOperationException();

            if (!m_window.IsInitialized)
                return false;


            var rtProp = new VD2D1.RenderTargetProperties();
            var hrtProp = new VD2D1.HwndRenderTargetProperties()
            {
                Hwnd = m_window.Handle.Pointer,
                PixelSize = new Size(), //m_window.ClientSize.ToSizeI(), 
                PresentOptions = VD2D1.PresentOptions.RetainContents
            };

            m_resource = m_d2D1Factory.CreateHwndRenderTarget(rtProp, 
                                                 hrtProp);
            m_resource.SetDpi(96f, 96f);

            return true;
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            m_resource.Dispose();
            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        //protected override object PlatformClone()
        //{
        //    return new D2D1HwndRenderTarget(m_device, m_d2D1Factory, m_window);
        //}

        internal void SetWindow(INWindow window)
        {
            m_window = window;
        }
    }
}
