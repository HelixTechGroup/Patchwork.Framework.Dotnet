#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Windowing;
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

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowDestroyed;
        #endregion

        #region Members
        protected IList<INRenderDevice> m_devices;
        protected IList<INRenderer> m_renderers;
        #endregion

        protected PlatformRenderManager()
        {
            m_devices = new ConcurrentList<INRenderDevice>();
            m_renderers = new ConcurrentList<INRenderer>();

            WindowCreated += OnWindowCreated;
            WindowDestroyed += OnWindowDestroyed;
        }

        #region Methods
        public bool IsRendererSupported<TRenderer>() where TRenderer : INRenderer
        {
            return m_devices.Any(device => device.SupportedRenderers.ContainsType<TRenderer>());
        }

        public TRenderer GetRenderer<TRenderer>(params object[] parameters) where TRenderer : INRenderer
        {
            Throw.If(!IsRendererSupported<TRenderer>()).InvalidOperationException();
            var devs = m_devices
                      .Where(d => d.SupportedRenderers.ContainsType<TRenderer>())
                      .OrderBy(d => d.Priority)
                      .First().GetRenderer<TRenderer>(parameters);

            return devs;
        }

        protected virtual void OnWindowCreated(object sender, INWindow window)
        {
            var win = window;
            if (!window.IsRenderable)
                return;

            foreach (var dev in m_devices)
            {
                var renderer = GetRenderer<INWindowRenderer>(window);
                //renderer.Initialize(window, dev);
                m_renderers.Add(renderer);
            }
        }

        protected virtual void OnWindowDestroyed(object sender, INWindow window)
        {
            var win = window;
            if (!window.IsRenderable)
                return;

            var renderers = m_renderers.Where(e => e.ContainsInterface(typeof(INWindowRenderer)) && (e as INWindowRenderer).Window == window);
            foreach (var ren in renderers)
            {
                m_renderers.Remove(ren);
                ren.Dispose();
            }
        }

        /// <inheritdoc />
        protected override void CreateManager(params AssemblyRenderingAttribute[] managers)
        {
            m_devices = new ConcurrentList<INRenderDevice>();
            foreach (var m in managers)
            {
                if (m.RenderDeviceType == null)
                    continue;

                if (m_devices.All(d => d.GetType() != m.RenderDeviceType))
                    m_devices.Add(Activator.CreateInstance(m.RenderDeviceType, Core.IoCContainer) as INRenderDevice);
            }
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            foreach (var device in m_devices)
                device.Dispose();

            foreach (var ren in m_renderers)
                ren.Dispose();

            base.DisposeManagedResources();
        }

        protected override void InitializeResources()
        {
            base.InitializeResources();

            if (m_isInitialized)
                return;

            m_supportedMessageIds = new[] {MessageIds.Rendering, MessageIds.Window, MessageIds.Quit};

            foreach (var device in m_devices)
                device.Initialize();
        }

        protected override void OnProcessMessage(IPlatformMessage message)
        {
            base.OnProcessMessage(message);

            foreach (var device in m_devices)
                Task.Run(() => { device.Push(message); });

            switch (message.Id)
            {
                case MessageIds.Rendering:
                    Core.Logger.LogDebug("Found Rendering Messages.");
                    var data2 = message as IPlatformMessage<IRenderMessageData>;
                    break;
                case MessageIds.Window:
                    Core.Logger.LogDebug("Found Windowing Messages.");
                    var data = (message as IPlatformMessage<IWindowMessageData>).Data;
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
                    }

                    break;
            }
        }

        /// <inheritdoc />
        protected override void RunManager()
        {
            base.RunManager();
        }

        private void WireUpApplicationWindowEvents()
        {
            WindowCreated += OnWindowCreated;
            WindowDestroyed += OnWindowDestroyed;
        }
        #endregion
    }
}