using System;
using System.Collections.Generic;
using System.Text;
using Shin.Framework;

namespace Patchwork.Framework.Threading
{
    public interface ISynchronize : IDispose
    {
        object Enter();

        void Exit();
    }
}
