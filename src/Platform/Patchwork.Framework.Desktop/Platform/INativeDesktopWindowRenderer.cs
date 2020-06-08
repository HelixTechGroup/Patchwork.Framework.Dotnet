using System;
using Patchwork.Framework.Platform.Window;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Platform
{
    public interface INativeDesktopWindowRenderer : INativeWindowRenderer
    {
        event EventHandler<PropertyChangingEventArgs<NativeWindowTransparency>> TransparencySupportChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowTransparency>> TransparencySupportChanged;
        event EventHandler<PropertyChangingEventArgs<NativeWindowDecorations>> SupportedDecorationsChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowDecorations>> SupportedDecorationsChanged;
        event EventHandler<PropertyChangingEventArgs<NativeWindowMode>> ModeChanging;
        event EventHandler<PropertyChangedEventArgs<NativeWindowMode>> ModeChanged;
        event EventHandler<PropertyChangedEventArgs<double>> DpiScalingChanged;
        event EventHandler<PropertyChangedEventArgs<double>> OpacityChanged;

        double DpiScaling { get; }
        NativeWindowTransparency TransparencySupport { get; set; }
        NativeWindowDecorations SupportedDecorations { get; set; }
        NativeWindowMode Mode { get; }

        //Thickness BorderThickness { get; }
        int TitlebarSize { get; }
        double Opacity { get; set; }

        void EnableWindowDecorations();
        void DisableWindowDecorations();
    }
}
