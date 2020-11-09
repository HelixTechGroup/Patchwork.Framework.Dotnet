#region Usings
using System;
using System.Runtime.InteropServices;
using Shin.Framework;
using Shin.Framework.Runtime;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface INHandle : IDispose
    {
        #region Properties
        string HandleDescriptor { get; }
        IntPtr Pointer { get; }

        INHandle Lock();
        void Unlock();
        #endregion
    }
}