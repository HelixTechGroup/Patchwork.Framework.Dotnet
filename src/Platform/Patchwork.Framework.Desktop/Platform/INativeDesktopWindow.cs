#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using Patchwork.Framework.Platform.Window;
using Shin.Framework.ComponentModel;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface INativeDesktopWindow : INativeWindow
    {
        #region Events
        event EventHandler<PropertyChangingEventArgs<Point>> PositionChanging;
        event EventHandler<PropertyChangedEventArgs<Point>> PositionChanged;
        event EventHandler<PropertyChangingEventArgs<Size>> SizeChanging;
        event EventHandler<PropertyChangedEventArgs<Size>> SizeChanged;
        event EventHandler<PropertyChangingEventArgs<bool>> FocusChanging;
        event EventHandler<PropertyChangedEventArgs<bool>> FocusChanged;
        event EventHandler FocusGained;
        event EventHandler FocusLost;
        event EventHandler<PropertyChangingEventArgs<NativeWindowState>> StateChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowState>> StateChanged;
        event EventHandler AttentionDrawn;
        event EventHandler ChildWindowCreated;
        event EventHandler ChildWindowDestroyed;
        #endregion

        #region Properties
        NativeWindowActivationPolicy ActivationPolicy { get; }
        IEnumerable<INativeDesktopWindow> ChildWindows { get; }
        bool IsChildWindow { get; }
        bool IsEnabled { get; }
        bool IsFocused { get; }
        bool IsResizable { get; set; }
        bool IsTopmostWindow { get; }
        bool IsVisibleInTaskbar { get; set; }
        Point Position { get; }
        NativeWindowState State { get; }
        Size WindowSize { get; }
        #endregion

        #region Methods
        void Move(Point position);

        void Resize(Size size);

        void Focus();

        void Unfocus();

        void Restore();

        void Minimize();

        void Maximize();

        void Enable();

        void Disable();

        void BringToFront(bool force);

        void ForceToFront();

        void CenterToScreen();

        void DrawAttention();

        INativeDesktopWindow CreateWindow();

        INativePopupWindow CreatePopup();

        INativeDesktopWindow CreateWindow(NativeDesktopWindowDefinition definition);

        INativePopupWindow CreatePopup(NativeDesktopWindowDefinition definition);

        void Initialize(NativeDesktopWindowDefinition definition);
        #endregion
    }
}