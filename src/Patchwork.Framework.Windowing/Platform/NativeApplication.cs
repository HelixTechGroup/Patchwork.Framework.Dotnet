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
    public abstract partial class NativeApplication : Initializable, INativeApplication
    {
        protected IList<INativeWindow> m_windows;
        protected INativeWindow m_mainWindow;
        protected INativeWindow m_currentWindow;

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

        partial void ConstructionShared()
        {
            m_windows = new ConcurrentList<INativeWindow>();
            WireUpApplicationWindowEvents();
        }

        partial void InitializeResourcesShared()
        {
            Core.ProcessMessage -= OnProcessMessageWindowing;
        }

        /// <inheritdoc />
        public INativeWindow CreateWindow()
        {
            Throw.If(!m_isInitialized).InvalidOperationException();
            var win = PlatformCreateWindow();

            return win;
        }

        protected abstract INativeWindow PlatformCreateWindow();

        partial void PumpMessagesShared(CancellationToken cancellationToken)
        {
            foreach (var window in m_windows)
                    window.SyncDataCache();
        }

        protected virtual void OnWindowCreated(object sender, INativeWindow window)
        {
            m_windows.Add(window);

            if (window.IsMainApplicationWindow && m_mainWindow is null)
                m_mainWindow = window;
        }

        protected virtual void OnWindowDestroyed(object sender, INativeWindow window)
        {
            m_windows.Remove(window);
        }

        private void WireUpApplicationWindowEvents()
        {
            WindowCreated += OnWindowCreated;
            WindowDestroyed += OnWindowDestroyed;
        }

        protected virtual void OnProcessMessageWindowing(IPlatformMessage message)
        {
            if (!m_isInitialized)
                return;

            switch (message.Id)
            {
                case MessageIds.Quit:
                    break;
                case MessageIds.Window:
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
        }
    }
}
