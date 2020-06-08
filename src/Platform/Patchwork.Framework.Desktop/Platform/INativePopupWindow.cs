using System;
using System.Drawing;
using Patchwork.Framework.Platform.Window;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Platform
{
    public interface INativePopupWindow : INativeWindow
    {
        event EventHandler<PropertyChangingEventArgs<Point>> PositionChanging;
        event EventHandler<PropertyChangedEventArgs<Point>> PositionChanged;
        event EventHandler<PropertyChangingEventArgs<NativeWindowState>> StateChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowState>> StateChanged;
        event EventHandler AttentionDrawn;

        NativeWindowState State { get; }
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
