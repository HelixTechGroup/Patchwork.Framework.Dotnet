#region Usings
using System;
using System.Drawing;
using Shin.Framework.ComponentModel;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public interface INPopupWindow : INWindow
    {
        #region Events
        event EventHandler AttentionDrawn;
        event EventHandler<PropertyChangedEventArgs<Point>> PositionChanged;
        event EventHandler<PropertyChangingEventArgs<Point>> PositionChanging;
        event EventHandler<PropertyChangedEventArgs<NWindowState>> StateChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowState>> StateChanging;
        #endregion

        #region Properties
        bool IsEnabled { get; }
        Point Position { get; }

        NWindowState State { get; }
        Size WindowSize { get; }
        #endregion

        #region Methods
        void Move(Point position);

        void Restore();

        void Minimize();

        void Maximize();

        void Enable();

        void Disable();

        void CenterToScreen();

        void DrawAttention();
        #endregion
    }
}