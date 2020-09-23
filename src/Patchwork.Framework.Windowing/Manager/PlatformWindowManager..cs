using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Patchwork.Framework.Manager;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform
{
    public abstract class PlatformWindowManager : 
        PlatformManager<AssemblyWindowingAttribute, IPlatformMessage<IWindowMessageData>>,
        IPlatformWindowManager
    {
        protected IList<INWindow> m_windows;
        protected INWindow m_mainWindow;
        protected INWindow m_currentWindow;

        /// <inheritdoc />
        public INWindow MainWindow
        {
            get { return m_mainWindow; }
        }

        /// <inheritdoc />
        public INWindow CurrentWindow
        {
            get { return m_currentWindow; }
        }

        /// <inheritdoc />
        public IEnumerable<INWindow> Windows
        {
            get { return m_windows; }
        }

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowCreated;

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowDestroyed;

        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_windows = new ConcurrentList<INWindow>();
            m_supportedMessageIds = new[] { MessageIds.Window, MessageIds.Quit };
            WireUpApplicationWindowEvents();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
        }

        protected override void CreateManager(params AssemblyWindowingAttribute[] managers)
        {
            base.CreateManager(managers);
            //m_devices = new ConcurrentList<INRenderDevice>();
            //foreach (var m in managers)
            //{
            //    if (m.RenderDeviceType == null)
            //        continue;

            //    if (m_devices.All(d => d.GetType() != m.RenderDeviceType))
            //        m_devices.Add(Activator.CreateInstance(m.RenderDeviceType) as INRenderDevice);
            //}
        }

        /// <inheritdoc />
        public INWindow CreateWindow()
        {
            return CreateWindow(NWindowDefinition.Default);
        }

        public INWindow CreateWindow(NWindowDefinition definition)
        {
            Throw.If(!m_isInitialized).InvalidOperationException();
            var win = PlatformCreateWindow(definition);

            return win;
        }

        protected abstract INWindow PlatformCreateWindow(NWindowDefinition definition);

        protected void OnProcessMessageWindow(IPlatformMessage<IRenderMessageData> message)
        {

        }
        /// <inheritdoc />
        protected override void OnProcessMessage(IPlatformMessage message)
        {
            base.OnProcessMessage(message);

            switch (message.Id)
            {
                case MessageIds.Window:
                    Core.Logger.LogDebug("Found Windowing Messages.");
                    var data = (message.RawData as IWindowMessageData);
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

            foreach (var window in m_windows)
                    window.SyncDataCache();
        }

        protected virtual void OnWindowCreated(object sender, INWindow window)
        {
            var win = window;
            m_windows.Add(win);

            if (window.IsMainApplicationWindow && m_mainWindow is null)
                m_mainWindow = win;
        }

        protected virtual void OnWindowDestroyed(object sender, INWindow window)
        {
            var win = window;
            m_windows.Remove(win);
        }

        private void WireUpApplicationWindowEvents()
        {
            WindowCreated += OnWindowCreated;
            WindowDestroyed += OnWindowDestroyed;
        }
    }
}
