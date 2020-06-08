using System;
using System.Collections.Generic;
using System.Drawing;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform
{
    public abstract class NativeDesktopWindow : NativeWindow, INativeDesktopWindow
    {
        protected NativeWindowActivationPolicy m_activationPolicy;
        protected NativeWindowState m_state;
        protected NativeWindowState m_previousState;
        protected bool m_isChildWindow;
        protected bool m_isTopmostWindow;
        protected bool m_isEnabled;
        protected bool m_isResizable;
        protected bool m_isVisibleInTaskbar;
        protected IList<INativeDesktopWindow> m_childWindows;
        protected NativeDesktopWindowDefinition m_definition;
        protected bool m_isFocused;
        protected NativeWindowState m_initialState;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<Point>> PositionChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<Point>> PositionChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<Size>> SizeChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<Size>> SizeChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<bool>> FocusChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<bool>> FocusChanged;

        /// <inheritdoc />
        public event EventHandler FocusGained;

        /// <inheritdoc />
        public event EventHandler FocusLost;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<NativeWindowState>> StateChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<NativeWindowState>> StateChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<NativeWindowMode>> ModeChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<NativeWindowMode>> ModeChanged;

        /// <inheritdoc />
        public event EventHandler AttentionDrawn;

        /// <inheritdoc />
        public event EventHandler ChildWindowCreated;

        /// <inheritdoc />
        public event EventHandler ChildWindowDestroyed;

        /// <inheritdoc />
        public NativeWindowActivationPolicy ActivationPolicy
        {
            get { return m_activationPolicy; }
        }

        /// <inheritdoc />
        public NativeWindowState State
        {
            get { return m_state; }
        }

        /// <inheritdoc />
        public bool IsChildWindow
        {
            get { return m_isChildWindow; }
        }

        /// <inheritdoc />
        public bool IsFocused
        {
            get { return m_isFocused; }
        }

        /// <inheritdoc />
        public bool IsTopmostWindow
        {
            get { return m_isTopmostWindow; }
        }

        /// <inheritdoc />
        public bool IsEnabled
        {
            get { return m_isEnabled; }
        }

        /// <inheritdoc />
        public bool IsResizable
        {
            get { return m_isResizable; }
            set { m_isResizable = value; }
        }

        /// <inheritdoc />
        public bool IsVisibleInTaskbar
        {
            get { return m_isVisibleInTaskbar; }
            set { m_isVisibleInTaskbar = value; }
        }

        /// <inheritdoc />
        public Point Position
        {
            get { return !m_isInitialized ? Point.Empty : PlatformGetPosition(); }
        }

        protected abstract Point PlatformGetPosition();

        /// <inheritdoc />
        public Size WindowSize
        {
            get { return !m_isInitialized ? Size.Empty : PlatformGetWindowSize(); }
        }

        protected abstract Size PlatformGetWindowSize();

        /// <inheritdoc />
        public IEnumerable<INativeDesktopWindow> ChildWindows
        {
            get { return m_childWindows; }
        }

        protected NativeDesktopWindow(NativeDesktopWindowDefinition definition, INativeObject parent)
        {
            m_definition = definition;
            m_parent = parent;
            m_childWindows = new ConcurrentList<INativeDesktopWindow>();
            WireUpDesktopWindowEvents();
        }

        /// <inheritdoc />
        public void Move(Point position)
        {
            if (!m_isInitialized) 
                return;
            var currentPosition = PlatformGetPosition();
            if (position == currentPosition)
                return;
            PositionChanging.Raise(this, new PropertyChangingEventArgs<Point>(currentPosition, position));
            PlatformSetPosition(position);
            PositionChanged.Raise(this, new PropertyChangedEventArgs<Point>(position, position, currentPosition));
        }

        protected abstract void PlatformSetPosition(Point position);

        /// <inheritdoc />
        public void Resize(Size size)
        {
            if (!m_isInitialized) 
                return;
            var currentSize = PlatformGetClientSize();
            if (size == currentSize)
                return;
            SizeChanging.Raise(this, new PropertyChangingEventArgs<Size>(currentSize, size));
            PlatformSetWindowSize(size);
            SizeChanged.Raise(this, new PropertyChangedEventArgs<Size>(size, size, currentSize));
        }

        protected abstract void PlatformSetWindowSize(Size size);

        /// <inheritdoc />
        public void Focus()
        {
            if (!m_isInitialized)
                return;
            if (m_isFocused)
                return;
            FocusChanging.Raise(this, new PropertyChangingEventArgs<bool>(!m_isFocused, m_isFocused));
            PlatformFocus();
            var focused = PlatformCheckFocus();
            FocusChanged.Raise(this, new PropertyChangedEventArgs<bool>(focused, !m_isFocused, m_isFocused));
            m_isFocused = focused;
            if (m_isFocused)
                FocusGained.Raise(this, null);
        }

        /// <inheritdoc />
        public void Unfocus()
        {
            if (!m_isInitialized)
                return;
            if (!m_isFocused)
                return;
            FocusChanging.Raise(this, new PropertyChangingEventArgs<bool>(m_isFocused, !m_isFocused));
            PlatformUnfocus();
            var focused = PlatformCheckFocus();
            FocusChanged.Raise(this, new PropertyChangedEventArgs<bool>(focused, m_isFocused, !m_isFocused));
            m_isFocused = focused;
            if (!m_isFocused)
                FocusLost.Raise(this, null);
        }

        protected abstract void PlatformFocus();

        protected abstract void PlatformUnfocus();

        protected abstract bool PlatformCheckFocus();

        /// <inheritdoc />
        public void Restore()
        {
            if (!m_isInitialized) 
                return;
            StateChanging.Raise(this, new PropertyChangingEventArgs<NativeWindowState>(m_state, m_previousState));
            PlatformRestore();
            var state = PlatformGetState();
            StateChanged.Raise(this, new PropertyChangedEventArgs<NativeWindowState>(state, m_previousState, m_state));
            m_previousState = m_state;
            m_state = state;
        }

        protected abstract void PlatformRestore();

        protected abstract Rectangle PlatformGetRestoreArea();

        /// <inheritdoc />
        public void Minimize()
        {
            if (m_isInitialized)
            {
                if (m_state == NativeWindowState.Minimized)
                    return;

                StateChanging.Raise(this, new PropertyChangingEventArgs<NativeWindowState>(m_state, NativeWindowState.Minimized));
                PlatformMinimize();
                var state = PlatformGetState();
                StateChanged.Raise(this, new PropertyChangedEventArgs<NativeWindowState>(state, 
                                                                                         NativeWindowState.Minimized, 
                                                                                         m_state));
                m_previousState = m_state;
                m_state = state;
                m_isVisible = PlatformCheckVisible();
            }
            else
                m_initialState = NativeWindowState.Minimized;
        }

        protected abstract void PlatformMinimize();

        /// <inheritdoc />
        public void Maximize()
        {
            if (m_isInitialized)
            {
                if (m_state == NativeWindowState.Maximized)
                    return;

                StateChanging.Raise(this, new PropertyChangingEventArgs<NativeWindowState>(m_state, NativeWindowState.Maximized));
                PlatformMaximize();
                var state = PlatformGetState();
                StateChanged.Raise(this,
                                   new PropertyChangedEventArgs<NativeWindowState>(state,
                                                                                   NativeWindowState.Maximized,
                                                                                   m_state));
                m_previousState = m_state;
                m_state = state;
                m_isVisible = PlatformCheckVisible();
            }
            else
                m_initialState = NativeWindowState.Minimized;
        }

        protected abstract void PlatformMaximize();

        protected abstract NativeWindowState PlatformGetState();

        /// <inheritdoc />
        public void Enable()
        {
            if (m_isInitialized && !m_isEnabled)
            {
                if (!PlatformCheckEnable())
                    PlatformEnable();
            }
            m_isEnabled = true;
        }

        protected abstract void PlatformEnable();

        protected abstract bool PlatformCheckEnable();

        /// <inheritdoc />
        public void Disable()
        {
            if (m_isInitialized && m_isEnabled)
            {
                if (PlatformCheckEnable())
                    PlatformDisable();
            }

            m_isEnabled = PlatformCheckEnable();
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
            if (!m_isInitialized)
                return;
            PlatformDrawAttention();
            AttentionDrawn.Raise(this, EventArgs.Empty);
        }

        protected abstract void PlatformDrawAttention();

        /// <inheritdoc />
        public INativeDesktopWindow CreateWindow()
        {
            var win = PlatformCreateChildWindow(NativeDesktopWindowDefinition.Default);
            win.Created += ChildWindowCreated;
            win.Destroyed += ChildWindowDestroyed;
            win.Create();
            return win;
        }

        protected abstract INativeDesktopWindow PlatformCreateChildWindow(NativeDesktopWindowDefinition definition);

        public INativePopupWindow CreatePopup()
        {
            var win = PlatformCreatePopupWindow(NativeDesktopWindowDefinition.Default);
            win.Created += ChildWindowCreated;
            win.Destroyed += ChildWindowDestroyed;
            win.Create();
            return win;
        }

        /// <inheritdoc />
        public INativeDesktopWindow CreateWindow(NativeDesktopWindowDefinition definition)
        {
            var win = PlatformCreateChildWindow(definition);
            win.Created += ChildWindowCreated;
            win.Destroyed += ChildWindowDestroyed;
            win.Create();
            return win;
        }

        /// <inheritdoc />
        public INativePopupWindow CreatePopup(NativeDesktopWindowDefinition definition)
        {
            var win = PlatformCreatePopupWindow(definition);
            win.Created += ChildWindowCreated;
            win.Destroyed += ChildWindowDestroyed;
            win.Create();
            return win;
        }

        /// <inheritdoc />
        public void Initialize(NativeDesktopWindowDefinition definition)
        {
            m_definition = definition;
            if (m_isInitialized)
                m_isInitialized = false;

            Initialize();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_title = m_definition.Title;
            m_activationPolicy = m_definition.ActivationPolicy;
            m_type = m_definition.Type;
            m_isVisibleInTaskbar = m_definition.IsVisibleInTaskbar;
            m_isTopmostWindow = m_definition.IsTopmostWindow;
            m_initialState = m_definition.InitialState;
        }

        protected abstract INativePopupWindow PlatformCreatePopupWindow(NativeDesktopWindowDefinition definition);

        protected virtual void OnPositionChanging(object sender, PropertyChangingEventArgs<Point> e)
        {
            PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Moving, this));
        }

        protected virtual void OnPositionChanged(object sender, PropertyChangedEventArgs<Point> e)
        {
            PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Moved, this));
        }

        protected virtual void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e)
        {
            //PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Resizing, this));
        }

        protected virtual void OnSizeChanged(object sender, PropertyChangedEventArgs<Size> e)
        {
            //PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Resized, this));
        }

        protected virtual void OnFocusChanging(object sender, PropertyChangingEventArgs<bool> e)
        {

        }

        protected virtual void OnFocusChanged(object sender, PropertyChangedEventArgs<bool> e) 
        {
            
        }

        protected virtual void OnFocusGained(object sender, EventArgs e) 
        {
            //PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Focused, this));
        }

        protected virtual void OnFocusLost(object sender, EventArgs e)
        {
            //PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Unfocused, this));
        }

        protected virtual void OnStateChanging(object sender, PropertyChangingEventArgs<NativeWindowState> e) { }

        protected virtual void OnStateChanged(object sender, PropertyChangedEventArgs<NativeWindowState> e) { }

        protected virtual void OnModeChanging(object sender, PropertyChangingEventArgs<NativeWindowMode> e) { }

        protected virtual void OnModeChanged(object sender, PropertyChangedEventArgs<NativeWindowMode> e) { }

        protected virtual void OnAttentionDrawn(object sender, EventArgs e)
        {
            PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.AttentionDrawn, this));
        }

        protected virtual void OnChildWindowCreated(object sender, EventArgs e)
        {
            var window = sender as INativeDesktopWindow;
            m_childWindows.Add(window);
        }

        protected virtual void OnChildWindowDestroyed(object sender, EventArgs e)
        {
            var window = sender as INativeDesktopWindow;
            m_childWindows.Remove(window);
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            foreach (var window in m_childWindows)
                window.Dispose();

            PositionChanging.Dispose();
            PositionChanging.Dispose();
            SizeChanging.Dispose();
            SizeChanged.Dispose();
            FocusChanging.Dispose();
            FocusChanged.Dispose();
            FocusGained.Dispose();
            FocusLost.Dispose();
            StateChanging.Dispose();
            StateChanged.Dispose();
            ModeChanging.Dispose();
            ModeChanged.Dispose();
            AttentionDrawn.Dispose();
            ChildWindowCreated.Dispose();
            ChildWindowDestroyed.Dispose();
            base.DisposeManagedResources();
        }

        private void WireUpDesktopWindowEvents()
        {
            PositionChanging += OnPositionChanging;
            PositionChanged += OnPositionChanged;
            SizeChanging += OnSizeChanging;
            SizeChanged += OnSizeChanged;
            FocusChanging += OnFocusChanging;
            FocusChanged += OnFocusChanged;
            FocusGained += OnFocusGained;
            FocusLost += OnFocusLost;
            StateChanging += OnStateChanging;
            StateChanged += OnStateChanged;
            ModeChanging += OnModeChanging;
            ModeChanged += OnModeChanged;
            AttentionDrawn += OnAttentionDrawn;
            ChildWindowCreated += OnChildWindowCreated;
            ChildWindowDestroyed += OnChildWindowDestroyed;
        }
    }
}
