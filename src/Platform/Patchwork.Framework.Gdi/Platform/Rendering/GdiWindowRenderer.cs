#region Usings
//using Patchwork.Framework.Platform.Interop.GdiPlus;
//using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop.Gdi32;
using Patchwork.Framework.Platform.Interop.GdiPlus;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
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
        protected Bitmap m_bmp;
        protected IntPtr m_bmpPtr;
        protected NFrameBuffer m_buffer;
        //protected WindowsEventHook m_eventHook;
        protected bool m_bufferChanged;
        protected IntPtr m_hdc;
        protected bool m_inModalSizeLoop;
        protected static readonly object m_lock = new object();
        protected readonly ReaderWriterLockSlim m_lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        protected bool m_hasLock = false;
        protected readonly int m_lockTimeout = 50;
        //protected static readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(0);
        //[ThreadStatic]
        //protected static SpinLock m_spin = new SpinLock();
        protected WindowsProcessHook m_hook;
        protected WindowsProcessHook m_hook2;
        protected IntPtr m_memHdc;
        protected NFrameBuffer m_oldBuffer;
        protected PaintStruct m_paintStruct;
        #endregion

        public GdiWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
            m_priority = 0;
            m_hook = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_GETMESSAGE);
            m_hook.ProcessMessage += OnGetMsg;
            m_hook2 = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_CALLWNDPROC);
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
            //                      

            var rect = m_window.ClientArea;
            var ret = InvalidateRect(m_window.Handle.Pointer, ref rect, shouldErase);
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
                                //Render();
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
            //if (m_hdc == IntPtr.Zero)
            //    CreateHDC(IntPtr.Zero);

            //var graphic = new GraphicsPlus(m_hdc);
            //SetBkMode(m_hdc, 1);
            //SetBkColor(m_hdc, 0);
            //IntPtr brush = CreateSolidBrush(ColorTranslator.ToWin32(Color.Blue));
            //var gb = new GpBrush();
            //gb.ptr = brush;
            //var b = new BrushPlus();
            //b.SetNativeBrush(gb);
            //SetClassLongPtr(m_window.Handle.Pointer, -10, brush);
            //graphic.FillRectangle(b, rec);
            //var s = graphic.GetLastStatus();
            //graphic.Dispose();

            //var rec = m_window.ClientArea;
            //var brush2 = CreateSolidBrush(ColorTranslator.ToWin32(Color.Orange));
            //FillRect(m_hdc, ref rec, brush2);
            
            //BitBlt(m_hdc, 0, 0, rec.Width, -rec.Height, m_memHdc, 0, 0, BitBltFlags.SRCCOPY);
            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRender, this);
        }

        /// <inheritdoc />
        protected override void PlatformRendered()
        {
            SetBuffer();

            try
            {
                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //m_hasLock = false;
                //m_spin.TryEnter(ref m_hasLock);
                //if (!m_hasLock)
                //    return;
                TryLock();
                //Monitor.Enter(m_lock);
                lock (m_lock)
                {
                    //Core.Dispatcher.InvokeAsync(SetBuffer);
                    //Validate();
                    //if (GetUpdateRect(m_window.Handle.Pointer, out var rec, false))
                    //   Validate(ref rec);

                    EndPaint(m_window.Handle.Pointer, ref m_paintStruct);

                    CheckOperation(DeleteDC(m_memHdc));
                    CheckOperation(DeleteObject(m_bmpPtr));
                    ReleaseDC(m_window.Handle.Pointer, m_hdc);
                    m_oldBuffer?.Dispose();
                    m_oldBuffer = m_buffer.Copy();
                    //m_buffer?.Dispose();
                    Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendered, this);
                    //Monitor.Exit(m_lock);
                }
            }
            finally
            {
                if (m_hasLock)
                {
                    //if (m_spin.IsHeldByCurrentThread)
                    //    m_spin.Exit();
                    m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
        }

        protected override void PlatformRendering()
        {
            if (m_hasLock)
                return;

            try
            {
                //m_spin.TryEnter(ref m_hasLock);
                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //if (!m_hasLock)
                //    return;
                TryLock();
                //Monitor.Enter(m_lock);
                lock (m_lock)
                {
                    if (m_inModalSizeLoop)
                    {
                        Core.Logger.LogDebug("Modal Loop Painter HERE!!!");
                        Core.Logger.LogDebug($"---Client Area: {m_window.ClientSize.Width}, {m_window.ClientSize.Height}");
                    }

                    try
                    {
                        m_hdc = BeginPaint(m_window.Handle.Pointer, out m_paintStruct);
                        CheckOperation(m_hdc != IntPtr.Zero);
                        m_memHdc = CreateCompatibleDC(m_hdc);
                        CheckOperation(m_memHdc != IntPtr.Zero);
                        m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
                        CheckOperation(m_bmpPtr != IntPtr.Zero);
                        //m_bmp = Image.FromHbitmap(m_bmpPtr);
                        SetMapMode(m_hdc, MapModes.MM_ANISOTROPIC);
                        //SetWindowExtEx(m_hdc, 100, 100, null);
                        SetViewportOrgEx(m_hdc,
                                         m_window.ClientArea.Left,
                                         m_window.ClientArea.Top,
                                         null);
                        SetViewportOrgEx(m_memHdc,
                                         m_window.ClientArea.Left,
                                         m_window.ClientArea.Top,
                                         null);
                        //SetViewportExtEx(m_hdc,
                        //                 m_window.ClientArea.Right,
                        //                 m_window.ClientArea.Bottom,
                        //                 null);
                    }
                    catch (Win32Exception winEx)
                    {
                        if (m_hdc != IntPtr.Zero && winEx.NativeErrorCode != 1400)
                            throw;
                    }

                    Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendering, this);
                    //Monitor.Exit(m_lock);
                }
            }
            finally
            {
                if (m_hasLock)
                {
                    //m_semaphore.Release();
                    //if (m_spin.IsHeldByCurrentThread)
                    //    m_spin.Exit();
                    m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
        }

        protected override bool PlatformValidate()
        {
            return ValidateRect(m_window.Handle.Pointer, IntPtr.Zero);
        }

        protected void SetBuffer()
        {
            if (!m_bufferChanged ^ m_hasLock)
                return;

            try
            {

                //m_spin.TryEnter(ref m_hasLock);
                //if (!m_hasLock)
                //    return;
                TryLock();
                //Monitor.Enter(m_lock);
                lock (m_lock)
                {
                    var pb = m_buffer.PixelBuffer;
                    var pBytes = pb.Contents;

                    if (pBytes.Length <= 0)
                        return;

                    var bitCount = SetBitsToBitmap(m_memHdc,
                                                   m_window.ClientSize.Width,
                                                   m_window.ClientSize.Height,
                                                   pBytes,
                                                   out var bmi,
                                                   m_bmpPtr);

                    CheckOperation(bitCount != 0);
                    CheckOperation(SelectObject(m_memHdc, m_bmpPtr));
                    SetStretchBltMode(m_memHdc, StretchBltMode.STRETCH_DELETESCANS);
                    //CheckLastError();
                    //CheckOperation(SetRgbBitsToDevice(m_memHdc,
                    //                                  m_window.ClientArea.Width,
                    //                                  m_window.ClientArea.Height, pb.Handle.Pointer,
                    //                                  0, 0, 0, 0));
                    //
                    // BitBlt(m_hdc, 0, 0, rec.Width, -rec.Height, m_memHdc, 0, 0, BitBltFlags.SRCCOPY);
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
                    //Monitor.Exit(m_lock);
                }
            }
            finally
            {
                if (m_hasLock)
                {
                    //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                    //if (m_spin.IsHeldByCurrentThread)
                    //    m_spin.Exit();
                    m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
        }

        private IntPtr OnGetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            //Core.Logger.LogDebug($"WINDOWS HOOK MESSAGE: {message.Id}");
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    Render();
                    //res = new IntPtr(1);
                    break;
                case WindowsMessageIds.ERASEBKGND:
                case WindowsMessageIds.MOVING:
                case WindowsMessageIds.MOVE:
                case WindowsMessageIds.SIZE:
                case WindowsMessageIds.WINDOWPOSCHANGED:
                case WindowsMessageIds.WINDOWPOSCHANGING:
                case WindowsMessageIds.ENTERSIZEMOVE:
                case WindowsMessageIds.EXITSIZEMOVE:
                    break;
                case WindowsMessageIds.TIMER:
                    if (m_inModalSizeLoop)
                    {
                        Invalidate();
                        SetViewportOrgEx(m_hdc,
                                         m_window.ClientArea.Left,
                                         m_window.ClientArea.Top,
                                         null);
                        //SetViewportExtEx(m_hdc, 
                        //                 m_window.ClientArea.Right, 
                        //                 m_window.ClientArea.Bottom, 
                        //                 null);
                        Render();
                    }

                    break;
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

        private IntPtr OnRetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            //Core.Logger.LogDebug($"WINDOWS HOOK MESSAGE: {message.Id}");
            switch (message.Id)
            {
                case WindowsMessageIds.ERASEBKGND:
                    break;
                    //return new IntPtr(1);
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
                    SetTimer(m_window.Handle.Pointer, new IntPtr(12345), 33, null);
                    break;
                case WindowsMessageIds.EXITSIZEMOVE:
                    m_inModalSizeLoop = false;
                    KillTimer(m_window.Handle.Pointer, new IntPtr(12345));
                    break;
                case WindowsMessageIds.TIMER:
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
                m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
                CheckOperation(m_bmpPtr != IntPtr.Zero);
            }
            catch (Win32Exception winEx)
            {
                if (m_hdc != IntPtr.Zero && winEx.NativeErrorCode != 1400)
                    throw;
            }
        }

        protected bool TryLock(int maxRetries = 3, int retryDelay = 50, int lockTimeout = 50)
        {
            for (var i = 0; i <= maxRetries; i++)
            {
                if (m_hasLock)
                    Thread.Sleep(retryDelay);

                m_hasLock = m_lockSlim.TryEnterWriteLock(lockTimeout);
                //m_hasLock = true;
                //m_semaphore.Wait();
                if (!m_hasLock)
                    Thread.Sleep(retryDelay);
                else
                    return true;

                //if (m_hasLock)
                //    return true;
                //    return null;
            }

            return false;
        }
        #endregion
    }
}