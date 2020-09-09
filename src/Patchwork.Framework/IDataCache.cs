namespace Patchwork.Framework.Platform.Window
{
    public interface IDataCache
    {
        bool IsValid { get; }

        void Invalidate();
        void Reset();
        void Validate();
    }
}