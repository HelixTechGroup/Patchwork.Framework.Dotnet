using System;

namespace Patchwork.Framework.Platform.Interop.User32
{
    public delegate IntPtr HookProc(WindowHookCode code, IntPtr wParam, IntPtr lParam);
}