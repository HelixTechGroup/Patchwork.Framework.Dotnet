using System.Collections.Generic;
using Patchwork.Framework.Platform.Rendering.Resources;
using Vortice.Direct3D12;

namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources.Collections
{
    internal class Dx12GraphicsCommandListSet : Dx12CommandListSet<ID3D12GraphicsCommandList>
    {
        /// <inheritdoc />
        internal Dx12GraphicsCommandListSet(INRenderCommandQueue queue, IEnumerable<ID3D12GraphicsCommandList> items) : base(queue, items) { }

        //internal Dx12GraphicsCommandListSet() { }

        internal Dx12GraphicsCommandListSet(INRenderCommandQueue queue) : base(queue)
        {
        }
    }
}