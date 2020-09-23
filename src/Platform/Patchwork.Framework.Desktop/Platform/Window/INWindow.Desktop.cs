#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using Shin.Framework.ComponentModel;
#endregion

namespace Patchwork.Framework.Platform.Window
{
    public partial interface INWindow
    {
        #region Events
        event EventHandler Hidden;
        event EventHandler Shown;
        event EventHandler<PropertyChangingEventArgs<NWindowState>> StateChanging;
        event EventHandler<PropertyChangedEventArgs<NWindowState>> StateChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowMode>> ModeChanging;
        event EventHandler<PropertyChangedEventArgs<NWindowMode>> ModeChanged;
        event EventHandler AttentionDrawn;
        event EventHandler ChildWindowCreated;
        event EventHandler ChildWindowDestroyed;
        #endregion

        #region Properties
        IEnumerable<INWindow> ChildWindows { get; }
        bool IsChildWindow { get; }        
        bool IsTopmostWindow { get; }
        bool IsVisibleInTaskbar { get; set; }        
        NWindowState State { get; }
        NWindowMode Mode { get; }
        NWindowType Type { get; }
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
        INWindow CreateWindow();
        INPopupWindow CreatePopup();
        INWindow CreateWindow(NWindowDefinition definition);
        INPopupWindow CreatePopup(NWindowDefinition definition);
        bool IsPointInWindow(Point point);
        #endregion
    }
}