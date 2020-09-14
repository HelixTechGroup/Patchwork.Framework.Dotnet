#region Usings
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface INativeRenderDevice : IInitialize, IDispose
    {
        #region Methods
        #endregion
    }

    public interface INativeRenderDevice<TAdapter, TRenderer> : INativeRenderAdapter
        where TAdapter : INativeRenderAdapter
        where TRenderer : INativeWindowRenderer { }
}