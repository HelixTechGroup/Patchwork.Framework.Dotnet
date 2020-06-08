#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface INativeHandle
    {
        #region Properties
        string HandleDescriptor { get; }
        IntPtr Pointer { get; }
        #endregion
    }
}