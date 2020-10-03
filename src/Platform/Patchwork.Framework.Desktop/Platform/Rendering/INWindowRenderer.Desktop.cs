#region Usings
using System;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public partial interface INWindowRenderer
    {
        #region Events
        event EventHandler<PropertyChangedEventArgs<double>> DpiScalingChanged;
        event EventHandler<PropertyChangedEventArgs<NWindowMode>> ModeChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowMode>> ModeChanging;
        event EventHandler<PropertyChangedEventArgs<double>> OpacityChanged;
        event EventHandler<PropertyChangedEventArgs<NWindowState>> StateChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowState>> StateChanging;
        event EventHandler<PropertyChangedEventArgs<NWindowDecorations>> SupportedDecorationsChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowDecorations>> SupportedDecorationsChanging;
        event EventHandler<PropertyChangedEventArgs<NWindowTransparency>> TransparencySupportChanged;
        event EventHandler<PropertyChangingEventArgs<NWindowTransparency>> TransparencySupportChanging;
        #endregion

        #region Properties
        double Opacity { get; set; }

        //Thickness BorderThickness { get; }
        int TitlebarSize { get; }
        NWindowTransparency TransparencySupport { get; set; }
        #endregion

        #region Methods
        void EnableWindowSystemDecorations();

        void DisableWindowSystemDecorations();
        #endregion
    }
}