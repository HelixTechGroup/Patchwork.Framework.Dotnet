#region Usings
using System;
using System.Drawing;
using Patchwork.Framework.Platform.Interop.User32;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
#endregion

namespace Patchwork.Framework.Platform
{
    public class WindowsDesktopWindowRenderer : WindowsProcessHook, INativeWindowRenderer
    {
        #region Events
        /// <inheritdoc />
        public event EventHandler Painting;

        /// <inheritdoc />
        public event EventHandler<Rectangle> Paint;

        /// <inheritdoc />
        public event EventHandler Painted;
        #endregion

        #region Members
        private INativeScreen m_screen;
        private Size m_virutalSize;
        private float m_aspectRatio;
        private INativeRenderAdapter m_adapter;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INativeRenderAdapter Adapter
        {
            get { return m_adapter; }
        }

        /// <inheritdoc />
        public INativeScreen Screen
        {
            get { return m_screen; }
        }

        /// <inheritdoc />
        public Size VirutalSize
        {
            get { return m_virutalSize; }
        }

        /// <inheritdoc />
        public float AspectRatio
        {
            get { return m_aspectRatio; }
        }
        #endregion

        public WindowsDesktopWindowRenderer(IWindowsProcess window, INativeRenderAdapter renderAdapter) : base(window, WindowHookType.WH_GETMESSAGE)
        {
            m_adapter = renderAdapter;
        }

        #region Methods
        /// <inheritdoc />
        protected override IntPtr OnGetMsg(WindowsMessage message)
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

            return base.OnGetMsg(message);
        }

        public bool Validate(ref Rectangle rect)
        {
            return ValidateRect(m_process.Handle.Pointer, ref rect);
        }

        public bool Validate()
        {
            return ValidateRect(m_process.Handle.Pointer, IntPtr.Zero);
        }

        public bool Invalidate(ref Rectangle rect, bool shouldErase)
        {
            return InvalidateRect(m_process.Handle.Pointer, ref rect, shouldErase);
        }

        public bool Invalidate(bool shouldErase)
        {
            return InvalidateRect(m_process.Handle.Pointer, IntPtr.Zero, shouldErase);
        }

        public bool Invalidate()
        {
            return Invalidate(false);
        }

        private IntPtr OnPaint(WindowsMessage msg)
        {
            if (GetUpdateRect(m_process.Handle.Pointer, out var rec, false))
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
        #endregion
    }
}