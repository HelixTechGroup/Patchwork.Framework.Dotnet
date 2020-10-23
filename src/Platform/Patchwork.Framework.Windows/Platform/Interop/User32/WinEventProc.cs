#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    public delegate void WinEventProc(IntPtr hWinEventHook,
                                      SWEH_Events eventType,
                                      IntPtr hwnd,
                                      SWEH_ObjectId idObject,
                                      uint idChild,
                                      uint dwEventThread,
                                      uint dwmsEventTime);
}