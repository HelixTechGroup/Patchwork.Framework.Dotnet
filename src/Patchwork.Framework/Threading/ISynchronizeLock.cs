using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Threading
{
    public interface ISynchronizeLock
    {
        ISynchronizeContext Context { get; set; }

        void Lock();

        void Unlock();
    }
}
