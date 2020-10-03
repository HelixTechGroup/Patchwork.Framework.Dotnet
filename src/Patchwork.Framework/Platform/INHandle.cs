#region Usings
using System;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface INHandle : IDispose
    {
        #region Properties
        string HandleDescriptor { get; }
        IntPtr Pointer { get; }
        #endregion
    }
}