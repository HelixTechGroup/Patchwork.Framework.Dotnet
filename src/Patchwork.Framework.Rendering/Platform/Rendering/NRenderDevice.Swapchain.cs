#region Usings
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.IoC.DependencyInjection;
using Shin.Framework.Threading;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract partial class NRenderDevice
    {
        #region Nested Types
        public abstract class NRenderDeviceHaveSwapchain : NRenderDevice, INRenderDevice.INSwapchainRenderDevice
        {
            #region Members
            protected ConcurrentList<INSwapchain> m_swapchains;
            private INSwapchain m_currentSwapchain;
            #endregion

            #region Properties
            /// <inheritdoc />
            public INSwapchain CurrentSwapchain
            {
                get { return m_currentSwapchain; }
            }
            #endregion

            /// <inheritdoc />
            protected NRenderDeviceHaveSwapchain(IDIContainer iocContainer) : base(iocContainer) { }

            #region Methods
            /// <inheritdoc />
            public INSwapchain CreateSwapChain(INWindow window)
            {
                Throw.If(!m_isInitialized || m_isDisposed
                       ^ !m_lockSlim.TryEnter(SynchronizationAccess.Write))
                     .InvalidOperationException();

                Throw.If(window.IsInitialized || window.IsRenderable)
                     .InvalidOperationException();

                try
                {
                    var sc = PlatformCreateSwapchain(window);
                    m_swapchains.Add(sc);
                    return sc;
                }
                finally
                {
                    m_lockSlim.TryExit();
                }
            }

            protected abstract INSwapchain PlatformCreateSwapchain(INWindow window);
            #endregion
        }
        #endregion
    }
}