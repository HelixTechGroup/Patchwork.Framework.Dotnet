#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Patchwork.Framework.Runtime;
using Patchwork.Framework.Threading.Runtime;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.IoC.DependencyInjection;
using Shin.Framework.Threading;
#endregion

namespace Patchwork.Framework.Manager
{
    [RunsOnMainThread]
    public class PlatformRenderManager : PlatformManager<AssemblyRenderingAttribute,
                                             IPlatformMessage<IRenderMessageData>>,
                                         IPlatformRenderingManager
    {
        #region Events
        public event EventHandler<INWindow> WindowCreated;
        //{
        //    add { Core.IoCContainer.Resolve<IPlatformWindowManager>().WindowCreated += value; }
        //    remove { Core.IoCContainer.Resolve<IPlatformWindowManager>().WindowCreated -= value; }
        //}

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowDestroyed;
        #endregion

        #region Members
        //protected IList<INRenderer> m_renderers;
        protected IPlatformWindowManager m_windowManager;
        #endregion

        #region Properties
        /// <inheritdoc />
        public IEnumerable<Type> SupportedRenderers
        {
            get
            {
                var devs = Core.IoCContainer.ResolveAll<INRenderDevice>(strategy: DIResolutionStrategy.SelfOnly);
                foreach (var d in devs)
                {
                    foreach (var r in d.SupportedRenderers) yield return r;
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<Type> SupportedResources
        {
            get
            {
                var devs = Core.IoCContainer.ResolveAll<INRenderDevice>(strategy: DIResolutionStrategy.SelfOnly);
                foreach (var d in devs)
                {
                    foreach (var r in d.SupportedResources) yield return r;
                }
            }
        }
        #endregion

        public PlatformRenderManager(ILogger logger, IPlatformWindowManager windowManager) : base(logger)
        {
            //m_renderers = new ConcurrentList<INRenderer>();
            m_windowManager = windowManager;
            WireUpApplicationWindowEvents();
        }

        #region Methods
        /// <inheritdoc />
        public TDevice GetDevice<TDevice>(params object[] parameters) where TDevice : class, INRenderDevice
        {
            //if (!m_lockSlim.TryEnter(SynchronizationAccess.Read))
            //    Wait();

            //m_lockSlim.EnterUpgradeableReadLock();//TryEnter(SynchronizationAccess.Read);
            //if (!m_lockSlim.IsUpgradeableReadLockHeld)
            //    Throw.Exception().InvalidOperationException();

            //if (Interlocked.CompareExchange(ref m_hasLock, 1, 0) == 0)
            //{
            //lock (m_lock)
            //{
            m_hasLock = true;
            var devs = Core.IoCContainer.ResolveAll<TDevice>(strategy: DIResolutionStrategy.SelfOnly);
            return devs.First();
            //}
            // };
        }

        public bool IsRendererSupported<TRenderer>() where TRenderer : class, INRender
        {
            //if (!m_lockSlim.TryEnter(SynchronizationAccess.Read))
            //Throw.Exception().InvalidOperationException();

            //m_hasLock = m_lockSlim.TryEnter(SynchronizationAccess.Read);
            //m_lockSlim.EnterUpgradeableReadLock(); //TryEnter(SynchronizationAccess.Read);
            //if (!m_lockSlim.IsUpgradeableReadLockHeld)
            //    Throw.Exception().InvalidOperationException();

            try
            {
                //lock (m_lock)
                //{
                var devs = Core.IoCContainer.ResolveAll<INRenderDevice>(strategy: DIResolutionStrategy.SelfOnly);
                if (m_isInitialized)
                {
                    foreach (var d in devs)
                        d.Initialize();
                }

                //return devs.Any(device => device.SupportedRenderers.ContainsType<TRenderer>());
                return true;
                //}
            }
            finally
            {
                //if (m_hasLock)
                //{
                //    m_lockSlim.ExitUpgradeableReadLock();
                //    m_hasLock = false;
                //}
            }
        }

        /// <inheritdoc />
        public bool IsResourceSupported<TResource>() where TResource : class, INRenderResource
        {
            throw new NotImplementedException();
        }

        public TRenderer GetRenderer<TRenderer>(params object[] parameters) where TRenderer : class, INRender
        {
            Throw.If(!IsRendererSupported<TRenderer>()).InvalidOperationException();

            //if (!m_lockSlim.TryEnter(SynchronizationAccess.Read))
            //    Wait();

            //if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
            //    Throw.Exception().InvalidOperationException();

            //m_lockSlim.EnterUpgradeableReadLock(); //TryEnter(SynchronizationAccess.Read);
            //if (!m_lockSlim.IsUpgradeableReadLockHeld)
            //    Throw.Exception().InvalidOperationException();

            //m_hasLock = true;

            //lock(m_lock)
            //{
            var renderer = Core.IoCContainer.ResolveAll<INRenderDevice>(strategy: DIResolutionStrategy.SelfOnly)
                               .Where(d => d.SupportedRenderers.ContainsType<TRenderer>())
                               .OrderBy(d => d.Priority)
                               .First().GetRenderer<TRenderer>(parameters);

            if (m_isInitialized)
                renderer.Initialize();

            return renderer;
            //}
        }

        public TRenderer[] GetRenderers<TRenderer>(params object[] parameters) where TRenderer : class, INRender
        {
            /*Throw.If(*/
            if (!IsRendererSupported<TRenderer>())
                return Array.Empty<TRenderer>();

            if (!m_lockSlim.TryEnter(SynchronizationAccess.Read))
                Wait();

            if (!m_lockSlim.IsReadLockHeld && !m_lockSlim.TryEnter(SynchronizationAccess.Read))
                Throw.Exception().InvalidOperationException();

            //m_lockSlim.EnterUpgradeableReadLock(); //TryEnter(SynchronizationAccess.Read);
            //if (!m_lockSlim.IsUpgradeableReadLockHeld)
            //    Throw.Exception().InvalidOperationException();

            //m_hasLock = true;

            //lock(m_lock)
            //{

            //var renderers = new ConcurrentList<TRenderer>();
            //foreach (var dev in Core.IoCContainer.ResolveAll<INRenderDevice>().OrderBy(d => d.Priority))
            //{
            //    renderers.Add(dev.GetRenderer<TRenderer>(parameters));
            //}
            var renderers = Core.IoCContainer.ResolveAll<INRenderDevice>(strategy: DIResolutionStrategy.SelfOnly)
                                .Where(d => d.SupportedRenderers.ContainsType<TRenderer>())
                                .OrderBy(d => d.Priority)
                                .Select(d => d.GetRenderer<TRenderer>(parameters))
                                .DistinctBy(r => r.GetType());

            var nRenderers = renderers as TRenderer[] ?? renderers.ToArray();
            if (m_isInitialized)
            {
                foreach (var renderer in nRenderers)
                    renderer.Initialize();
            }

            //var renderers = m_renderers;
            //m_renderers = m_renderers.AddRange(nRenderers
            //                                    .Select(r => r as INRenderer))
            //                       .OrderBy(r => r.Stage)
            //                       .ThenBy(r => r.Priority)
            //                       .ToList();
            return nRenderers.ToArray();
            //}
        }

        /// <inheritdoc />
        public TResource GetResource<TResource>(params object[] parameters) where TResource : class, INResource
        {
            throw new NotImplementedException();
        }

        protected virtual void OnWindowCreated(object sender, INWindow window)
        {
            //if ()
            var win = window;
            if (!window.IsRenderable)
                return;

            if (!window.IsInitialized)
            {
                m_logger.LogError(@$"PlatformRenderManager:
/r/nWindow:{win.Handle} 
/r/nWindow Initialized:{win.IsInitialized}
/r/nDevice Initialized:{m_isInitialized}");
                return;
            }
            //    window.Initialize();

            var devs = Core.IoCContainer.ResolveAll<INRenderDevice>(strategy: DIResolutionStrategy.SelfOnly);
            foreach (var dev in devs)
            {
                dev.Create();
                //dev.Initialize();
            }

            //foreach (var dev in devs.Select(d => d as INSwapChainDevice))
            //{
            //    dev?.CreateSwapChain(window);
            //}

            var renderer = GetRenderers<INWindowRenderer>(window);

            foreach (var dev in devs.Select(d => d as INContextDevice))
            {
                dev?.CreateContext(window);
            }

            //window.AddRenderer(renderer);
                //foreach (var r in renderer)
                //{
                //r.Initialize();
                //if (!r.OwnsRenderLoop)
                //r.Render();
                //}
            }

        protected virtual void OnWindowDestroyed(object sender, INWindow window)
        {
            var win = window;
            if (!window.IsRenderable)
                return;

            //if (window.IsMainApplicationWindow) 
            //    return;

            //lock(m_renderers)
            //{

            var renderers = GetRenderers<INWindowRenderer>(window);
            foreach (var ren in renderers) ren.Dispose();
        }

        /// <inheritdoc />
        protected override void CreateManager(params AssemblyRenderingAttribute[] managers)
        {
            foreach (var m in managers)
            {
                if (m.RenderDeviceType == null)
                    continue;

                var devs = Core.IoCContainer.ResolveAll<INRenderDevice>(strategy: DIResolutionStrategy.SelfOnly);
                if (devs.All(d => d.GetType() != m.RenderDeviceType))
                    Core.IoCContainer.Register(m.RenderDeviceType);
            }
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            foreach (var device in Core.IoCContainer.ResolveAll<INRenderDevice>(strategy: DIResolutionStrategy.SelfOnly))
                device.Dispose();

            //lock(m_renderers)
            //{
            //foreach (var ren in m_renderers)
            //    ren.Dispose();
            //}

            base.DisposeManagedResources();
        }

        protected override void InitializeResources()
        {
            base.InitializeResources();

            if (m_isInitialized)
                return;

            m_supportedMessageIds = new[] {MessageIds.Rendering, MessageIds.Window, MessageIds.Quit};

            //foreach (var device in Core.IoCContainer.ResolveAll<INRenderDevice>(strategy: DIResolutionStrategy.SelfOnly))
            //{
            //    device.Create();
            //    /*Core.Dispatcher.InvokeAsync(() => */
            //    device.Initialize(); //);
            //}
                

            //m_windowManager.WindowDestroyed += WindowDestroyed;
            //m_windowManager.WindowCreated += WindowCreated;
            //foreach (var renderer in m_renderers)
            //    Core.Dispatcher.InvokeAsync(() => renderer.Initialize());
        }

        protected IPlatformMessage m_prevMessage = new PlatformMessage();

        protected override void OnProcessMessage(IPlatformMessage message)
        {
            m_logger.LogDebug(@"**--Platform Render Manager Message Handler.\r\n" +
                              $"MessageId: {message.Id}");

            if (m_prevMessage.Equals(message))
                return;

            switch (message.Id)
            {
                case MessageIds.Quit:
                    return;
                case MessageIds.Rendering:
                    //m_logger.LogDebug("Found Rendering Messages.");
                    var data2 = message.RawData as IWindowMessageData;
                    break;
                case MessageIds.Window:
                    //m_logger.LogDebug("Found Windowing Messages.");
                    var data = message.RawData as IWindowMessageData;
                    switch (data?.MessageId)
                    {
                        case WindowMessageIds.None:
                            break;
                        case WindowMessageIds.Created:
                            WindowCreated.Raise(this, data.Window);
                            break;
                        case WindowMessageIds.Destroyed:
                            WindowDestroyed.Raise(this, data.Window);
                            break;
                        case WindowMessageIds.Resizing:
                        case WindowMessageIds.Moving:
                        case WindowMessageIds.Activated:
                            //foreach (var r in m_renderers.Where(r => !r.ContainsInterface<IFrameBufferRenderer>()))
                            //{
                            //    /*Core.Dispatcher.InvokeAsync(() => */r.Invalidate()/*)*/;
                            //    Core.MessagePump.PushRenderMessage(RenderMessageIds.Invalidating, r);
                            //}
                            break;
                    }

                    break;
            }

            foreach (var device in Core.IoCContainer.ResolveAll<INRenderDevice.INRenderDevicePump>(strategy: DIResolutionStrategy.SelfOnly))
                Core.Dispatcher.InvokeAsync(() => device.Push(message));

            m_prevMessage = message;
            base.OnProcessMessage(message);
        }

        /// <inheritdoc />
        protected override void RunManager(CancellationToken token)
        {
            base.RunManager(token);

            var devs = Core.IoCContainer.ResolveAll<INRenderDevice.INRenderDevicePump>(strategy: DIResolutionStrategy.SelfOnly);
            //var devices = devs as INRenderDevice[] ?? devs.ToArray();
            var tasks = new ConcurrentList<Task>();
            Task.Run(() =>
                     {
                         foreach (var device in devs)
                             device.Pump(token);
                     },
                     token);

            //var whenAll = Task.WhenAll(m_tasks);
            //Task.WhenAll(whenAll).ConfigureAwait(false);
            //for (;;)
            //{
            //    while (!whenAll.IsCompleted)
            //    {
            //        Console.Write(".");
            //        Thread.Sleep(500);
            //    }

            //    break;
            //}

            //foreach (var renderer in m_renderers.Where(r => r.HandleRenderLoop)/*.Where(r  => !r.ContainsInterface<IFrameBufferRenderer>())*/)
            //{
            //    Core.Dispatcher.InvokeAsync(() => renderer.Render());
            //renderer.Render();
            //}

            foreach (var device in devs)
                device.Wait();
        }

        private void WireUpApplicationWindowEvents()
        {
            //WindowCreated += OnWindowCreated;
            //WindowDestroyed += OnWindowDestroyed;

            m_windowManager.WindowCreated += OnWindowCreated;
            m_windowManager.WindowDestroyed += OnWindowDestroyed;

        }
        #endregion

        //{
        //    add { Core.IoCContainer.Resolve<IPlatformWindowManager>().WindowDestroyed += value; }
        //    remove { Core.IoCContainer.Resolve<IPlatformWindowManager>().WindowDestroyed -= value; }
        //}
    }
}