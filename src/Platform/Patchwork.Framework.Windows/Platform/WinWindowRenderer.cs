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
    public class WinWindowRenderer : WindowsProcessHook, INativeWindowRenderer
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
        private double m_dpiScaling;
        private NativeWindowTransparency m_transparencySupport;
        private NativeWindowDecorations m_supportedDecorations;
        private int m_titlebarSize;
        private double m_opacity;
        private Size m_size;
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
        public Size Size
        {
            get { return m_size; }
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

        public WinWindowRenderer(IWindowsProcess window, INativeRenderAdapter renderAdapter) : base(window, WindowHookType.WH_GETMESSAGE)
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

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<NativeWindowTransparency>> TransparencySupportChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<NativeWindowTransparency>> TransparencySupportChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<NativeWindowDecorations>> SupportedDecorationsChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<NativeWindowDecorations>> SupportedDecorationsChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<NativeWindowState>> StateChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<NativeWindowState>> StateChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<NativeWindowMode>> ModeChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<NativeWindowMode>> ModeChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<double>> DpiScalingChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<double>> OpacityChanged;

        /// <inheritdoc />
        public double DpiScaling
        {
            get { return m_dpiScaling; }
        }

        /// <inheritdoc />
        public NativeWindowTransparency TransparencySupport
        {
            get { return m_transparencySupport; }
            set { m_transparencySupport = value; }
        }

        /// <inheritdoc />
        public NativeWindowDecorations SupportedDecorations
        {
            get { return m_supportedDecorations; }
            set { m_supportedDecorations = value; }
        }

        /// <inheritdoc />
        public int TitlebarSize
        {
            get { return m_titlebarSize; }
        }

        /// <inheritdoc />
        public double Opacity
        {
            get { return m_opacity; }
            set { m_opacity = value; }
        }

        /// <inheritdoc />
        public void EnableWindowSystemDecorations()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void DisableWindowSystemDecorations()
        {
            throw new NotImplementedException();
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