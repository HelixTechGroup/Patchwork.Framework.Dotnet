#region Usings
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface IINResource<TNType> : INRenderer
    {
        #region Properties
        TNType Resource { get; }
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