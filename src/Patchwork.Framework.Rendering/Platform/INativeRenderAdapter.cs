using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public interface INativeRenderAdapter : IInitialize, IDispose
    {
        INativeRenderAdapterConfiguration Configuration { get; }
    }
}
