#region Usings
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Threading;
using Patchwork.Framework.Messaging;
using Shin.Framework;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;
using Patchwork.Framework.Platform;
#endregion

namespace Patchwork.Framework.Platform.Window
{
    public abstract partial class NWindow : Initializable, INWindow, IEquatable<NWindow>
    {
        #region Events
        public event EventHandler Activated;
        public event EventHandler Activating;
        public event EventHandler Closed;
        public event EventHandler Closing;
        public event EventHandler Created;
        public event EventHandler Deactivated;
        public event EventHandler Deactivating;
        public event EventHandler Destroyed;
        public event EventHandler<PropertyChangedEventArgs<bool>> FocusChanged;
        public event EventHandler<PropertyChangingEventArgs<bool>> FocusChanging;
        public event EventHandler FocusGained;
        public event EventHandler FocusLost;
        public event EventHandler Enabled;
        public event EventHandler Disabled;
        public event EventHandler<PropertyChangedEventArgs<Point>> PositionChanged;
        public event EventHandler<PropertyChangingEventArgs<Point>> PositionChanging;
        public event EventHandler<PropertyChangedEventArgs<Size>> SizeChanged;
        public event EventHandler<PropertyChangingEventArgs<Size>> SizeChanging;
        public event EventHandler<PropertyChangedEventArgs<string>> TitleChanged;
        #endregion

        #region Members
        internal IWindowDataCache m_cache;
        protected INHandle m_handle;
        protected INInput m_input;
        protected bool m_isCreated;
        protected bool m_isMainApplicationWindow;
        protected readonly INObject m_parent;
        //protected INWindowRenderer m_renderer;
        #endregion

        #region Properties

        /// <inheritdoc />
        public Rectangle ClientArea
        {
            get { return m_cache.ClientArea; }
        }

        /// <inheritdoc />
        public Size ClientSize
        {
            get { return m_cache.ClientSize; }
        }

        /// <inheritdoc />
        public INHandle Handle
        {
            get { return m_handle; }
        }

        public INInput Input
        {
            get { return m_input; }
        }

        /// <inheritdoc />
        public bool IsActive
        {
            get { return m_cache.IsActive; }
        }

        /// <inheritdoc />
        public bool IsEnabled
        {
            get { return m_cache.IsEnabled; }
        }

        /// <inheritdoc />
        public bool IsFocused
        {
            get { return m_cache.IsFocused; }
        }

        /// <inheritdoc />
        public bool IsMainApplicationWindow
        {
            get { return m_isMainApplicationWindow; }
        }

        public bool IsResizable
        {
            get { return m_cache.IsResizable; }
            set { m_cache.IsResizable = value; }
        }

        /// <inheritdoc />
        public bool IsVisible
        {
            get { return m_cache.IsVisible; }
        }

        /// <inheritdoc />
        public Size MaxClientSize
        {
            get { return m_cache.MaxClientSize; }
        }

        /// <inheritdoc />
        public INObject Parent
        {
            get { return m_parent; }
        }

        /// <inheritdoc />
        public Point Position
        {
            get { return m_cache.Position; }
        }

        /// <inheritdoc />
        public Size Size
        {
            get { return m_cache.Size; }
        }

        /// <inheritdoc />
        public string Title
        {
            get { return m_cache.Title; }
            set { SetWindowTitle(value); }
        }
        #endregion

        protected NWindow()
        {
            m_cache = new WindowStateDataCache();
            WireUpWindowEvents();
        }

        protected NWindow(INObject parent, NWindowDefinition definition) : this() 
        {
            m_cache.Definition = definition;
            m_parent = parent;
        }

        #region Methods
        /// <inheritdoc />
        public void Create(bool initialize = true)
        {
            if (!m_isCreated)
            {
                PlatformCreate();
            }

            if (!m_isInitialized && initialize)
                Initialize();

            m_isCreated = true;
        }

        /// <inheritdoc />
        public void Destroy()
        {
            if (m_isInitialized)
            {
                PlatformDestroy();
                m_isInitialized = false;
            }

            Dispose();
        }

        public void Initialize(NWindowDefinition definition)
        {
            m_cache.Definition = definition;
            m_isInitialized = false;

            Initialize();
        }

        /// <inheritdoc />
        public Point PointToClient(Point point)
        {
            return PlatformPointToClient(point);
        }

        /// <inheritdoc />
        public Point PointToScreen(Point point)
        {
            return PlatformPointToScreen(point);
        }

        public void SyncDataCache(bool force = false)
        {
            if (!force)
                if (m_cache.IsValid)
                    return;

            PlatformSyncDataCache();
            m_cache.Validate();
        }

        /// <inheritdoc />
        public void InvalidateDataCache()
        {
            m_cache.Invalidate();
        }

        protected virtual void OnActivated(object sender, EventArgs e) { }

        protected virtual void OnActivating(object sender, EventArgs e) { }

        protected virtual void OnClosed(object sender, EventArgs e) { }

        protected virtual void OnClosing(object sender, EventArgs e) { }

        protected virtual void OnCreated(object sender, EventArgs e){ }

        protected virtual void OnDeactivated(object sender, EventArgs e) { }

        protected virtual void OnDeactivating(object sender, EventArgs e) { }

        protected virtual void OnDestroyed(object sender, EventArgs e) { }

        protected virtual void OnFocusChanged(object sender, PropertyChangedEventArgs<bool> e) { }

        protected virtual void OnFocusChanging(object sender, PropertyChangingEventArgs<bool> e) { }

        protected virtual void OnFocusGained(object sender, EventArgs e) { }

        protected virtual void OnFocusLost(object sender, EventArgs e) { }

        protected virtual void OnEnabled(object sender, EventArgs e) { }

        protected virtual void OnDisabled(object sender, EventArgs e) { }

        protected virtual void OnPositionChanged(object sender, PropertyChangedEventArgs<Point> e){ }

        protected virtual void OnPositionChanging(object sender, PropertyChangingEventArgs<Point> e) { }

        protected virtual void OnSizeChanged(object sender, PropertyChangedEventArgs<Size> e) { }

        protected virtual void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e) { }

        protected virtual void OnTitleChanged(object sender, PropertyChangedEventArgs<string> e) { }

        protected override void DisposeManagedResources()
        {
            DisposeManagedResourcesShared();
            Closing.Dispose();
            Closed.Dispose();
            Created.Dispose();
            Destroyed.Dispose();
            Activating.Dispose();
            Activated.Dispose();
            Deactivating.Dispose();
            Deactivated.Dispose();
            PositionChanging.Dispose();
            PositionChanging.Dispose();
            SizeChanging.Dispose();
            SizeChanged.Dispose();
            FocusChanging.Dispose();
            FocusChanged.Dispose();
            FocusGained.Dispose();
            FocusLost.Dispose();
            Enabled.Dispose();
            Disabled.Dispose();
            TitleChanged.Dispose();
            m_handle = null;
            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            base.DisposeUnmanagedResources();
            Core.Window.ProcessMessage += OnProcessMessage;
            DisposeUnmanagedResourcesShared();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            InitializeResourcesShared();
            Core.ProcessMessage += OnProcessMessage;
            SyncDataCache(true);
        }

        protected abstract bool PlatformCheckActive();

        protected abstract bool PlatformCheckEnable();

        protected abstract bool PlatformCheckFocus();

        protected abstract bool PlatformCheckVisible();

        protected abstract void PlatformCreate();

        protected abstract void PlatformDestroy();

        protected abstract Rectangle PlatformGetClientArea();

        protected abstract Size PlatformGetClientSize();

        protected abstract Size PlatformGetMaxClientSize();

        protected abstract Point PlatformGetPosition();

        protected abstract string PlatformGetTitle();

        protected abstract Size PlatformGetWindowSize();

        protected abstract Point PlatformPointToClient(Point point);

        protected abstract Point PlatformPointToScreen(Point point);

        protected abstract void PlatformSetTitle(string value);

        protected abstract void PlatformSyncDataCache();

        protected void SetWindowTitle(string value)
        {
            if (!m_isInitialized)
                return;

            var title = PlatformGetTitle();
            if (value == title)
                return;

            PlatformSetTitle(value);
            var currTitle = PlatformGetTitle();
            TitleChanged.Raise(this, new PropertyChangedEventArgs<string>(currTitle, value, title));
            m_cache.Title = currTitle;
        }

        partial void DisposeManagedResourcesShared();

        partial void DisposeUnmanagedResourcesShared();

        partial void InitializeResourcesShared();

        private void WireUpWindowEvents()
        {
            WireUpWindowEventsShared();
            Closing += OnClosing;
            Closed += OnClosed;
            Created += OnCreated;
            Destroyed += OnDestroyed;
            Activating += OnActivating;
            Activated += OnActivated;
            Deactivating += OnDeactivating;
            Deactivated += OnDeactivated;
            PositionChanging += OnPositionChanging;
            PositionChanged += OnPositionChanged;
            SizeChanging += OnSizeChanging;
            SizeChanged += OnSizeChanged;
            TitleChanged += OnTitleChanged;
            FocusChanging += OnFocusChanging;
            FocusChanged += OnFocusChanged;
            FocusGained += OnFocusGained;
            Enabled += OnEnabled;
            Disabled += OnDisabled;
            FocusLost += OnFocusLost;
        }

        partial void WireUpWindowEventsShared();

        protected virtual void OnProcessMessage(IPlatformMessage message)
        {
            if (!m_isInitialized)
                return;

            switch (message.Id)
            {
                case MessageIds.Quit:
                    break;
                case MessageIds.Window:
                    var data = (WindowMessageData)message.RawData;
                    if (!Equals(this, data.Window))
                        return;

                    switch (data.MessageId)
                    {
                        case WindowMessageIds.None:
                            break;
                        case WindowMessageIds.Created:
                            Created.Raise(this, null);
                            break;
                        case WindowMessageIds.Destroyed:
                            Destroyed.Raise(this, null);
                            break;
                        case WindowMessageIds.Moving:
                            PositionChanging.Raise(this, data.PositionChangingData.ToEventArgs());
                            break;
                        case WindowMessageIds.Moved:
                            PositionChanged.Raise(this, data.PositionChangedData.ToEventArgs());
                            break;
                        case WindowMessageIds.Resizing:
                            SizeChanging.Raise(this, data.SizeChangingData.ToEventArgs());
                            break;
                        case WindowMessageIds.Resized:
                            SizeChanged.Raise(this, data.SizeChangedData.ToEventArgs());
                            break;
                        case WindowMessageIds.Focusing:
                            FocusChanging.Raise(this, new PropertyChangingEventArgs<bool>(false, true));
                            break;
                        case WindowMessageIds.Focused:
                            FocusGained.Raise(this, null);
                            FocusChanged.Raise(this, new PropertyChangedEventArgs<bool>(true, true, false));
                            break;
                        case WindowMessageIds.Unfocusing:
                            FocusChanging.Raise(this, new PropertyChangingEventArgs<bool>(true, false));
                            break;
                        case WindowMessageIds.Unfocused:                                
                            FocusLost.Raise(this, null);
                            FocusChanged.Raise(this, new PropertyChangedEventArgs<bool>(false, false, true));
                            break;
                        case WindowMessageIds.Closing:
                            Closing.Raise(this, null);
                            break;
                        case WindowMessageIds.Closed:
                            Closed.Raise(this, null);
                            break;
                        case WindowMessageIds.Activating:
                            Activating.Raise(this, null);
                            break;
                        case WindowMessageIds.Activated:
                            Activated.Raise(this, null);
                            break;
                        case WindowMessageIds.Deactivating:
                            Deactivating.Raise(this, null);
                            break;
                        case WindowMessageIds.Deactivated:
                            Deactivated.Raise(this, null);
                            break;
                        case WindowMessageIds.Enabled:
                            Enabled.Raise(this, null);
                            break;
                        case WindowMessageIds.Disabled:
                            Disabled.Raise(this, null);
                            break;
                    }
                    break;
            }

            OnProcessMessageShared(message);
        }

        partial void OnProcessMessageShared(IPlatformMessage message);
        #endregion

        /// <inheritdoc />
        public bool Equals(NWindow other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(m_handle, other.m_handle) && Equals(m_parent, other.m_parent);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is NWindow other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(m_handle, m_parent);
        }

        public static bool operator ==(NWindow left, NWindow right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NWindow left, NWindow right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public bool Equals(INWindow other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(m_handle, other.Handle) && Equals(m_parent, other.Parent);
        }

        public IWindowDataCache GetDataCache()
        {
            return m_cache;
        }
    }
}