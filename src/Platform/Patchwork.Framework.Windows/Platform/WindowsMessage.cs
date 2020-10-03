#region Usings
using System;
using Patchwork.Framework.Platform.Interop.User32;
#endregion

namespace Patchwork.Framework.Platform
{
    public struct WindowsMessage
    {
        #region Members
        public IntPtr Hwnd;
        public WindowsMessageIds Id;
        public IntPtr LParam;
        public IntPtr Result;
        public IntPtr WParam;
        #endregion

        public WindowsMessage(IntPtr hwnd, uint id, IntPtr wParam, IntPtr lParam)
        {
            Hwnd = hwnd;
            Id = (WindowsMessageIds)id;
            WParam = wParam;
            LParam = lParam;
            Result = IntPtr.Zero;
        }

        #region Methods
        public void SetResult(IntPtr result)
        {
            Result = result;
        }

        public void SetResult(int result)
        {
            SetResult(new IntPtr(result));
        }
        #endregion
    }
}