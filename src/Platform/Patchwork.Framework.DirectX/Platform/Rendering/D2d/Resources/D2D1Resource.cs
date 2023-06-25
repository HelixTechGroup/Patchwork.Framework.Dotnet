using Patchwork.Framework.Platform.Rendering.Resources;
using VD2D1 = Vortice.Direct2D1;

//using Vortice.Direct2D1;

//using DN = Vortice.Direct2D1;

namespace Patchwork.Framework.Platform.Rendering.D2d.Resources
{
    internal interface ID2D1Resource
    {
        VD2D1.ID2D1Factory D2D1Factory { get; internal set; }

        VD2D1.ID2D1RenderTarget D2D1RenderTarget { get; internal set; }
    }

    public abstract class D2D1Resource<T> : NRenderResource<T>, ID2D1Resource where T : VD2D1.ID2D1Resource
    {
        protected VD2D1.ID2D1Factory m_d2D1Factory;
        protected VD2D1.ID2D1RenderTarget m_d2D1RenderTarget;

        VD2D1.ID2D1Factory ID2D1Resource.D2D1Factory
        {
            get { return m_d2D1Factory; }
            set { m_d2D1Factory = value; }
        }

        VD2D1.ID2D1RenderTarget ID2D1Resource.D2D1RenderTarget
        {
            get { return m_d2D1RenderTarget; }
            set { m_d2D1RenderTarget = value; }
        }

        protected D2D1Resource(INRenderDevice device) : base(device) { }

        protected D2D1Resource(INRenderDevice device, VD2D1.ID2D1RenderTarget renderTarget)
            : base(device)
        {
            m_d2D1RenderTarget = renderTarget;
        }

        protected D2D1Resource(INRenderDevice device, VD2D1.ID2D1Factory factory) : base(device)
        {
            m_d2D1Factory = factory;
        }

        protected D2D1Resource(INRenderDevice device,
                               VD2D1.ID2D1Factory factory,
                               VD2D1.ID2D1RenderTarget renderTarget) : base(device)
        {
            m_d2D1Factory = factory;
            m_d2D1RenderTarget = renderTarget;
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_handle = new NHandle(m_resource.NativePointer);
        }
    }
}
