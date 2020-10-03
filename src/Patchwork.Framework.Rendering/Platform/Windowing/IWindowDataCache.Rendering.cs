namespace Patchwork.Framework.Platform.Windowing
{
    public partial interface IWindowDataCache : IDataCache
    {
        #region Properties
        bool IsRenderable { get; set; }

        bool PreviouslyRenderable { get; }
        #endregion
    }
}