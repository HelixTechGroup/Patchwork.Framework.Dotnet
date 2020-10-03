#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [Flags]
    public enum RegionModeFlags
    {
        RGN_AND = 1,
        RGN_OR = 2,
        RGN_XOR = 3,
        RGN_DIFF = 4,
        RGN_COPY = 5,
        RGN_MIN = RGN_AND,
        RGN_MAX = RGN_COPY
    }
}