#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using Shin.Framework.ComponentModel;
#endregion

namespace Patchwork.Framework.Platform.Window
{
    public partial interface INativeWindow
    {
        #region Events
        event EventHandler Hidden;
        event EventHandler Shown;
        event EventHandler<PropertyChangingEventArgs<NativeWindowState>> StateChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowState>> StateChanged;
        event EventHandler<PropertyChangingEventArgs<NativeWindowMode>> ModeChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowMode>> ModeChanged;
        event EventHandler AttentionDrawn;
        event EventHandler ChildWindowCreated;
        event EventHandler ChildWindowDestroyed;
        #endregion

        #region Properties
        IEnumerable<INativeWindow> ChildWindows { get; }
        bool IsChildWindow { get; }        
        bool IsTopmostWindow { get; }
        bool IsVisibleInTaskbar { get; set; }        
        NativeWindowState State { get; }
        NativeWindowMode Mode { get; }
        NativeWindowType Type { get; }
        #endregion

        #region Methods
        void Show();
        void Hide();
        void Move(Point position);
        void Resize(Size size);
        void Focus();
        void Unfocus();
        void Activate();
        void Deactivate();
        void Restore();
        void Minimize();
        void Maximize();
        void Enable();
        void Disable();
        void BringToFront(bool force);
        void ForceToFront();
        void CenterToScreen();
        void DrawAttention();
        INativeWindow CreateWindow();
        INativePopupWindow CreatePopup();
        INativeWindow CreateWindow(NativeWindowDefinition definition);
        INativePopupWindow CreatePopup(NativeWindowDefinition definition);
        bool IsPointInWindow(Point point);
        #endregion
    }
}