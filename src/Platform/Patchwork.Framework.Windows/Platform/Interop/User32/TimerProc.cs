using System;

namespace Patchwork.Framework.Platform.Interop.User32
{
    public delegate void TimerProc(IntPtr hWnd, uint uMsg, IntPtr nIdEvent, uint dwTickCountMillis);
}