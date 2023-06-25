using Patchwork.Framework.Platform.Rendering.Resources;
using VD2D1 = Vortice.Direct2D1;
using VD3D = Vortice.Direct3D;
using VD3D11 = Vortice.Direct3D11;
using VD3D11on12 = Vortice.Direct3D11on12;
using VD3D12 = Vortice.Direct3D12;
using VDXGI = Vortice.DXGI;

namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    internal interface IDx12GraphicsCommandList : INRenderCommandList<VD3D12.ID3D12GraphicsCommandList> { }
}