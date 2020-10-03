namespace Patchwork.Framework.Platform.Rendering
{
    public interface INRenderFence : INResource
    {
        #region Properties
        bool Signaled { get; }
        #endregion

        #region Methods
        void Reset();
        #endregion
    }
}