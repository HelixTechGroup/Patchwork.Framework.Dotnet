using Patchwork.Framework.Platform.Rendering.Runtime;
using Patchwork.Framework.Platform.Runtime;

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INRenderFence : INRenderMemoryBarrier
    {
        #region Properties
        bool WasSignaled { get; }
        int CurrentValue { get; }
        int CompletedValue { get; }
        #endregion

        #region Methods
        //void Reset();
        void Wait();
        void Signal();
        #endregion
    }
}