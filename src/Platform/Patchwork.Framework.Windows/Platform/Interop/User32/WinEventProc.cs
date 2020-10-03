#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    public delegate void WinEventProc(IntPtr hWinEventHook,
                                      uint eventType,
                                      IntPtr hwnd,
                                      uint idObject,
                                      uint idChild,
                                      uint dwEventThread,
                                      uint dwmsEventTime);
}