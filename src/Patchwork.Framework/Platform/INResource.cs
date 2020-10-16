#region Usings
using Patchwork.Framework.Platform.Rendering;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface INResource<TNType> : INResource
    {
        #region Properties
        new TNType Resource { get; }
        #endregion
    }

    public interface INResource : IInitialize, IDispose
    {
        #region Properties
        string Name { get; set; }
        object Resource { get; }
        #endregion
    }
}