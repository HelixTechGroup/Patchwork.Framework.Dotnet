#region Usings
using System;

using Patchwork.Framework.Platform.Rendering;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface INResource<TNType> : INResource, ICloneable<TNType>
    {
        #region Properties
        new TNType Resource { get; }
        #endregion
    }

    public interface INResource : INObject, ICreate, ICloneable
    {
        #region Properties
        string Name { get; set; }
        object Resource { get; }
        #endregion
    }
}