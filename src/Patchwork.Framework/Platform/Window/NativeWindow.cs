#region Usings
using System;
using System.Drawing;
using Patchwork.Framework.Messaging;
using Shin.Framework;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;
#endregion

namespace Patchwork.Framework.Platform.Window
{
    public abstract class NativeWindow : Initializable, INativeWindow
    {
        #region Events
        public event EventHandler Closing;
        public event EventHandler Closed;
        public event EventHandler Created;
        public event EventHandler Destroyed;
        public event EventHandler Activating;
        public event EventHandler Activated;
        public event EventHandler Deactivating;
        public event EventHandler Deactivated;
        public event EventHandler Hidden;
        public event EventHandler Shown;
        public event EventHandler<PropertyChangedEventArgs<string>> TitleChanged; 
        #endregion

        #region Members
        protected string m_title;
        protected string m_previousTitle;
        protected INativeHandle m_handle;
        protected INativeInput m_input;
        protected bool m_isMainApplicationWindow;
        protected bool m_isVisible;
        protected INativeObject m_parent;
        protected INativeWindowRenderer m_renderer;
        protected NativeWindowType m_type;
        protected bool m_isFirstTimeVisible;
        protected bool m_isCreated;
        private bool m_isActive;
        #endregion

        #region Properties
        /// <inheritdoc />
        public Rectangle ClientArea
        {
            get
            {
                return !m_isInitialized ? Rectangle.Empty : PlatformGetClientArea();
            }
        }

        /// <inheritdoc />
        public Size ClientSize
        {
            get { return !m_isInitialized ? Size.Empty : PlatformGetClientSize(); }
        }

        /// <inheritdoc />
        public INativeHandle Handle
        {
            get { return m_handle; }
        }

        /// <inheritdoc />
        public INativeInput Input
        {
            get { return m_input; }
        }

        /// <inheritdoc />
        public bool IsMainApplicationWindow
        {
            get { return m_isMainApplicationWindow; }
        }

        /// <inheritdoc />
        public bool IsActive
        {
            get { return m_isActive; }
        }

        /// <inheritdoc />
        public bool IsVisible
        {
            get { return m_isVisible; }
        }

        /// <inheritdoc />
        public Size MaxClientSize
        {
            get { return !m_isInitialized ? Size.Empty : PlatformGetMaxClientSize(); }
        }

        /// <inheritdoc />
        public INativeObject Parent
        {
            get { return m_parent; }
        }

        /// <inheritdoc />
        public INativeWindowRenderer Renderer
        {
            get { return m_renderer; }
        }

        /// <inheritdoc />
        public string Title
        {
            get { return m_title; }
            set { SetWindowTitle(value); }
        }

        /// <inheritdoc />
        public NativeWindowType Type
        {
            get { return m_type; }
        }
        #endregion

        protected NativeWindow()
        {
            WireUpWindowEvents();
        }

        #region Methods
        protected abstract Rectangle PlatformGetClientArea();

        protected abstract Size PlatformGetClientSize();

        protected abstract Size PlatformGetMaxClientSize();

        protected abstract void PlatformSetTitle(string value);

        protected abstract string PlatformGetTitle();

        protected abstract void PlatformShow();

        protected abstract void PlatformHide();

        protected abstract void PlatformCreate();

        protected abstract void PlatformDestroy();

        /// <inheritdoc />
        public void Deactivate()
        {
            if (m_isInitialized && m_isActive)
            {
                if (PlatformCheckActive())
                {
                    Deactivating.Raise(this, EventArgs.Empty);
                    PlatformDeactivate();
                    Deactivated.Raise(this, EventArgs.Empty);
                }
            }

            m_isActive = PlatformCheckActive();
        }

        protected abstract void PlatformDeactivate();

        protected abstract bool PlatformCheckActive();

        /// <inheritdoc />
        public abstract bool IsPointInWindow(Point point);

        /// <inheritdoc />
        public abstract Point PointToClient(Point point);

        /// <inheritdoc />
        public abstract Point PointToScreen(Point point);

        /// <inheritdoc />
        private void SetWindowTitle(string value)
        {
            if (!m_isInitialized)
                return;
            if (value == m_title)
                return;
            PlatformSetTitle(value);
            var title = PlatformGetTitle();
            TitleChanged.Raise(this, new PropertyChangedEventArgs<string>(title, value, m_title));
            m_previousTitle = m_title;
            m_title = title;
        }

        protected virtual void OnClosing(object sender, EventArgs e) 
        {
            //PlatformManager.MessagePump.Push(new WindowMessage(WindowMessageIds.Closing, this));
        }

        protected virtual void OnClosed(object sender, EventArgs e)
        {
            //PlatformManager.MessagePump.Push(new WindowMessage(WindowMessageIds.Closed, this));
        }

        protected virtual void OnCreated(object sender, EventArgs e)
        {
            //PlatformManager.MessagePump.Push(new WindowMessage(WindowMessageIds.Created, this));
        }

        protected virtual void OnDestroyed(object sender, EventArgs e)
        {
            //PlatformManager.MessagePump.Push(new WindowMessage(WindowMessageIds.Destroyed, this));
        }

        protected virtual void OnActivating(object sender, EventArgs e) 
        {
            //PlatformManager.MessagePump.Push(new WindowMessage(WindowMessageIds.Activating, this));
        }

        protected virtual void OnActivated(object sender, EventArgs e)
        {
            PlatformManager.MessagePump.Push(new WindowMessage(WindowMessageIds.Activated, this));
        }

        protected virtual void OnDeactivating(object sender, EventArgs e)
        {
            //PlatformManager.MessagePump.Push(new WindowMessage(WindowMessageIds.Deactivating, this));
        }

        protected virtual void OnDeactivated(object sender, EventArgs e)
        {
            PlatformManager.MessagePump.Push(new WindowMessage(WindowMessageIds.Deactivated, this));
        }

        protected virtual void OnShown(object sender, EventArgs e)
        {
            //PlatformManager.MessagePump.Push(new WindowMessage(WindowMessageIds.Shown, this));
        }

        protected virtual void OnHidden(object sender, EventArgs e)
        {
            //PlatformManager.MessagePump.Push(new WindowMessage(WindowMessageIds.Hidden, this));
        }

        protected virtual void OnTitleChanged(object sender, PropertyChangedEventArgs<string> e) { }

        protected override void DisposeManagedResources()
        {
            Closing.Dispose();
            Closed.Dispose();
            Created.Dispose();
            Destroyed.Dispose();
            Activating.Dispose();
            Activated.Dispose();
            Deactivating.Dispose();
            Deactivated.Dispose();
            Shown.Dispose();
            Hidden.Dispose();
            TitleChanged.Dispose();
            m_handle = null;
            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        public void Show()
        {
            if (!m_isInitialized)
                return;
            if (m_isVisible)
                return;

            PlatformShow();
            Shown.Raise(this, EventArgs.Empty);            
        }

        /// <inheritdoc />
        public void Hide()
        {
            if (!m_isInitialized)
                return;
            if (!m_isVisible)
                return;

            PlatformHide();
            Hidden.Raise(this, EventArgs.Empty);
            m_isVisible = PlatformCheckVisible();
        }

        /// <inheritdoc />
        public void Create()
        {
            if (m_isCreated)
                return;

            PlatformCreate();
            Created.Raise(this, EventArgs.Empty);

            if (!m_isInitialized)
                Initialize();
        }

        /// <inheritdoc />
        public void Destroy()
        {
            if (m_isInitialized)
            {
                PlatformDestroy();
                Destroyed.Raise(this, EventArgs.Empty);
            }

            Dispose();
        }

        /// <inheritdoc />
        public void Activate()
        {
            if (m_isInitialized && !m_isActive)
            {
                if (!PlatformCheckActive())
                {
                    Activating.Raise(this, EventArgs.Empty);
                    PlatformActivate();
                    Activated.Raise(this, EventArgs.Empty);
                }
            }

            m_isActive = PlatformCheckActive();
        }

        protected abstract void PlatformActivate();

        protected abstract bool PlatformCheckVisible();

        private void WireUpWindowEvents()
        {
            Closing += OnClosing;
            Closed += OnClosed;
            Created += OnCreated;
            Destroyed += OnDestroyed;
            Activating += OnActivating;
            Activated += OnActivated;
            Deactivating += OnDeactivating;
            Deactivated += OnDeactivated;
            Shown += OnShown;
            Hidden += OnHidden;
            TitleChanged += OnTitleChanged;
        }
        #endregion
    }
}