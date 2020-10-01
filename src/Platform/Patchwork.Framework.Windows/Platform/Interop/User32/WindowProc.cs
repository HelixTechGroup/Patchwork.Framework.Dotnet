using System;

namespace Patchwork.Framework.Platform.Interop.User32
{
    public delegate IntPtr WindowProc(IntPtr hwnd, WindowsMessageIds msg, IntPtr wParam, IntPtr lParam);
}