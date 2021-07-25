using System;
using System.Collections.Generic;
using System.Text;
using Shin.Framework;

namespace Patchwork.Framework.Threading
{
    public interface ISynchronizeContext : IDispose
    {
        SynchronizationAccess AccessLevel { get; }
    }
}
