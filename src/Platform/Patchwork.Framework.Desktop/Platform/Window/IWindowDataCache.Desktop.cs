namespace Patchwork.Framework.Platform.Window
{
    public partial interface IWindowDataCache
    {
        NWindowState State { get; set; }
        NWindowState PreviousState { get; }
        bool IsVisibleInTaskbar { get; set; }
        bool IsTopmostWindow { get; set; }
        NWindowMode Mode { get; set; }
        NWindowMode PreviousMode { get; }
    }
}