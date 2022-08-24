using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Rendering.Resources;
using SharpGen.Runtime;
using Shin.Framework;
using Shin.Framework.IoC.DependencyInjection;
using Vortice.Direct2D1;
using D2D1 = Vortice.Direct2D1.D2D1;

namespace Patchwork.Framework.Platform.Rendering
{
    public sealed class D2D1ResourceFactory : NRenderResourceFactory
    {
        private ID2D1Factory2 m_d2D1Factory;

        /// <inheritdoc />
        public D2D1ResourceFactory(IContainer iocContainer) : base(iocContainer) { }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            var res = D2D1.D2D1CreateFactory(FactoryType.MultiThreaded, out m_d2D1Factory);
            Throw.If(res != Result.Ok).InvalidOperationException();
            m_iocContainer.Register(m_d2D1Factory);
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_d2D1Factory.Dispose();

            base.DisposeManagedResources();
        }


        /// <inheritdoc />
        protected override void RegisterResources()
        {
            m_supportedResources.Add(typeof(D2D1RenderTarget));
        }
    }
}
