namespace Patchwork.Framework.Platform.Windowing
{
    public partial interface IWindowDataCache
    {
        #region Properties
        bool IsTopmostWindow { get; set; }
        bool IsVisibleInTaskbar { get; set; }
        NWindowMode Mode { get; set; }
        NWindowMode PreviousMode { get; }
        NWindowState PreviousState { get; }
        NWindowState State { get; set; }
        #endregion
    }
}