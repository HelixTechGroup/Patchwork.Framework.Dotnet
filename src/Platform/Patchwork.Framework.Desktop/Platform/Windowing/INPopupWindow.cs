using System;
using System.Drawing;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Platform.Windowing
{
    public interface INPopupWindow : INWindow
    {
        event EventHandler<PropertyChangingEventArgs<Point>> PositionChanging;
        event EventHandler<PropertyChangedEventArgs<Point>> PositionChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowState>> StateChanging;
        event EventHandler<PropertyChangedEventArgs<NWindowState>> StateChanged;
        event EventHandler AttentionDrawn;

        NWindowState State { get; }
        Point Position { get; }
        Size WindowSize { get; }
        bool IsEnabled { get; }

        void Move(Point position);
        void Restore();
        void Minimize();
        void Maximize();
        void Enable();
        void Disable();
        void CenterToScreen();
        void DrawAttention();
    }
}
