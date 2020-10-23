#region Usings
//using Patchwork.Framework.Platform.Interop.GdiPlus;
//using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;
using System;
using System.ComponentModel;
using System.Drawing;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop.Gdi32;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Helpers;
using static Patchwork.Framework.Platform.Interop.Utilities;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class GdiWindowRenderer : NWindowRenderer, IFrameBufferRenderer
    {
        #region Members
        protected IntPtr m_bmp;
        protected NFrameBuffer m_buffer;
        //protected WindowsEventHook m_eventHook;
        protected bool m_bufferChanged;
        protected IntPtr m_hdc;
        protected bool m_inModalSizeLoop;

        protected WindowsProcessHook m_hook;
        protected WindowsProcessHook m_hook2;
        protected IntPtr m_memHdc;
        protected NFrameBuffer m_oldBuffer;
        protected PaintStruct m_paintStruct;
        #endregion

        public GdiWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
            m_hook = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_GETMESSAGE);
            m_hook.ProcessMessage += OnGetMsg;
            m_hook2 = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_CALLWNDPROCRET);
            m_hook2.ProcessMessage += OnRetMsg;
            //m_eventHook = new WindowsEventHook(window as IWindowsProcess, SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE);
            //m_eventHook.ProcessEvent += OnEventMsg;
            //m_oldBuffer = 
            m_buffer = new NFrameBuffer(m_window.ClientSize.Width, m_window.ClientSize.Height);
            renderDevice.ProcessMessage += OnProcessMessage;
        }

        #region Methods
        public bool Validate(ref Rectangle rect)
        {
            var ret = ValidateRect(m_window.Handle.Pointer, ref rect);
            CheckLastError();

            return ret;
        }

        public bool Invalidate(ref Rectangle rect, bool shouldErase)
        {
            return InvalidateRect(m_window.Handle.Pointer, ref rect, shouldErase);
        }

        public bool Invalidate(bool shouldErase)
        {
            //var ptt = Rectangle.Empty;
            //var ret = RedrawWindow(m_window.Handle.Pointer,
            //                       ref ptt,
            //                       IntPtr.Zero,
            //                       RedrawWindowFlags.RDW_INVALIDATE | RedrawWindowFlags.RDW_INTERNALPAINT);
            var ret = InvalidateRect(m_window.Handle.Pointer, IntPtr.Zero, shouldErase);
            //if (ret) 
            //    Core.Dispatcher.InvokeAsync(() => UpdateWindow(m_window.Handle.Pointer));
            //CheckLastError();

            return ret;
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_hook.Dispose();
            m_hook2.Dispose();
            //m_eventHook.Dispose();

            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_hook.Initialize();
            m_hook2.Initialize();
            //m_eventHook.Initialize();
        }

        protected override void OnProcessMessage(IPlatformMessage message)
        {
            switch (message.Id)
            {
                case MessageIds.Rendering:
                    //Core.Logger.LogDebug("Found Rendering Messages.");
                    var data = message.RawData as IRenderMessageData;
                    switch (data?.MessageId)
                    {
                        case RenderMessageIds.None:
                            break;
                        case RenderMessageIds.SetFrameBuffer:
                            if (data.FrameBuffer != m_oldBuffer)
                            {
                                //m_oldBuffer?.Dispose();
                                m_oldBuffer = m_buffer;
                                m_buffer = data.FrameBuffer;
                                m_bufferChanged = true;
                                Invalidate();
                            }
                            break;
                    }

                    break;
            }
        }

        /// <inheritdoc />
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e)
        {
            //Invalidate();
        }

        protected override bool PlatformInvalidate()
        {
            return Invalidate(false);
        }

        /// <inheritdoc />
        protected override void PlatformRender()
        {
            if (m_hdc == IntPtr.Zero)
                CreateHDC(IntPtr.Zero);

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
            SetBuffer();
            //Validate();
            if (GetUpdateRect(m_window.Handle.Pointer, out var rec, false))
                Validate(ref rec);
            EndPaint(m_window.Handle.Pointer, ref m_paintStruct);
            //CheckLastError();
            //if (GetUpdateRect(m_window.Handle.Pointer, out var rec, false))
            //    Validate(ref rec);
            //CheckLastError();
            CheckOperation(DeleteDC(m_memHdc));
            CheckOperation(DeleteObject(m_bmp));
            ReleaseDC(m_window.Handle.Pointer, m_hdc);
            //m_oldBuffer.Dispose();
            //m_oldBuffer = m_buffer;
        }

        protected override void PlatformRendering()
        {
            try
            {
                m_hdc = BeginPaint(m_window.Handle.Pointer, out m_paintStruct);
                CheckOperation(m_hdc != IntPtr.Zero);
                m_memHdc = CreateCompatibleDC(m_hdc);
                CheckOperation(m_memHdc != IntPtr.Zero);
                m_bmp = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
                CheckOperation(m_bmp != IntPtr.Zero);
            }
            catch (Win32Exception winEx)
            {
                if (m_hdc != IntPtr.Zero && winEx.NativeErrorCode != 1400)
                    throw;
            }

            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendering, this);
        }

        protected override bool PlatformValidate()
        {
            return ValidateRect(m_window.Handle.Pointer, IntPtr.Zero);
        }

        protected void SetBuffer()
        {
            if (!m_bufferChanged)
                return;

            var pb = m_buffer.PixelBuffer;
            var pBytes = pb.Contents;

            if (pBytes.Length <= 0)
                return;

            var bitCount = SetBitsToBitmap(m_memHdc,
                                           m_window.ClientSize.Width,
                                           m_window.ClientSize.Height,
                                           pBytes,
                                           out var bmi,
                                           m_bmp);
            CheckOperation(bitCount != 0);
            CheckOperation(SelectObject(m_memHdc, m_bmp));
            SetStretchBltMode(m_memHdc, StretchBltMode.STRETCH_DELETESCANS);
            //CheckLastError();
            //CheckOperation(SetRgbBitsToDevice(m_memHdc,
            //                                  m_window.ClientArea.Width,
            //                                  m_window.ClientArea.Height, pb.Handle.Pointer,
            //                                  0, 0, 0, 0));
            CheckOperation(StretchBlt(m_hdc,
                                      0,
                                      0,
                                      m_window.ClientArea.Width,
                                      m_window.ClientArea.Height,
                                      m_memHdc,
                                      0,
                                      0,
                                      pb.Width,
                                      pb.Height,
                                      BitBltFlags.SRCCOPY));
            m_bufferChanged = false;
        }

        private IntPtr OnGetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            //Core.Logger.LogDebug($"WINDOWS HOOK MESSAGE: {message.Id}");
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    Render();
                    res = new IntPtr(1);
                    break;
                case WindowsMessageIds.ERASEBKGND:
                //return new IntPtr(1);
                case WindowsMessageIds.MOVING:
                case WindowsMessageIds.SIZE:
                case WindowsMessageIds.MOVE:
                case WindowsMessageIds.WINDOWPOSCHANGED:
                case WindowsMessageIds.WINDOWPOSCHANGING:
                //Invalidate();
                //break;
                case WindowsMessageIds.ENTERSIZEMOVE:
                    //m_inModalSizeLoop = true;
                    //break;
                case WindowsMessageIds.EXITSIZEMOVE:
                    //m_inModalSizeLoop = false;
                    //break;
                case WindowsMessageIds.NCCALCSIZE:
                case WindowsMessageIds.DISPLAYCHANGE:
                case WindowsMessageIds.CAPTURECHANGED:
                case WindowsMessageIds.NCHITTEST:
                case WindowsMessageIds.GETMINMAXINFO:
                case WindowsMessageIds.NCPAINT:
                    break;
            }

            //var rect = Rectangle.Empty;
            //if (m_inModalSizeLoop)
            //{
            //    SendMessage(m_window.Handle.Pointer, (uint)WindowsMessageIds.PAINT, IntPtr.Zero, IntPtr.Zero);
            //    UpdateWindow(m_window.Handle.Pointer);
            //}

            return res;
        }

        private IntPtr OnRetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            //Core.Logger.LogDebug($"WINDOWS HOOK MESSAGE: {message.Id}");
            switch (message.Id)
            {
                //case WindowsMessageIds.PAINT:
                    //Render();
                    //break;
                case WindowsMessageIds.ERASEBKGND:
                    //Render();
                    return new IntPtr(1);
                //case WindowsMessageIds.MOVING:
                //case WindowsMessageIds.SIZING:
                case WindowsMessageIds.SIZE:
                case WindowsMessageIds.MOVE:
                case WindowsMessageIds.DISPLAYCHANGE:
                    //case WindowsMessageIds.WINDOWPOSCHANGED:
                    //case WindowsMessageIds.WINDOWPOSCHANGING:
                    //if(GetUpdateRect(m_window.Handle.Pointer, out var rect, false))
                    //    Invalidate(ref rect, true);
                    //Invalidate();
                    //res = new IntPtr(1);
                    break;
                case WindowsMessageIds.ENTERSIZEMOVE:
                    m_inModalSizeLoop = true;
                    res = new IntPtr(1);
                    break;
                case WindowsMessageIds.EXITSIZEMOVE:
                    m_inModalSizeLoop = false;
                    res = new IntPtr(1);
                    break;
                case WindowsMessageIds.NCCALCSIZE:
                case WindowsMessageIds.CAPTURECHANGED:
                case WindowsMessageIds.NCHITTEST:
                case WindowsMessageIds.GETMINMAXINFO:
                case WindowsMessageIds.NCPAINT:
                    break;
            }

            //var rect = Rectangle.Empty;
            //if (m_inModalSizeLoop)
            //{
            //    Invalidate();
            //    UpdateWindow(m_window.Handle.Pointer);
            //}

            return res;
        }

        private void OnEventMsg(WindowsEvent winEvent)
        {
            if (winEvent.ObjectId == SWEH_ObjectId.OBJID_WINDOW &&
                winEvent.EventId == SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE ||
                winEvent.EventId == SWEH_Events.EVENT_OBJECT_STATECHANGE)
                Invalidate();
        }

        private void CreateHDC(IntPtr hwnd)
        {
            try
            {
                m_hdc = GetDC(hwnd);
                CheckOperation(m_hdc != IntPtr.Zero);
                m_memHdc = CreateCompatibleDC(m_hdc);
                CheckOperation(m_memHdc != IntPtr.Zero);
                m_bmp = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
                CheckOperation(m_bmp != IntPtr.Zero);
            }
            catch (Win32Exception winEx)
            {
                if (m_hdc != IntPtr.Zero && winEx.NativeErrorCode != 1400)
                    throw;
            }
        }
        #endregion
    }
}