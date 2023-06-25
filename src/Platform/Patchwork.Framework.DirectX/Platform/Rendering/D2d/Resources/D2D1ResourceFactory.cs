using Patchwork.Framework.Platform.Rendering.Resources;
using Shin.Framework.IoC.DependencyInjection;
using VD2D1 = Vortice.Direct2D1;
//using Vortice.DXGI;

//using DN = Vortice.Direct2D1;

namespace Patchwork.Framework.Platform.Rendering.D2d.Resources
{
    public sealed class D2D1ResourceFactory : NRenderResourceFactory
    {
        /// <inheritdoc />
        //public D2D1ResourceFactory(IDIContainer iocContainer) : base(iocContainer) { }

        /// <inheritdoc />
        public D2D1ResourceFactory(IDIChildContainer iocContainer, INRenderDevice renderDevice) : base(iocContainer, renderDevice) { }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            

            base.InitializeResources();
        }

        /// <inheritdoc />
        //protected override void DisposeUnmanagedResources()
        //{
        //    var factory = m_iocContainer.Resolve<ID2D1Factory2>();
        //    factory.Dispose();
        //    base.DisposeUnmanagedResources();
        //}

        ///// <inheritdoc />
        //public override T Create<T>(params object[] parameters)
        //{
        //    var factory = m_iocContainer.Resolve<ID2D1Factory2>();
        //    var tmp = parameters.ToList();
        //    tmp.Insert(0, factory);
        //    return base.Create<T>(tmp.ToArray());
        //}

        /// <inheritdoc />
        protected override void CreateFactoryType(ref INRenderResource instance)
        {
            var context = m_iocContainer.Resolve<D2dRenderContext>();
            var factory = m_iocContainer.Resolve<VD2D1.ID2D1Factory2>();
            ((ID2D1Resource)instance).D2D1Factory = factory;
            ((ID2D1Resource)instance).D2D1RenderTarget = context.Resource;

            base.CreateFactoryType(ref instance);
        }

        /// <inheritdoc />
        protected override void RegisterFactoryTypes()
        {
            m_supportedTypes.Add(typeof(D2D1HwndRenderTarget));
        }
    }
}
