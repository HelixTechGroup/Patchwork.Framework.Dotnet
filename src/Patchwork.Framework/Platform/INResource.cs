#region Usings
using Patchwork.Framework.Platform.Rendering;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface IINResource<TNType> : INResource
    {
        #region Properties
        new TNType Resource { get; }
        #endregion
    }

    public interface INResource : IInitialize, IDispose
    {
        #region Properties
        INRenderDevice Device { get; }
        string Name { get; set; }
        object Resource { get; }
        #endregion
    }
}