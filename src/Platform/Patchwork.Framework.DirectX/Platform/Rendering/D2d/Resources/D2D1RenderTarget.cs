#region Usings
using Patchwork.Framework.Platform.Windowing;
using VD2D1 = Vortice.Direct2D1;
#endregion

namespace Patchwork.Framework.Platform.Rendering.D2d.Resources
{
    internal class D2D1RenderTarget : D2D1Resource<VD2D1.ID2D1RenderTarget>
    {
        #region Members
        protected INWindow m_window;
        #endregion

        /// <inheritdoc />
        public D2D1RenderTarget(INRenderDevice device, VD2D1.ID2D1Factory factory) : base(device, factory) { }

        protected D2D1RenderTarget(INRenderDevice device, VD2D1.ID2D1Factory factory, INWindow window) : this(device, factory)
        {
            m_window = window;
        }

        protected D2D1RenderTarget(INRenderDevice device, INWindow window) : this(device, null, window) { }

        #region Methods
        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            if (!m_window.IsInitialized)
                return false;

            return true;
            //var rtProp = new RenderTargetProperties();
            //var hrtProp = new HwndRenderTargetProperties
            //              {
            //                  Hwnd = m_window.Handle.Pointer,
            //                  PixelSize = m_window.ClientSize,
            //                  PresentOptions = PresentOptions.RetainContents
            //              };
            //m_resource = m_d2D1Factory2.CreateDxgiSurfaceRenderTarget()
            //m_resource.SetDpi(96f, 96f);
            //m_handle = new NHandle(m_resource.NativePointer);
        }

        /// <inheritdoc />
        //protected override object PlatformClone()
        //{
        //    return new D2D1RenderTarget(m_device, m_d2D1Factory, m_window);
        //}
        #endregion
    }
}