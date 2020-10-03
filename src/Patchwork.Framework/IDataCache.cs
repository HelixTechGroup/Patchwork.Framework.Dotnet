namespace Patchwork.Framework
{
    public interface IDataCache
    {
        #region Properties
        bool IsValid { get; }
        #endregion

        #region Methods
        void Invalidate();

        void Reset();

        void Validate();
        #endregion
    }
}