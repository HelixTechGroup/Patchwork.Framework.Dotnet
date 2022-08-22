#region Usings
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INRenderAdapter : IInitialize, IDispose
    {
        #region Properties
        INRenderAdapterConfiguration Configuration { get; }

        INRenderDevice Device { get; }

        INResourceFactory ResourceFactory { get; }

        INScreen Screen { get; }
        #endregion

        #region Methods
        void SwapBuffers();

        void Flush();
        #endregion

        TResource CreateResource<TResource>(params object[] parameters) where TResource : class, INRenderResource;

        //IRenderCommandList GetCommandList();

        //void SubmitCommand(IRenderCommandList commandList);

        //void SubmitCommands(IRenderCommandList commandList, INRenderFence fence);


        //void WaitForFence(INRenderFence fence);


        //bool WaitForFence(INRenderFence fence, TimeSpan timeout);


        //bool WaitForFence(INRenderFence fence, ulong nanosecondTimeout);


        //void WaitForFences(INRenderFence[] fences, bool waitAll);


        //bool WaitForFences(INRenderFence[] fences, bool waitAll, TimeSpan timeout);


        //bool WaitForFences(INRenderFence[] fences, bool waitAll, ulong nanosecondTimeout);


        //void ResetFence(INRenderFence fence);

        ////void SwapBuffers(Swapchain swapchain);


        ////void ResizeMainWindow(uint width, uint height);


        //void WaitForIdle();


        ////TextureSampleCount GetSampleCountLimit(PixelFormat format, bool depthFormat);


        ////MappedResource Map(MappableResource resource, MapMode mode);


        ////MappedResource Map(MappableResource resource, MapMode mode, uint subresource);


        ////MappedResourceView<T> Map<T>(MappableResource resource, MapMode mode) where T : struct;


        ////MappedResourceView<T> Map<T>(MappableResource resource, MapMode mode, uint subresource) where T : struct;


        ////void Unmap(MappableResource resource);


        ////void Unmap(MappableResource resource, uint subresource);


        ////void UpdateTexture(
        ////    Texture texture,
        ////    IntPtr source,
        ////    uint sizeInBytes,
        ////    uint x, uint y, uint z,
        ////    uint width, uint height, uint depth,
        ////    uint mipLevel, uint arrayLayer);


        ////void UpdateTexture<T>(
        ////    Texture texture,
        ////    T[] source,
        ////    uint x, uint y, uint z,
        ////    uint width, uint height, uint depth,
        ////    uint mipLevel, uint arrayLayer) where T : struct;


        ////unsafe void UpdateBuffer<T>(
        ////    DeviceBuffer buffer,
        ////    uint bufferOffsetInBytes,
        ////    T source) where T : struct;


        ////unsafe void UpdateBuffer<T>(
        ////    DeviceBuffer buffer,
        ////    uint bufferOffsetInBytes,
        ////    ref T source) where T : struct;


        ////unsafe void UpdateBuffer<T>(
        ////    DeviceBuffer buffer,
        ////    uint bufferOffsetInBytes,
        ////    ref T source,
        ////    uint sizeInBytes) where T : struct;


        ////unsafe void UpdateBuffer<T>(
        ////    DeviceBuffer buffer,
        ////    uint bufferOffsetInBytes,
        ////    T[] source) where T : struct;


        ////void UpdateBuffer(
        ////    DeviceBuffer buffer,
        ////    uint bufferOffsetInBytes,
        ////    IntPtr source,
        ////    uint sizeInBytes);


        ////bool GetPixelFormatSupport(
        ////    PixelFormat format,
        ////    TextureType type,
        ////    TextureUsage usage);


        ////bool GetPixelFormatSupport(
        ////    PixelFormat format,
        ////    TextureType type,
        ////    TextureUsage usage,
        ////    out PixelFormatProperties properties);


        //void DisposeWhenIdle(IDispose disposable);
    }
}