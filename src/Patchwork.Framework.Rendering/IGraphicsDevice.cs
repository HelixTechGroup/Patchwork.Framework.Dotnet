using System;

namespace Microsoft.Xna.Framework.Graphics
{
    public interface IGraphicsDevice
    {

        GraphicsBackend BackendType { get; }


        bool IsUvOriginTopLeft { get; }


        bool IsDepthRangeZeroToOne { get; }


        bool IsClipSpaceYInverted { get; }


        ResourceFactory ResourceFactory { get; }


        Swapchain MainSwapchain { get; }


        GraphicsDeviceFeatures Features { get; }


        bool SyncToVerticalBlank { get; set; }


        uint UniformBufferMinOffsetAlignment { get; }


        uint StructuredBufferMinOffsetAlignment { get; }


        Framebuffer SwapchainFramebuffer { get; }


        Sampler PointSampler { get; }


        Sampler LinearSampler { get; }


        Sampler Aniso4xSampler { get; }


        void SubmitCommands(CommandList commandList);


        void SubmitCommands(CommandList commandList, Fence fence);


        void WaitForFence(Fence fence);


        bool WaitForFence(Fence fence, TimeSpan timeout);


        bool WaitForFence(Fence fence, ulong nanosecondTimeout);


        void WaitForFences(Fence[] fences, bool waitAll);


        bool WaitForFences(Fence[] fences, bool waitAll, TimeSpan timeout);


        bool WaitForFences(Fence[] fences, bool waitAll, ulong nanosecondTimeout);


        void ResetFence(Fence fence);


        void SwapBuffers();


        void SwapBuffers(Swapchain swapchain);


        void ResizeMainWindow(uint width, uint height);


        void WaitForIdle();


        TextureSampleCount GetSampleCountLimit(PixelFormat format, bool depthFormat);


        MappedResource Map(MappableResource resource, MapMode mode);


        MappedResource Map(MappableResource resource, MapMode mode, uint subresource);


        MappedResourceView<T> Map<T>(MappableResource resource, MapMode mode) where T : struct;


        MappedResourceView<T> Map<T>(MappableResource resource, MapMode mode, uint subresource) where T : struct;


        void Unmap(MappableResource resource);


        void Unmap(MappableResource resource, uint subresource);


        void UpdateTexture(
            Texture texture,
            IntPtr source,
            uint sizeInBytes,
            uint x, uint y, uint z,
            uint width, uint height, uint depth,
            uint mipLevel, uint arrayLayer);


        void UpdateTexture<T>(
            Texture texture,
            T[] source,
            uint x, uint y, uint z,
            uint width, uint height, uint depth,
            uint mipLevel, uint arrayLayer) where T : struct;


        unsafe void UpdateBuffer<T>(
            DeviceBuffer buffer,
            uint bufferOffsetInBytes,
            T source) where T : struct;


        unsafe void UpdateBuffer<T>(
            DeviceBuffer buffer,
            uint bufferOffsetInBytes,
            ref T source) where T : struct;


        unsafe void UpdateBuffer<T>(
            DeviceBuffer buffer,
            uint bufferOffsetInBytes,
            ref T source,
            uint sizeInBytes) where T : struct;


        unsafe void UpdateBuffer<T>(
            DeviceBuffer buffer,
            uint bufferOffsetInBytes,
            T[] source) where T : struct;


        void UpdateBuffer(
            DeviceBuffer buffer,
            uint bufferOffsetInBytes,
            IntPtr source,
            uint sizeInBytes);


        bool GetPixelFormatSupport(
            PixelFormat format,
            TextureType type,
            TextureUsage usage);


        bool GetPixelFormatSupport(
            PixelFormat format,
            TextureType type,
            TextureUsage usage,
            out PixelFormatProperties properties);


        void DisposeWhenIdle(IDisposable disposable);


        void Dispose();


        bool GetD3D11Info(out BackendInfoD3D11 info);


        BackendInfoD3D11 GetD3D11Info();


        bool GetVulkanInfo(out BackendInfoVulkan info);


        BackendInfoVulkan GetVulkanInfo();


        bool GetOpenGLInfo(out BackendInfoOpenGL info);


        BackendInfoOpenGL GetOpenGLInfo();
    }

    public interface IGraphicsDevice
    {

        object Handle { get; }


        bool UseHalfPixelOffset { get; }

        TextureCollection VertexTextures { get; }
        SamplerStateCollection VertexSamplerStates { get; }
        TextureCollection Textures { get; }
        SamplerStateCollection SamplerStates { get; }
        bool IsDisposed { get; }
        bool IsContentLost { get; }
        GraphicsAdapter Adapter { get; }


        GraphicsMetrics Metrics { get; set; }


        GraphicsDebug GraphicsDebug { get; set; }

        RasterizerState RasterizerState { get; set; }


        Color BlendFactor { get; set; }

        BlendState BlendState { get; set; }
        DepthStencilState DepthStencilState { get; set; }
        DisplayMode DisplayMode { get; }
        GraphicsDeviceStatus GraphicsDeviceStatus { get; }
        PresentationParameters PresentationParameters { get; }
        Viewport Viewport { get; set; }
        GraphicsProfile GraphicsProfile { get; }
        Rectangle ScissorRectangle { get; set; }
        int RenderTargetCount { get; }
        IndexBuffer Indices { set; get; }
        bool ResourcesLost { get; set; }

        void SetRenderTarget(RenderTarget2D renderTarget, int arraySlice);

        void SetRenderTarget(RenderTarget3D renderTarget, int arraySlice);


        void Flush();

        event EventHandler<EventArgs> DeviceLost;
        event EventHandler<EventArgs> DeviceReset;
        event EventHandler<EventArgs> DeviceResetting;
        event EventHandler<ResourceCreatedEventArgs> ResourceCreated;
        event EventHandler<ResourceDestroyedEventArgs> ResourceDestroyed;
        event EventHandler<EventArgs> Disposing;

        void Clear(Color color);

        void Clear(ClearOptions options, Color color, float depth, int stencil);

        void Clear(ClearOptions options, Vector4 color, float depth, int stencil);

        void Dispose();

        void Present();

        void Reset();

        void Reset(PresentationParameters presentationParameters);

        void SetRenderTarget(RenderTarget2D renderTarget);

        void SetRenderTarget(RenderTargetCube renderTarget, CubeMapFace cubeMapFace);

        void SetRenderTargets(params RenderTargetBinding[] renderTargets);

        RenderTargetBinding[] GetRenderTargets();

        void GetRenderTargets(RenderTargetBinding[] outTargets);

        void SetVertexBuffer(VertexBuffer vertexBuffer);

        void SetVertexBuffer(VertexBuffer vertexBuffer, int vertexOffset);

        void SetVertexBuffers(params VertexBufferBinding[] vertexBuffers);


        void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCount);


        void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int startIndex, int primitiveCount);


        void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount) where T : struct, IVertexType;


        void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct;


        void DrawPrimitives(PrimitiveType primitiveType, int vertexStart, int primitiveCount);


        void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount) where T : struct, IVertexType;


        void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct;


        void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount) where T : struct, IVertexType;


        void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct;


        void DrawInstancedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex,
                                     int numVertices, int startIndex, int primitiveCount, int instanceCount);


        void DrawInstancedPrimitives(PrimitiveType primitiveType, int baseVertex, int startIndex, int primitiveCount, int instanceCount);


        void DrawInstancedPrimitives(PrimitiveType primitiveType, int baseVertex, int startIndex, int primitiveCount, int baseInstance, int instanceCount);


        void GetBackBufferData<T>(T[] data) where T : struct;

        void GetBackBufferData<T>(T[] data, int startIndex, int elementCount) where T : struct;

        void GetBackBufferData<T>(Rectangle? rect, T[] data, int startIndex, int elementCount)
            where T : struct;
    }
}
