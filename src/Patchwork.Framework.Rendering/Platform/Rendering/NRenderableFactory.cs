using Shin.Framework.IoC.DependencyInjection;

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderableFactory : NFactory<INRender>, INRenderFactory
    {
        protected readonly INRenderDevice m_device;

        public INRenderDevice Device
        {
            get { return m_device; }
        }

        /// <inheritdoc />
        protected NRenderableFactory(IDIChildContainer iocContainer, INRenderDevice device) : base(iocContainer)
        {
            m_device = device;
        }
    }
}