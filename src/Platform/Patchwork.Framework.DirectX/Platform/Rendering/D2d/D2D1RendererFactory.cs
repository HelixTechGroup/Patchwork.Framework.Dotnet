using Shin.Framework.IoC.DependencyInjection;

namespace Patchwork.Framework.Platform.Rendering.D2d
{
    public sealed class D2D1RendererFactory : NRenderableFactory
    {
        /// <inheritdoc />
        internal D2D1RendererFactory(IDIChildContainer iocContainer, INRenderDevice device) : base(iocContainer, device) { }

        /// <inheritdoc />
        protected override void CreateFactoryType(ref INRender instance)
        {
            
        }

        /// <inheritdoc />
        protected override void DestroyFactoryType(ref INRender instance)
        {
            
        }

        /// <inheritdoc />
        protected override void RegisterFactoryTypes()
        {
            //m_iocContainer.Register<INWindowRenderer, WinWindowRenderer>(false);
            //m_supportedTypes.AddRange(new[] 
            //                          {
            //                                    typeof(INWindowRenderer),
            //                                    typeof(INOperatingSystemRenderer)
            //                          });
        }
    }
}