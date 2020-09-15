using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform
{
    public abstract class WindowManager : 
        PlatformManager<AssemblyWindowingAttribute, IPlatformMessage<IWindowMessageData>>,
        IWindowManager
    {
        protected IList<INativeWindow> m_windows;
        protected INativeWindow m_mainWindow;
        protected INativeWindow m_currentWindow;
        private bool m_isRunning;
        private IPlatformMessagePump m_messagePump;

        /// <inheritdoc />
        public INativeWindow MainWindow
        {
            get { return m_mainWindow; }
        }

        /// <inheritdoc />
        public INativeWindow CurrentWindow
        {
            get { return m_mainWindow; }
        }

        /// <inheritdoc />
        public IEnumerable<INativeWindow> Windows
        {
            get { return m_windows; }
        }

        /// <inheritdoc />
        public event EventHandler<INativeWindow> WindowCreated;

        /// <inheritdoc />
        public event EventHandler<INativeWindow> WindowDestroyed;

        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_windows = new ConcurrentList<INativeWindow>();
            WireUpApplicationWindowEvents();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
        }

        protected override void CreateManager(params AssemblyWindowingAttribute[] managers)
        {
            //m_devices = new ConcurrentList<INativeRenderDevice>();
            //foreach (var m in managers)
            //{
            //    if (m.RenderDeviceType == null)
            //        continue;

            //    if (m_devices.All(d => d.GetType() != m.RenderDeviceType))
            //        m_devices.Add(Activator.CreateInstance(m.RenderDeviceType) as INativeRenderDevice);
            //}
        }

        /// <inheritdoc />
        public INativeWindow CreateWindow()
        {
            return CreateWindow(NativeWindowDefinition.Default);
        }

        public INativeWindow CreateWindow(NativeWindowDefinition definition)
        {
            Throw.If(!m_isInitialized).InvalidOperationException();
            var win = PlatformCreateWindow(definition);

            return win;
        }

        protected abstract INativeWindow PlatformCreateWindow(NativeWindowDefinition definition);

        /// <inheritdoc />
        protected override void OnProcessMessage(IPlatformMessage<IWindowMessageData> message)
        {
            base.OnProcessMessage(message);

            switch (message.Id)
            {
                case MessageIds.Window:
                    Core.Logger.LogDebug("Found Windowing Messages.");
                    var data = message.Data as WindowMessageData;
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

        protected virtual void OnWindowCreated(object sender, INativeWindow window)
        {
            var win = window;
            m_windows.Add(win);

            if (window.IsMainApplicationWindow && m_mainWindow is null)
                m_mainWindow = win;
        }

        protected virtual void OnWindowDestroyed(object sender, INativeWindow window)
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
