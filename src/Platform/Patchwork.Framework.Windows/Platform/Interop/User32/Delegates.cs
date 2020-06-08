using System;

namespace Patchwork.Framework.Platform.Interop.User32
{
    public delegate IntPtr WindowProc(IntPtr hwnd, WindowsMessageIds msg, IntPtr wParam, IntPtr lParam);

    public delegate void WinEventProc(IntPtr hWinEventHook,
                                      uint eventType,
                                      IntPtr hwnd,
                                      uint idObject,
                                      uint idChild,
                                      uint dwEventThread,
                                      uint dwmsEventTime);

    public delegate IntPtr HookProc(WindowHookCode code, IntPtr wParam, IntPtr lParam);

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    public delegate IntPtr GetMsgProc(WindowHookCode code, WindowsMessageIds msg, IntPtr lParam);

    public delegate void TimerProc(IntPtr hWnd, uint uMsg, IntPtr nIdEvent, uint dwTickCountMillis);
}