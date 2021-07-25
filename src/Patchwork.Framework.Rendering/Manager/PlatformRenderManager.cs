#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Threading;
using Patchwork.Framework.Platform.Windowing;
using Shield.Framework.Threading;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
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
        #endregion

        public PlatformRenderManager()
        {
            //m_renderers = new ConcurrentList<INRenderer>();

            WindowCreated += OnWindowCreated;
            WindowDestroyed += OnWindowDestroyed;

        }

        #region Methods
        public bool IsRendererSupported<TRenderer>() where TRenderer : INRenderer
        {
            var devs = Core.IoCContainer.ResolveAll<INRenderDevice>();
            if (m_isInitialized)
                foreach (var d in devs)
                    d.Initialize();

            return devs.Any(device => device.SupportedRenderers.ContainsType<TRenderer>());
        }

        public TRenderer GetRenderer<TRenderer>(params object[] parameters) where TRenderer : INRenderer
        {
            Throw.If(!IsRendererSupported<TRenderer>()).InvalidOperationException();
            var renderer = Core.IoCContainer.ResolveAll<INRenderDevice>()
                      .Where(d => d.SupportedRenderers.ContainsType<TRenderer>())
                      .OrderBy(d => d.Priority)
                      .First().GetRenderer<TRenderer>(parameters);

            if (m_isInitialized)
                renderer.Initialize();

            return renderer;
        }

        public TRenderer[] GetRenderers<TRenderer>(params object[] parameters) where TRenderer : INRenderer
        {
            Throw.If(!IsRendererSupported<TRenderer>()).InvalidOperationException();
            var renderers = Core.IoCContainer.ResolveAll<INRenderDevice>()
                           .Where(d => d.SupportedRenderers.ContainsType<TRenderer>())
                           .OrderBy(d => d.Priority)
                           .Select(r => r.GetRenderer<TRenderer>(parameters));

            if (m_isInitialized)
                foreach (var renderer in renderers)
                    renderer.Initialize();

            //var renderers = m_renderers;
            //m_renderers = m_renderers.AddRange(nRenderers
            //                                    .Select(r => r as INRenderer))
            //                       .OrderBy(r => r.Stage)
            //                       .ThenBy(r => r.Priority)
            //                       .ToList();
            return renderers.ToArray();
        }

        protected virtual void OnWindowCreated(object sender, INWindow window)
        {
            var win = window;
            if (!window.IsRenderable)
                return;

            if (!window.IsInitialized)
                window.Initialize();

            //foreach (var dev in Core.IoCContainer.ResolveAll<INRenderDevice>())
            //{
                var renderer = GetRenderers<INWindowRenderer>(window);
                //m_renderers.AddRange(renderer);
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
            //foreach (var ren in renderers)
            //{
            //    m_renderers.Remove(ren);
            //    ren.Dispose();
            //}
            //}
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
                    Core.IoCContainer.Register(m.RenderDeviceType);
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
                Core.Dispatcher.InvokeAsync(() => device.Initialize());

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