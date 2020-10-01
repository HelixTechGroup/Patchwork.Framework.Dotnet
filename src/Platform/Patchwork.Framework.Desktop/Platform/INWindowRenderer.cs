using System;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Platform
{
    public partial interface INWindowRenderer : INRenderer
    {
        event EventHandler<PropertyChangingEventArgs<NWindowTransparency>> TransparencySupportChanging;
        event EventHandler<PropertyChangedEventArgs<NWindowTransparency>> TransparencySupportChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowDecorations>> SupportedDecorationsChanging;
        event EventHandler<PropertyChangedEventArgs<NWindowDecorations>> SupportedDecorationsChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowState>> StateChanging;
        event EventHandler<PropertyChangedEventArgs<NWindowState>> StateChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowMode>> ModeChanging;
        event EventHandler<PropertyChangedEventArgs<NWindowMode>> ModeChanged;
        event EventHandler<PropertyChangedEventArgs<double>> DpiScalingChanged;
        event EventHandler<PropertyChangedEventArgs<double>> OpacityChanged;

        double DpiScaling { get; }
        NWindowTransparency TransparencySupport { get; set; }
        NWindowDecorations SupportedDecorations { get; set; }

        //Thickness BorderThickness { get; }
        int TitlebarSize { get; }
        double Opacity { get; set; }
        float AspectRatio { get; }

        void EnableWindowSystemDecorations();
        void DisableWindowSystemDecorations();
    }
}
