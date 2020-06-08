using System;
using Patchwork.Framework.Platform.Interop.User32;

namespace Patchwork.Framework.Platform
{
    public struct WindowsMessage
    {
        public IntPtr Hwnd;
        public WindowsMessageIds Id;
        public IntPtr WParam;
        public IntPtr LParam;
        public IntPtr Result;

        public WindowsMessage(IntPtr hwnd, uint id, IntPtr wParam, IntPtr lParam)
        {
            this.Hwnd = hwnd;
            this.Id = (WindowsMessageIds)id;
            this.WParam = wParam;
            this.LParam = lParam;
            this.Result = IntPtr.Zero;
        }

        public void SetResult(IntPtr result)
        {
            this.Result = result;
        }

        public void SetResult(int result)
        {
            this.SetResult(new IntPtr(result));
        }
    }
}
