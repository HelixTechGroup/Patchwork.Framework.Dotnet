using Shin.Framework.IoC.DependencyInjection;

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public abstract class NRenderResourceFactory : NFactory<INRenderResource>, INRenderResourceFactory
    {
        protected INRenderDevice m_device;

        protected NRenderResourceFactory(IDIChildContainer iocContainer, INRenderDevice device) : base(iocContainer)
        {
            m_device = device;
        }

        /// <inheritdoc />
        //protected NRenderResourceFactory(IDIContainer iocContainer) : base(iocContainer)
        //{
        //    m_device = iocContainer.Resolve<INRenderDevice>();
        //}

        /// <inheritdoc />
        protected override void CreateFactoryType(ref INRenderResource instance)
        {
            instance.Create();
        }

        /// <inheritdoc />
        protected override void DestroyFactoryType(ref INRenderResource instance)
        {
            instance.Dispose();
        }
    }
}