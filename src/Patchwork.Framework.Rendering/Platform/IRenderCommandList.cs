#region Usings
using System;
using Patchwork.Framework.Platform;
#endregion

namespace Patchwork.Framework.Rendering
{
    public interface IRenderCommandList : INResource
    {
        #region Methods
        void Begin();

        void End();


        //void SetPipeline(Pipeline pipeline);


        //void SetVertexBuffer(uint index, DeviceBuffer buffer);


        //void SetVertexBuffer(uint index, DeviceBuffer buffer, uint offset);


        //void SetIndexBuffer(DeviceBuffer buffer, IndexFormat format);


        //void SetIndexBuffer(DeviceBuffer buffer, IndexFormat format, uint offset);


        //void SetGraphicsResourceSet(uint slot, ResourceSet rs);


        //void SetGraphicsResourceSet(uint slot, ResourceSet rs, uint[] dynamicOffsets);


        //void SetGraphicsResourceSet(uint slot, ResourceSet rs, uint dynamicOffsetsCount, ref uint dynamicOffsets);


        //void SetComputeResourceSet(uint slot, ResourceSet rs);


        //void SetComputeResourceSet(uint slot, ResourceSet rs, uint[] dynamicOffsets);


        //void SetComputeResourceSet(uint slot, ResourceSet rs, uint dynamicOffsetsCount, ref uint dynamicOffsets);


        //void SetFramebuffer(Framebuffer fb);


        //void ClearColorTarget(uint index, RgbaFloat clearColor);


        //void ClearDepthStencil(float depth);


        //void ClearDepthStencil(float depth, byte stencil);


        //void SetFullViewports();


        //void SetFullViewport(uint index);


        //void SetViewport(uint index, Viewport viewport);


        //void SetViewport(uint index, ref Viewport viewport);


        //void SetFullScissorRects();


        //void SetFullScissorRect(uint index);


        //void SetScissorRect(uint index, uint x, uint y, uint width, uint height);


        //void Draw(uint vertexCount);


        //void Draw(uint vertexCount, uint instanceCount, uint vertexStart, uint instanceStart);


        //void DrawIndexed(uint indexCount);


        //void DrawIndexed(uint indexCount, uint instanceCount, uint indexStart, int vertexOffset, uint instanceStart);


        //void DrawIndirect(DeviceBuffer indirectBuffer, uint offset, uint drawCount, uint stride);


        //void DrawIndexedIndirect(DeviceBuffer indirectBuffer, uint offset, uint drawCount, uint stride);


        //void Dispatch(uint groupCountX, uint groupCountY, uint groupCountZ);


        //void DispatchIndirect(DeviceBuffer indirectBuffer, uint offset);


        //void ResolveTexture(Texture source, Texture destination);


        //void UpdateBuffer<T>(
        //    DeviceBuffer buffer,
        //    uint bufferOffsetInBytes,
        //    T source) where T : struct;


        //void UpdateBuffer<T>(
        //    DeviceBuffer buffer,
        //    uint bufferOffsetInBytes,
        //    ref T source) where T : struct;


        //void UpdateBuffer<T>(
        //    DeviceBuffer buffer,
        //    uint bufferOffsetInBytes,
        //    ref T source,
        //    uint sizeInBytes) where T : struct;


        //void UpdateBuffer<T>(
        //    DeviceBuffer buffer,
        //    uint bufferOffsetInBytes,
        //    T[] source) where T : struct;


        //void UpdateBuffer(
        //    DeviceBuffer buffer,
        //    uint bufferOffsetInBytes,
        //    IntPtr source,
        //    uint sizeInBytes);


        //void CopyBuffer(DeviceBuffer source, uint sourceOffset, DeviceBuffer destination, uint destinationOffset, uint sizeInBytes);


        //void CopyTexture(Texture source, Texture destination);


        //void CopyTexture(Texture source, Texture destination, uint mipLevel, uint arrayLayer);


        //void CopyTexture(
        //    Texture source,
        //    uint srcX,
        //    uint srcY,
        //    uint srcZ,
        //    uint srcMipLevel,
        //    uint srcBaseArrayLayer,
        //    Texture destination,
        //    uint dstX,
        //    uint dstY,
        //    uint dstZ,
        //    uint dstMipLevel,
        //    uint dstBaseArrayLayer,
        //    uint width,
        //    uint height,
        //    uint depth,
        //    uint layerCount);


        //void GenerateMipmaps(Texture texture);


        //void PushDebugGroup(string name);


        //void PopDebugGroup();


        //void InsertDebugMarker(string name);
        #endregion
    }
}