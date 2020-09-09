using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework
{
    public enum ActivityEvent
    {
        Created,
        Resumed,
        Paused,
        Destroyed,
        SaveInstanceState,
        Started,
        Stopped
    }
}
