#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    [Flags]
    public enum NWindowDecorations
    {
        None = 0,
        Titlebar,
        Border,
        All = Titlebar | Border
    }
}