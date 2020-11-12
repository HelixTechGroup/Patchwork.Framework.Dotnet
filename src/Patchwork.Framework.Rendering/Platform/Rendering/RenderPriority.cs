using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Platform.Rendering
{
    public enum RenderPriority : int
    {
        None = 0,
        Realtime = int.MaxValue,
        Highest = 5,
        High = 4,
        Normal = 3,
        Low = 2,
        Lowest = 1
    }
}
