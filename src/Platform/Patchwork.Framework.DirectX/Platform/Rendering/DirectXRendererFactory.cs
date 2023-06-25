using System;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.IoC.DependencyInjection;

namespace Patchwork.Framework.Platform.Rendering
{
    public sealed class DirectXRendererFactory : NRenderableFactory
    {
        /// <inheritdoc />
        public DirectXRendererFactory(IDIChildContainer iocContainer, DirectXRenderDevice device) : base(iocContainer, device)
        {
        }

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
            m_iocContainer.Register<WinWindowRenderer>(false);
            m_supportedTypes.AddRange(new[] 
                                      {
                                                typeof(INWindowRenderer),
                                                typeof(INOperatingSystemRenderer)
                                      });
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            //var winM = m_iocContainer.Resolve<IPlatformWindowManager>();
            //winM.WindowCreated += OnWindowCreated;
        }

        protected void OnWindowCreated(object sender, INWindow window)
        {
            Create<INWindowRenderer>(m_device, window);
        }
    }
}