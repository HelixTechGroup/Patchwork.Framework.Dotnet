using Patchwork.Framework.Platform.Rendering.Resources;
using SharpGen.Runtime;
using Shin.Framework;
using Shin.Framework.IoC.DependencyInjection;
using VDXGI = Vortice.DXGI;
//using Vortice;

namespace Patchwork.Framework.Platform.Rendering.Dxgi.Resources
{
    public sealed class DxgiResourceFactory : NRenderResourceFactory
    {
        /// <inheritdoc />
        public DxgiResourceFactory(IDIChildContainer iocContainer, INRenderDevice renderDevice) : base(iocContainer, renderDevice) { }
        //public DXGIResourceFactory(IDIContainer iocContainer) : base(iocContainer) { }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            var res = VDXGI.DXGI.CreateDXGIFactory2(true, out VDXGI.IDXGIFactory2 factory);
            Throw.If(res != Result.Ok).InvalidOperationException();
            var f = factory?.QueryInterface<VDXGI.IDXGIFactory>();
            m_iocContainer.Register(f);
            factory?.Release();
        }

        ///// <inheritdoc />
        //public override T Create<T>(params object[] parameters)
        //{
        //    var factory = m_iocContainer.Resolve<IDXGIFactory7>();
        //    var tmp = parameters.ToList();
        //    tmp.Insert(0, factory);
        //    return base.Create<T>(tmp.ToArray());
        //}

        /// <inheritdoc />
        protected override void RegisterFactoryTypes()
        {
            m_supportedTypes.Add(typeof(DxgiSwapChain));
        }
    }
}
