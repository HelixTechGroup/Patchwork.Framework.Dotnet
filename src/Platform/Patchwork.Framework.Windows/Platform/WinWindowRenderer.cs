#region Usings
using System;
using System.Drawing;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
#endregion

namespace Patchwork.Framework.Platform
{
    public class WinWindowRenderer : NWindowRenderer
    {
        protected WindowsProcessHook m_hook;

        public WinWindowRenderer(INWindow  window, INRenderDevice renderDevice) : base(window, renderDevice)
        {
            m_hook = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_GETMESSAGE);
            m_hook.ProcessMessage += OnGetMsg;
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_hook.Initialize();
        }

        /// <inheritdoc />
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e)
        {
            
        }

        #region Methods
        public bool Validate(ref Rectangle rect)
        {
            return Methods.ValidateRect(m_hook.Process.Handle.Pointer, ref rect);
        }

        public bool Invalidate(ref Rectangle rect, bool shouldErase)
        {
            return Methods.InvalidateRect(m_hook.Process.Handle.Pointer, ref rect, shouldErase);
        }

        public bool Invalidate(bool shouldErase)
        {
            return Methods.InvalidateRect(m_hook.Process.Handle.Pointer, IntPtr.Zero, shouldErase);
        }

        private IntPtr OnGetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    PlatformPaint();
                    break;
                case WindowsMessageIds.NCCALCSIZE:
                case WindowsMessageIds.SIZE:
                case WindowsMessageIds.MOVE:
                case WindowsMessageIds.WINDOWPOSCHANGED:
                case WindowsMessageIds.WINDOWPOSCHANGING:
                case WindowsMessageIds.ERASEBKGND:
                case WindowsMessageIds.DISPLAYCHANGE:
                case WindowsMessageIds.CAPTURECHANGED:
                case WindowsMessageIds.NCHITTEST:
                case WindowsMessageIds.GETMINMAXINFO:
                case WindowsMessageIds.NCPAINT:
                    break;
            }

            return res;
        }

        protected override bool PlatformInvalidate()
        {
            return Invalidate(false);
        }

        /// <inheritdoc />
        protected override void PlatformPaint()
        {
            if (BeginPaint(m_window.Handle.Pointer, out var ps) != IntPtr.Zero) 
                return;

            if (GetUpdateRect(m_hook.Process.Handle.Pointer, out var rec, false))
                Validate();
            OnPaint();
            EndPaint(m_window.Handle.Pointer, ref ps);
        }

        protected override bool PlatformValidate()
        {
            return ValidateRect(m_hook.Process.Handle.Pointer, IntPtr.Zero);
        }
        #endregion
    }
}