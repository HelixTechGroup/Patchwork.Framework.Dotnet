#region Usings
using System;
using System.Drawing;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Window;
using Shin.Framework.ComponentModel;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
#endregion

namespace Patchwork.Framework.Platform
{
    public class WinWindowRenderer : NWindowRenderer
    {
        protected WindowsProcessHook m_hook;

        public WinWindowRenderer(IWindowsProcess window, INRenderDevice renderDevice) : base(window, renderDevice)
        {
            m_hook = new WindowsProcessHook(window, WindowHookType.WH_GETMESSAGE);
            m_hook.ProcessMessage += OnGetMsg;
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

        /// <inheritdoc />
        protected IntPtr OnGetMsg(WindowsMessage message)
        {
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    return OnPaint(message);
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

            return IntPtr.Zero;
        }

        private IntPtr OnPaint(WindowsMessage msg)
        {
            if (GetUpdateRect(m_hook.Process.Handle.Pointer, out var rec, false))
                Validate();

            //if (BeginPaint(m_process.Handle.Pointer, out var ps) == IntPtr.Zero) 
            //    return IntPtr.Zero;

            //var f = m_scaling;
            //var r = ps.PaintRect;
            //Painting.Raise(this, null);
            //Paint.Raise(this, new Rectangle((int)Math.Floor(r.Left / f),
            //                                (int)Math.Floor(r.Top / f),
            //                                (int)Math.Floor((r.Right - r.Left) / f),
            //                                (int)Math.Floor((r.Bottom - r.Top) / f)));
            //Painted.Raise(this, null);
            //EndPaint(m_process.Handle.Pointer, ref ps);
            //Validate();

            return IntPtr.Zero;
        }

        protected override bool PlatformInvalidate()
        {
            return Invalidate(false);
        }

        protected override bool PlatformValidate()
        {
            return Methods.ValidateRect(m_hook.Process.Handle.Pointer, IntPtr.Zero);
        }
        #endregion
    }
}