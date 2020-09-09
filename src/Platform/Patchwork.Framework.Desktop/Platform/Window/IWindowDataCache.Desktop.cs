namespace Patchwork.Framework.Platform.Window
{
    public partial interface IWindowDataCache
    {
        NativeWindowState State { get; set; }
        NativeWindowState PreviousState { get; }
        bool IsVisibleInTaskbar { get; set; }
        bool IsTopmostWindow { get; set; }
        NativeWindowMode Mode { get; set; }
        NativeWindowMode PreviousMode { get; }
    }
}