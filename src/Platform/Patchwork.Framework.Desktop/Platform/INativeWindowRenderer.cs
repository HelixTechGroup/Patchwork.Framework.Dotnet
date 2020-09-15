using System;
using Patchwork.Framework.Platform.Window;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Platform
{
    public partial interface INativeWindowRenderer : INativeRenderer
    {
        event EventHandler<PropertyChangingEventArgs<NativeWindowTransparency>> TransparencySupportChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowTransparency>> TransparencySupportChanged;
        event EventHandler<PropertyChangingEventArgs<NativeWindowDecorations>> SupportedDecorationsChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowDecorations>> SupportedDecorationsChanged;
        event EventHandler<PropertyChangingEventArgs<NativeWindowState>> StateChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowState>> StateChanged;
        event EventHandler<PropertyChangingEventArgs<NativeWindowMode>> ModeChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowMode>> ModeChanged;
        event EventHandler<PropertyChangedEventArgs<double>> DpiScalingChanged;
        event EventHandler<PropertyChangedEventArgs<double>> OpacityChanged;

        double DpiScaling { get; }
        NativeWindowTransparency TransparencySupport { get; set; }
        NativeWindowDecorations SupportedDecorations { get; set; }

        //Thickness BorderThickness { get; }
        int TitlebarSize { get; }
        double Opacity { get; set; }
        float AspectRatio { get; }

        void EnableWindowSystemDecorations();
        void DisableWindowSystemDecorations();
    }
}
