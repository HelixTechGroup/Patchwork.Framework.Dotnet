#region Usings
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface INativeRenderDevice : IInitialize, IDispose
    {
        #region Methods
        TAdapter GetAdapter<TAdapter>() where TAdapter : INativeRenderAdapter;
        TRenderer GetRenderer<TRenderer>() where TRenderer : INativeRenderer;
        #endregion
    }

    public interface INativeRenderDevice<TAdapter> : INativeRenderAdapter
        where TAdapter : INativeRenderAdapter { }
}