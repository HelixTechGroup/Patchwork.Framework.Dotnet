#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using Shin.Framework.ComponentModel;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public partial interface INWindow
    {
        #region Events
        event EventHandler AttentionDrawn;
        event EventHandler ChildWindowCreated;
        event EventHandler ChildWindowDestroyed;
        event EventHandler Hidden;
        event EventHandler<PropertyChangedEventArgs<NWindowMode>> ModeChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowMode>> ModeChanging;
        event EventHandler Shown;
        event EventHandler<PropertyChangedEventArgs<NWindowState>> StateChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowState>> StateChanging;
        #endregion

        #region Properties
        IEnumerable<INWindow> ChildWindows { get; }
        bool IsChildWindow { get; }
        bool IsTopmostWindow { get; }
        bool IsVisibleInTaskbar { get; set; }
        NWindowMode Mode { get; }
        NWindowState State { get; }
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