#region Usings
//using Patchwork.Framework.Platform.Interop.GdiPlus;
//using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop.Gdi32;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Helpers;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class GdiWindowRenderer : NWindowRenderer, IFrameBufferRenderer
    {
        #region Members
        protected IntPtr m_bmp;
        protected NFrameBuffer m_buffer;

        protected IntPtr m_hdc;
        protected IntPtr m_memHdc;
        protected NFrameBuffer m_oldBuffer;

        //protected WindowsProcessHook m_hook;
        protected PaintStruct m_paintStruct;
        #endregion

        public GdiWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
            //m_hook = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_GETMESSAGE);
            //m_hook.ProcessMessage += OnGetMsg;
            m_buffer = new NFrameBuffer(m_window.ClientSize.Width, m_window.ClientSize.Height);
            renderDevice.ProcessMessage += OnProcessMessage;
        }

        #region Methods
        public bool Validate(ref Rectangle rect)
        {
            return ValidateRect(m_window.Handle.Pointer, ref rect);
        }

        public bool Invalidate(ref Rectangle rect, bool shouldErase)
        {
            return InvalidateRect(m_window.Handle.Pointer, ref rect, shouldErase);
        }

        public bool Invalidate(bool shouldErase)
        {
            return InvalidateRect(m_window.Handle.Pointer, IntPtr.Zero, shouldErase);
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            //m_hook.Initialize();
        }

        protected override void OnProcessMessage(IPlatformMessage message)
        {
            switch (message.Id)
            {
                case MessageIds.Rendering:
                    Core.Logger.LogDebug("Found Rendering Messages.");
                    var data = message.RawData as IRenderMessageData;
                    switch (data?.MessageId)
                    {
                        case RenderMessageIds.None:
                            break;
                        case RenderMessageIds.SetFrameBuffer:
                            m_buffer = data.FrameBuffer;
                            Invalidate();
                            break;
                    }

                    break;
            }
        }

        /// <inheritdoc />
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e) { }

        protected override bool PlatformInvalidate()
        {
            return Invalidate(false);
        }

        /// <inheritdoc />
        protected override void PlatformRender()
        {
            if (m_hdc == IntPtr.Zero)
            {
                m_hdc = GetDC(IntPtr.Zero);
                m_memHdc = CreateCompatibleDC(m_hdc);
                m_bmp = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
            }

            //var graphic = new GraphicsPlus(m_hdc);
            //SetBkMode(m_hdc, 1);
            //SetBkColor(m_hdc, 0);
            //IntPtr brush = CreateSolidBrush(ColorTranslator.ToWin32(Color.Blue));
            //var gb = new GpBrush();
            //gb.ptr = brush;
            //var b = new BrushPlus();
            //b.SetNativeBrush(gb);
            //var rec = m_window.ClientArea;
            //SetClassLongPtr(m_window.Handle.Pointer, -10, brush);
            //graphic.FillRectangle(b, rec);
            //var s = graphic.GetLastStatus();
            //graphic.Dispose();
            //IntPtr brush2 = CreateSolidBrush(ColorTranslator.ToWin32(Color.Orange));
            //FillRect(m_hdc, ref rec, brush2);
            //BitBlt(m_hdc, 0, 0, rec.Width, -rec.Height, m_memHdc, 0, 0, BitBltFlags.SRCCOPY);
            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRender, this);
        }

        /// <inheritdoc />
        protected override void PlatformRendered()
        {
            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendered, this);
            SetBuffer(m_buffer);
            EndPaint(m_window.Handle.Pointer, ref m_paintStruct);
            //ReleaseDC(IntPtr.Zero, m_memHdc);
            DeleteObject(m_bmp);
            //if (GetUpdateRect(m_window.Handle.Pointer, out var rec, false))
            //    Validate(ref rec);
            //Validate();
            m_oldBuffer = m_buffer;
        }

        protected override void PlatformRendering()
        {
            m_hdc = BeginPaint(m_window.Handle.Pointer, out m_paintStruct);

            m_memHdc = CreateCompatibleDC(m_hdc);
            //m_bmp = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
            //if (m_bmp == IntPtr.Zero)
            //{
            //    var e = (int)GetLastError();
            //    if (e != 0)
            //        throw new Win32Exception(e);
            //    return;
            //}
            //SelectObject(m_memHdc, m_bmp);

            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendering, this);
        }

        protected override bool PlatformValidate()
        {
            return ValidateRect(m_window.Handle.Pointer, IntPtr.Zero);
        }

        protected void SetBuffer(NFrameBuffer buffer)
        {
            var pb = buffer.PixelBuffer;
            //var hBitmap = IntPtr.Zero;
            var pBytes = pb.Contents;
            var p = IntPtr.Zero;

            if (pBytes.Length <= 0) return;

            var ret = SetBitsToBitmap(m_memHdc,
                                      m_window.ClientSize.Width, m_window.ClientSize.Height, pBytes, out var bmi);
            if (ret == IntPtr.Zero)
                throw new Win32Exception();

            var e = (int)GetLastError();
            if (e != 0)
                throw new Win32Exception(e);

            if (!StretchBlt(m_hdc,
                   0,
                   0,
                   m_window.ClientArea.Width,
                   m_window.ClientArea.Height,
                   m_memHdc,
                   0,
                   0,
                   pb.Width,
                   pb.Height,
                   BitBltFlags.SRCCOPY))
                throw new Win32Exception();

            e = (int)GetLastError();
            if (e != 0)
                throw new Win32Exception(e);
        }

        private IntPtr OnGetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    Render();
                    break;
                case WindowsMessageIds.ERASEBKGND:
                    return new IntPtr(1);
                case WindowsMessageIds.SIZE:
                case WindowsMessageIds.MOVE:
                case WindowsMessageIds.WINDOWPOSCHANGED:
                case WindowsMessageIds.WINDOWPOSCHANGING:
                case WindowsMessageIds.NCCALCSIZE:
                case WindowsMessageIds.DISPLAYCHANGE:
                case WindowsMessageIds.CAPTURECHANGED:
                case WindowsMessageIds.NCHITTEST:
                case WindowsMessageIds.GETMINMAXINFO:
                case WindowsMessageIds.NCPAINT:
                    break;
            }

            return res;
        }
        #endregion
    }
}