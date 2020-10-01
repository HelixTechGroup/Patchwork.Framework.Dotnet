using System;
using System.Collections.Generic;
using System.Drawing;
using Patchwork.Framework.Messaging;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform.Windowing
{
    public abstract partial class NWindow
    {
        protected bool m_isChildWindow;
        protected IList<INWindow> m_childWindows;
        protected bool m_isFirstTimeVisible;
        protected NWindowState m_initialState;

        public event EventHandler Hidden;
        public event EventHandler Shown;
        public event EventHandler<PropertyChangingEventArgs<NWindowState>> StateChanging;
        public event EventHandler<PropertyChangedEventArgs<NWindowState>> StateChanged;
        public event EventHandler<PropertyChangingEventArgs<NWindowMode>> ModeChanging;
        public event EventHandler<PropertyChangedEventArgs<NWindowMode>> ModeChanged;
        public event EventHandler AttentionDrawn;
        public event EventHandler ChildWindowCreated;
        public event EventHandler ChildWindowDestroyed;

        /// <inheritdoc />
        public NWindowState State
        {
            get { return m_cache.State; }
        }

        public NWindowMode Mode
        {
            get { return m_cache.Mode; }
        }

        public NWindowType Type
        {
            get { return m_cache.Definition.Type; }
        }

        /// <inheritdoc />
        public bool IsChildWindow
        {
            get { return m_isChildWindow; }
        }

        /// <inheritdoc />
        public bool IsTopmostWindow
        {
            get { return m_cache.IsTopmostWindow; }
        }        

        /// <inheritdoc />
        public bool IsVisibleInTaskbar
        {
            get { return m_cache.IsVisibleInTaskbar; }
            set { m_cache.IsVisibleInTaskbar = value; }
        }

        /// <inheritdoc />
        public IEnumerable<INWindow> ChildWindows
        {
            get { return m_childWindows; }
        }

        partial void InitializeResourcesShared()
        {
            m_childWindows = new ConcurrentList<INWindow>();
            m_initialState = m_cache.Definition.InitialState;
            m_isFirstTimeVisible = true;
        }

        partial void DisposeUnmanagedResourcesShared() { }

        protected abstract void PlatformShow();

        protected abstract void PlatformHide();        

        /// <inheritdoc />
        public void Show()
        {
            if (m_isInitialized && !m_cache.IsVisible)
                PlatformShow();
        }

        /// <inheritdoc />
        public void Hide()
        {
            if (m_isInitialized && m_cache.IsVisible)
                PlatformHide();
        }

        /// <inheritdoc />
        public void Move(Point position)
        {
            if (m_isInitialized && position != m_cache.Position)
                PlatformSetPosition(position);
        }

        protected abstract void PlatformSetPosition(Point position);

        /// <inheritdoc />
        public void Resize(Size size)
        {
            if (m_isInitialized && size == m_cache.Size)
                PlatformSetWindowSize(size);
        }

        protected abstract void PlatformSetWindowSize(Size size);

        /// <inheritdoc />
        public void Focus()
        {
            if (m_isInitialized && !m_cache.IsFocused)
                PlatformFocus();
        }

        /// <inheritdoc />
        public void Unfocus()
        {
            if (m_isInitialized && m_cache.IsFocused)
                PlatformUnfocus();
        }

        protected abstract void PlatformFocus();

        protected abstract void PlatformUnfocus();        

        /// <inheritdoc />
        public void Deactivate()
        {
            if (m_isInitialized && m_cache.IsActive)
                PlatformDeactivate();
        }

        protected abstract void PlatformDeactivate();

        /// <inheritdoc />
        public void Activate()
        {
            if (m_isInitialized && !m_cache.IsActive)
                PlatformActivate();
        }

        protected abstract void PlatformActivate();

        /// <inheritdoc />
        public void Restore()
        {
            if (m_isInitialized)
                PlatformRestore();
        }

        protected abstract void PlatformRestore();

        protected abstract Rectangle PlatformGetRestoreArea();

        /// <inheritdoc />
        public void Minimize()
        {
            if (!m_isInitialized)
                m_initialState = NWindowState.Minimized;

            if (m_cache.State != NWindowState.Minimized)
                PlatformMaximize();
        }

        protected abstract void PlatformMinimize();

        /// <inheritdoc />
        public void Maximize()
        {
            if (!m_isInitialized)
                m_initialState = NWindowState.Maximized;

            if (m_cache.State != NWindowState.Maximized)
                    PlatformMaximize();
        }

        protected abstract void PlatformMaximize();

        protected abstract NWindowState PlatformGetState();

        /// <inheritdoc />
        public void Enable()
        {
            if (m_isInitialized && !m_cache.IsEnabled)
                PlatformEnable();

        }

        protected abstract void PlatformEnable();

        /// <inheritdoc />
        public void Disable()
        {
            if (m_isInitialized && m_cache.IsEnabled)
                PlatformDisable();
        }

        protected abstract void PlatformDisable();

        /// <inheritdoc />
        public abstract void BringToFront(bool force);

        /// <inheritdoc />
        public abstract void ForceToFront();

        /// <inheritdoc />
        public abstract void CenterToScreen();

        /// <inheritdoc />
        public void DrawAttention()
        {
            if (m_isInitialized)
                PlatformDrawAttention();
        }

        protected abstract void PlatformDrawAttention();

        public bool IsPointInWindow(Point point)
        {
            return PlatformIsPointInWindow(point);
        }

        protected abstract bool PlatformIsPointInWindow(Point point);

        /// <inheritdoc />
        public INWindow CreateWindow()
        {
            var win = PlatformCreateChildWindow(NWindowDefinition.Default);
            win.Created += ChildWindowCreated;
            win.Destroyed += ChildWindowDestroyed;
            win.Create();
            return win;
        }

        protected abstract INWindow PlatformCreateChildWindow(NWindowDefinition definition);

        public INPopupWindow CreatePopup()
        {
            var win = PlatformCreatePopupWindow(NWindowDefinition.Default);
            win.Created += ChildWindowCreated;
            win.Destroyed += ChildWindowDestroyed;
            win.Create();
            return win;
        }

        /// <inheritdoc />
        public INWindow CreateWindow(NWindowDefinition definition)
        {
            var win = PlatformCreateChildWindow(definition);
            win.Created += ChildWindowCreated;
            win.Destroyed += ChildWindowDestroyed;
            win.Create();
            return win;
        }

        /// <inheritdoc />
        public INPopupWindow CreatePopup(NWindowDefinition definition)
        {
            var win = PlatformCreatePopupWindow(definition);
            win.Created += ChildWindowCreated;
            win.Destroyed += ChildWindowDestroyed;
            win.Create();
            return win;
        }

        protected abstract INPopupWindow PlatformCreatePopupWindow(NWindowDefinition definition);

        protected abstract NWindowMode PlatformGetMode();

        protected virtual void OnShown(object sender, EventArgs e) { }

        protected virtual void OnHidden(object sender, EventArgs e) { }                

        protected virtual void OnStateChanging(object sender, PropertyChangingEventArgs<NWindowState> e) { }

        protected virtual void OnStateChanged(object sender, PropertyChangedEventArgs<NWindowState> e) { }

        protected virtual void OnModeChanging(object sender, PropertyChangingEventArgs<NWindowMode> e) { }

        protected virtual void OnModeChanged(object sender, PropertyChangedEventArgs<NWindowMode> e) { }

        protected virtual void OnAttentionDrawn(object sender, EventArgs e) { }

        protected virtual void OnChildWindowCreated(object sender, EventArgs e)
        {
            var window = sender as INWindow;
            m_childWindows.Add(window);
        }

        protected virtual void OnChildWindowDestroyed(object sender, EventArgs e)
        {
            var window = sender as INWindow;
            m_childWindows.Remove(window);
        }

        partial void DisposeManagedResourcesShared()
        {
            foreach (var window in m_childWindows)
                window.Dispose();

            Shown.Dispose();
            Hidden.Dispose();            
            StateChanging.Dispose();
            StateChanged.Dispose();
            ModeChanging.Dispose();
            ModeChanged.Dispose();
            AttentionDrawn.Dispose();
            ChildWindowCreated.Dispose();
            ChildWindowDestroyed.Dispose();            
            base.DisposeManagedResources();
        }

        partial void WireUpWindowEventsShared()
        {
            StateChanging += OnStateChanging;
            StateChanged += OnStateChanged;
            ModeChanging += OnModeChanging;
            ModeChanged += OnModeChanged;
            AttentionDrawn += OnAttentionDrawn;
            Shown += OnShown;
            Hidden += OnHidden;
            ChildWindowCreated += OnChildWindowCreated;
            ChildWindowDestroyed += OnChildWindowDestroyed;            
        }

        partial void OnProcessMessageShared(IPlatformMessage message)
        {
            switch (message.Id)
            {
                case MessageIds.Window:
                    var data = message.RawData as WindowMessageData;
                    if (!Equals(this, data?.Window)) 
                        return;

                    switch (data?.MessageId)
                    {
                        case WindowMessageIds.Hidden:
                            Hidden.Raise(this, null);
                            break;
                        case WindowMessageIds.Maximizing:
                            StateChanging.Raise(this, data.StateChangingData.ToEventArgs());
                            break;
                        case WindowMessageIds.Maximized:
                            StateChanged.Raise(this, data.StateChangedData.ToEventArgs());
                            break;
                        case WindowMessageIds.Minimizing:
                            StateChanging.Raise(this, data.StateChangingData.ToEventArgs());
                            break;
                        case WindowMessageIds.Minimized:
                            StateChanged.Raise(this, data.StateChangedData.ToEventArgs());
                            break;
                        case WindowMessageIds.ModeChanging:
                            ModeChanging.Raise(this, data.ModeChangingData.ToEventArgs());
                            break;
                        case WindowMessageIds.ModeChanged:
                            ModeChanged.Raise(this, data.ModeChangedData.ToEventArgs());
                            break;
                        case WindowMessageIds.StateChanging:
                            StateChanging.Raise(this, data.StateChangingData.ToEventArgs());
                            break;
                        case WindowMessageIds.StateChanged:
                            StateChanged.Raise(this, data.StateChangedData.ToEventArgs());
                            break;
                        case WindowMessageIds.AttentionDrawn:
                            AttentionDrawn.Raise(this, null);
                            break;
                        case WindowMessageIds.Restored:
                            break;
                        case WindowMessageIds.Shown:
                            Shown.Raise(this, null);
                            break;
                    }
                    break;
            }
        }
    }
}
