namespace Patchwork.Framework
{
    public interface IDataCache
    {
        bool IsValid { get; }

        void Invalidate();
        void Reset();
        void Validate();
    }
}