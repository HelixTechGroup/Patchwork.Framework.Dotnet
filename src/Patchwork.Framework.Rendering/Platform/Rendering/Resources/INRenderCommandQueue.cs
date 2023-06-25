using System.Collections.Generic;

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public interface INRenderCommandQueue : INRenderResource
    {
        #region Properties
        IEnumerable<INRenderCommandList> CommandLists { get; }
        NRenderCommandType Type { get; }
        #endregion

        #region Methods
        INRenderCommandList CreateCommandList();

        void Begin();

        void End();

        void Reset();

        void Execute(params INRenderCommandList[] commandLists);

        void Execute(IEnumerable<INRenderCommandList> commandLists);

        bool Wait(INRenderFence fence);

        bool Signal(INRenderFence fence);
        #endregion
    }
}