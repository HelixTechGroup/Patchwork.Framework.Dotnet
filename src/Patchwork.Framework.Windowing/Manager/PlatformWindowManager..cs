#region Usings
using System;
using System.Collections.Generic;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
#endregion

namespace Patchwork.Framework.Manager
{
    public class PlatformWindowManager :
        PlatformManager<AssemblyWindowingAttribute, IPlatformMessage<IWindowMessageData>>,
        IPlatformWindowManager
    {
        #region Events
        /// <inheritdoc />
        public event EventHandler<INWindow> WindowCreated;

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowDestroyed;
        #endregion

        #region Members
        protected INWindow m_currentWindow;
        protected INWindow m_mainWindow;
        protected IList<INWindow> m_windows;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INWindow CurrentWindow
        {
            get { return m_currentWindow; }
        }

        /// <inheritdoc />
        public INWindow MainWindow
        {
            get { return m_mainWindow; }
        }

        /// <inheritdoc />
        public IEnumerable<INWindow> Windows
        {
            get { return m_windows; }
        }
        #endregion

        #region Methods
        /// <inheritdoc />
        public INWindow CreateWindow()
        {
            return CreateWindow(NWindowDefinition.Default);
        }

        public INWindow CreateWindow(NWindowDefinition definition)
        {
            Throw.If(!m_isInitialized).InvalidOperationException();
            var win = Core.IoCContainer.Resolve<INWindow>(null, Core.Application, definition);
            win.Create();
            m_currentWindow = win;

            return win;
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
            //if (win.IsMainApplicationWindow)
            //    Core.MessagePump.Push(new PlatformMessage(MessageIds.Quit));
        }

        protected override void CreateManager(params AssemblyWindowingAttribute[] managers)
        {
            base.CreateManager(managers);

            foreach (var m in managers)
            {
                Core.IoCContainer.Register(m.WindowType, false);
            }
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
        }

        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_windows = new ConcurrentList<INWindow>();
            m_supportedMessageIds = new[] {MessageIds.Window, MessageIds.Quit};
            WireUpApplicationWindowEvents();

        }

        /// <inheritdoc />
        protected override void OnProcessMessage(IPlatformMessage message)
        {
            switch (message.Id)
            {
                case MessageIds.Window:
                    Core.Logger.LogDebug("Found Windowing Messages.");
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
                    }

                    break;
                case MessageIds.Rendering:
                    //foreach (var window in m_windows)
                    //    window.Render();
                    //Core.Logger.LogDebug("Render message found.");
                    break;
            }

            if (message.Id != MessageIds.Quit)
                foreach (var window in m_windows)
                    window.SyncDataCache();

            base.OnProcessMessage(message);
        }

        private void WireUpApplicationWindowEvents()
        {
            WindowCreated += OnWindowCreated;
            WindowDestroyed += OnWindowDestroyed;
        }
        #endregion
    }
}