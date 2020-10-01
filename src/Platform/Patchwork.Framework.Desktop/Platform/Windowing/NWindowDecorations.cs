using System;

namespace Patchwork.Framework.Platform.Windowing
{
    [Flags]
    public enum NWindowDecorations
    {
        None = 0,
        CloseButton,
        MinimizeButton,
        MaximizeButton,
        Titlebar,
        Border,
        All = CloseButton | MinimizeButton | MaximizeButton | Titlebar | Border
    }
}
