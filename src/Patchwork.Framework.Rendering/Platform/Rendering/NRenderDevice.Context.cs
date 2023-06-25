using System;
using Shin.Framework.Extensions;

using Shin.Framework;
using Shin.Framework.IoC.DependencyInjection;
using Shin.Framework.Threading;

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract partial class NRenderDevice
    {
        public abstract partial class NRenderDeviceHaveContext : NRenderDevice, INRenderDevice.INContextRenderDevice
        {
            [ThreadStatic]
            protected static INRenderContext m_currentContext;

            /// <inheritdoc />
            public static INRenderContext CurrentContext
            {
                get { return m_currentContext; }
            }

            /// <inheritdoc />
            public INRenderContext CreateContext()
            {
                Throw.If(!m_isInitialized || m_isDisposed
                       ^ !m_lockSlim.TryEnter(SynchronizationAccess.Write))
                     .InvalidOperationException();

                try
                {
                    return m_currentContext = PlatformCreateRenderContext();
                }
                finally
                {
                    m_lockSlim.TryExit();
                }
            }

            protected abstract INRenderContext PlatformCreateRenderContext();

            /// <inheritdoc />
            protected NRenderDeviceHaveContext(IDIContainer iocContainer) : base(iocContainer) { }


        }
    }
}