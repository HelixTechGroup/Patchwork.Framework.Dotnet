#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Threading;
using Patchwork.Framework.Platform.Windowing;

using Shield.Framework.IoC.Native.DependencyInjection;
using Shield.Framework.Threading;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.Threading;
#endregion

namespace Patchwork.Framework.Manager
{
    public class PlatformRenderManager : PlatformManager<AssemblyRenderingAttribute,
                                                    IPlatformMessage<IRenderMessageData>>,
                                                    IPlatformRenderManager
    {
        #region Events
        public event EventHandler<INWindow> WindowCreated;
        //{
        //    add { Core.IoCContainer.Resolve<IPlatformWindowManager>().WindowCreated += value; }
        //    remove { Core.IoCContainer.Resolve<IPlatformWindowManager>().WindowCreated -= value; }
        //}

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowDestroyed;
        //{
        //    add { Core.IoCContainer.Resolve<IPlatformWindowManager>().WindowDestroyed += value; }
        //    remove { Core.IoCContainer.Resolve<IPlatformWindowManager>().WindowDestroyed -= value; }
        //}
        #endregion

        #region Members
        //protected IList<INRenderer> m_renderers;
        protected IPlatformWindowManager m_windowManager;
        #endregion

        public PlatformRenderManager()
        {
            //m_renderers = new ConcurrentList<INRenderer>();

            WireUpApplicationWindowEvents();
        }
        #region Methods
        /// <inheritdoc />
        public TDevice GetDevice<TDevice>(params object[] parameters) where TDevice : INRenderDevice
        {
            //if (!m_lockSlim.TryEnter(SynchronizationAccess.Read))
            //    Wait();

            //m_lockSlim.EnterUpgradeableReadLock();//TryEnter(SynchronizationAccess.Read);
            //if (!m_lockSlim.IsUpgradeableReadLockHeld)
            //    Throw.Exception().InvalidOperationException();

            //if (Interlocked.CompareExchange(ref m_hasLock, 1, 0) == 0)
            //{
            try
            {
                    //lock (m_lock)
                    //{
                        m_hasLock = true;
                        var devs = Core.IoCContainer.ResolveAll<TDevice>();
                        return devs.First();
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
           // };
            
        }

        public bool IsRendererSupported<TRenderer>() where TRenderer : INRenderer
        {

            //if (!m_lockSlim.TryEnter(SynchronizationAccess.Read))
            //Throw.Exception().InvalidOperationException();

            //m_hasLock = m_lockSlim.TryEnter(SynchronizationAccess.Read);
            m_lockSlim.EnterUpgradeableReadLock(); //TryEnter(SynchronizationAccess.Read);
            if (!m_lockSlim.IsUpgradeableReadLockHeld)
                Throw.Exception().InvalidOperationException();

            try
            {
                //lock (m_lock)
                //{
                    var devs = Core.IoCContainer.ResolveAll<INRenderDevice>();
                    if (m_isInitialized)
                        foreach (var d in devs)
                            d.Initialize();

                    return devs.Any(device => device.SupportedRenderers.ContainsType<TRenderer>());
                //}
            }
            finally

            {
                if (m_hasLock)
                {
                    m_lockSlim.ExitUpgradeableReadLock();
                    m_hasLock = false;
                }
            }
        }

        public TRenderer GetRenderer<TRenderer>(params object[] parameters) where TRenderer : INRenderer
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

            try
            {
                //lock(m_lock)
                //{
                    var renderer = Core.IoCContainer.ResolveAll<INRenderDevice>()
                                       .Where(d => d.SupportedRenderers.ContainsType<TRenderer>())
                                       .OrderBy(d => d.Priority)
                                       .First().GetRenderer<TRenderer>(parameters);

                    if (m_isInitialized)
                        renderer.Initialize();

                    return renderer;
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

        public TRenderer[] GetRenderers<TRenderer>(params object[] parameters) where TRenderer : INRenderer
        {
            /*Throw.If(*/
            if (!IsRendererSupported<TRenderer>())
                return Array.Empty<TRenderer>();

            //if (!m_lockSlim.TryEnter(SynchronizationAccess.Read))
            //    Wait();

            //if (!m_lockSlim.TryEnter(SynchronizationAccess.Read))
            //    Throw.Exception().InvalidOperationException();

            //m_lockSlim.EnterUpgradeableReadLock(); //TryEnter(SynchronizationAccess.Read);
            //if (!m_lockSlim.IsUpgradeableReadLockHeld)
            //    Throw.Exception().InvalidOperationException();

            //m_hasLock = true;

            try
            {
                //lock(m_lock)
                //{
                    var renderers = Core.IoCContainer.ResolveAll<INRenderDevice>()
                                        .Where(d => d.SupportedRenderers.ContainsType<TRenderer>())
                                        .OrderBy(d => d.Priority)
                                        .Select(d => d.GetRenderer<TRenderer>(parameters))
                                        .DistinctBy(r => r.GetType());

                    var nRenderers = renderers as TRenderer[] ?? renderers.ToArray();
                    if (m_isInitialized)
                        foreach (var renderer in nRenderers)
                            renderer.Initialize();

                    //var renderers = m_renderers;
                    //m_renderers = m_renderers.AddRange(nRenderers
                    //                                    .Select(r => r as INRenderer))
                    //                       .OrderBy(r => r.Stage)
                    //                       .ThenBy(r => r.Priority)
                    //                       .ToList();
                    return nRenderers.ToArray();
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

        protected virtual void OnWindowCreated(object sender, INWindow window)
        {
            var win = window;
                    if (!window.IsRenderable)
                        return;

            if (!window.IsInitialized)
            {
                Core.Logger.LogError(@$"PlatformRenderManager:
/r/nWindow:{win.Handle.ToString} 
/r/nWindow Initialized:{win.IsInitialized}
/r/nDevice Initialized:{m_isInitialized}");
                return;
            }
                    //    window.Initialize();

                    foreach (var dev in Core.IoCContainer.ResolveAll<INRenderDevice>())
                    {
                        dev.Context?.Create(window);
                    }

                    var renderer = GetRenderers<INWindowRenderer>(window);
                    foreach(var r in renderer)
                    {
                        r.Initialize();
                        if (!r.OwnsRenderLoop)
                            r.Render();
                    }

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
            foreach (var ren in renderers)
            {
                ren.Dispose();
            }
        }

        /// <inheritdoc />
        protected override void CreateManager(params AssemblyRenderingAttribute[] managers)
        {
            foreach (var m in managers)
            {
                if (m.RenderDeviceType == null)
                    continue;

                var devs = Core.IoCContainer.ResolveAll<INRenderDevice>();
                if (devs.All(d => d.GetType() != m.RenderDeviceType))
                    Core.IoCContainer.Register(m.RenderDeviceType, true);
            }
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            foreach (var device in Core.IoCContainer.ResolveAll<INRenderDevice>())
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

            foreach (var device in Core.IoCContainer.ResolveAll<INRenderDevice>())
                Core.Dispatcher.InvokeAsync(() =>  device.Initialize());

            Core.Window.WindowDestroyed += WindowDestroyed;
            Core.Window.WindowCreated += WindowCreated;
            //foreach (var renderer in m_renderers)
            //    Core.Dispatcher.InvokeAsync(() => renderer.Initialize());
        }

        protected override void OnProcessMessage(IPlatformMessage message)
        {
            switch (message.Id)
            {
                case MessageIds.Quit:
                    return;
                case MessageIds.Rendering:
                    //Core.Logger.LogDebug("Found Rendering Messages.");
                    var data2 = message.RawData as IWindowMessageData;
                    break;
                case MessageIds.Window:
                    //Core.Logger.LogDebug("Found Windowing Messages.");
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

            foreach (var device in Core.IoCContainer.ResolveAll<INRenderDevice>())
                Core.Dispatcher.InvokeAsync(() => device.Push(message));

            base.OnProcessMessage(message);
        }

        /// <inheritdoc />
        protected override void RunManager(CancellationToken token)
        {
            base.RunManager(token);

            var devs = Core.IoCContainer.ResolveAll<INRenderDevice>();
            var devices = devs as INRenderDevice[] ?? devs.ToArray();
            var tasks = new ConcurrentList<Task>();
            foreach (var device in devices)
                device.Pump(token);

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

            foreach (var device in devices)
                device.Wait();
        }

        private void WireUpApplicationWindowEvents()
        {
            WindowCreated += OnWindowCreated;
            WindowDestroyed += OnWindowDestroyed;
        }
        #endregion
    }
}