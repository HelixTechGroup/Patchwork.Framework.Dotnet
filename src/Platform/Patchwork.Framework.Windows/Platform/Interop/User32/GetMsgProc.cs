#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    public delegate IntPtr GetMsgProc(WindowHookCode code, WindowsMessageIds msg, IntPtr lParam);
}