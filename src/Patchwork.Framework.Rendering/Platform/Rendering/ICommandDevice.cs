using System.Collections.Generic;
using Patchwork.Framework.Platform.Rendering.Resources;

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INCommandDevice : INRenderDevice
    {
        IEnumerable<INRenderCommandList> CommandLists { get; }

        INRenderCommandList CreateCommandList(NRenderCommandType type);
    }

    public interface INRenderCommandQueueDevice : INRenderDevice
    {
        IEnumerable<INRenderCommandQueue> CommandQueues { get; }

        INRenderCommandQueue CreateCommandQueue(NRenderCommandType type);
    }
}