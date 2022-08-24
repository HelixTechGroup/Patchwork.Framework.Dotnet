using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.IoC.DependencyInjection;

namespace Patchwork.Framework.Platform.Rendering
{
    public class D2D1Device : NRenderDevice<D2D1Adapter>
    {
        /// <inheritdoc />
        protected override void RegisterRenderers()
        {
            m_iocContainer.Register<INRenderDevice>(this);
            m_iocContainer.Register<INRenderAdapter, D2D1Adapter>();
            m_iocContainer.Register<INResourceFactory, D2D1ResourceFactory>();
            m_iocContainer.Register<INWindowRenderer, D2D1WindowRenderer>(false);
            m_supportedRenderers.Add(typeof(D2D1WindowRenderer));
        }

        /// <inheritdoc />
        protected override void PlatformSetFrameBuffer(NFrameBuffer buffer)
        {
            return;
        }

        /// <inheritdoc />
        protected override void PlatformGetDpi(INWindow window)
        {
            return;
        }

        /// <inheritdoc />
        public D2D1Device(IContainer iocContainer) : base(iocContainer) { }
    }
}
