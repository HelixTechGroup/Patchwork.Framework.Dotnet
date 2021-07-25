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
using Shin.Framework.Extensions;
using Shin.Framework.Threading;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Helpers;
using static Patchwork.Framework.Platform.Interop.Utilities;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class GdiWindowRenderer : NWindowRenderer, INFrameBufferRenderer, INOperatingSystemRenderer
    {
        #region Members
        protected Bitmap m_bmp;
        protected IntPtr m_bmpPtr;
        protected NFrameBuffer m_buffer;
        protected NFrameBuffer m_tmpBuffer;
        //protected WindowsEventHook m_eventHook;
        protected bool m_bufferChanged;
        protected IntPtr m_hdc;
        protected bool m_inModalSizeLoop;
        //protected static readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(0);
        //[ThreadStatic]
        //protected static SpinLock m_spin = new SpinLock();
        protected WindowsProcessHook m_hook;
        protected WindowsProcessHook m_hook2;
        protected IntPtr m_memHdc;
        protected NFrameBuffer m_oldBuffer;
        protected PaintStruct m_paintStruct;
        protected IntPtr m_oldBmpPtr;
        #endregion

        public GdiWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
            m_priority = RenderPriority.Highest;
            m_level = RenderStage.Os;
            m_handleRenderLoop = false;
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
            m_buffer = m_oldBuffer = m_tmpBuffer = new NFrameBuffer();
            //m_eventHook.Initialize();
        }

        /// <inheritdoc />
        protected override void OnSizeChanged(object sender, PropertyChangedEventArgs<Size> e)
        {
            Invalidate();
        }

        protected override void OnProcessMessage(IPlatformMessage message)
        {
            switch (message.Id)
            {
                case MessageIds.Window:
                    var dataW = message.RawData as IWindowMessageData;
                    switch (dataW?.MessageId) 
                    {
                        case WindowMessageIds.Created:
                            if (dataW.Window.Equals(m_window))
                            {
                                //CreateHDC(m_window.Handle.Pointer);
                                //Invalidate();
                                
                                //OsRender();
                            }
                            break;
                        //case WindowMessageIds.Destroyed:
                        //    if (dataW.Window.Equals(m_window))
                        //    {
                        //        DestroyHDC(m_window.Handle.Pointer);
                        //        //Invalidate();
                        //        //OsRender();
                        //    }
                        //    break;
                        default:
                            break;
                    }
                    break;
                case MessageIds.Rendering:
                    //Core.Logger.LogDebug("Found Rendering Messages.");
                    var data = message.RawData as IRenderMessageData;
                    switch (data?.MessageId)
                    {
                        case RenderMessageIds.None:
                            break;
                        case RenderMessageIds.SetFrameBuffer:
                            //if (m_hasLock)
                            //    return;

                            if (!m_isInitialized)
                                break;

                            lock (m_lock)
                            {
                                if (data.FrameBuffer is null || m_tmpBuffer.Equals(data.FrameBuffer))
                                    break;

                                //m_oldBuffer?.Dispose();
                                //m_oldBuffer = m_buffer.Copy();
                                //m_buffer.Dispose();
                                //m_buffer = data.FrameBuffer.Copy();
                                //m_oldBuffer = m_buffer;
                                //m_tmpBuffer?.Dispose();
                                m_tmpBuffer = data.FrameBuffer;
                                
                                m_bufferChanged = true;
                                //Invalidate();
                                //Render();
                                //data.FrameBuffer.Dispose();
                            }

                            //Invalidate();
                            break;
                    }

                    break;
            }
        }

        // The function that defines the surface we are drawing.
        private double F(double x, double z)
        {
            const double two_pi = 2 * 3.14159265;
            double r2 = x * x + z * z;
            double r = Math.Sqrt(r2);
            double theta = Math.Atan2(z, x);
            return Math.Exp(-r2) * Math.Sin(two_pi * r) * Math.Cos(3 * theta);
        }

        private void MapRainbowColor(double value,
                                     double min_value,
                                     double max_value,
                                     out byte red,
                                     out byte green,
                                     out byte blue)
        {
            // Convert into a value between 0 and 1023.
            int int_value = (int)(1023 * (value - min_value) / (max_value - min_value));

            // Map different color bands.
            if (int_value < 256)
            {
                // Red to yellow. (255, 0, 0) to (255, 255, 0).
                red = 255;
                green = (byte)int_value;
                blue = 0;
            }
            else if (int_value < 512)
            {
                // Yellow to green. (255, 255, 0) to (0, 255, 0).
                int_value -= 256;
                red = (byte)(255 - int_value);
                green = 255;
                blue = 0;
            }
            else if (int_value < 768)
            {
                // Green to aqua. (0, 255, 0) to (0, 255, 255).
                int_value -= 512;
                red = 0;
                green = 255;
                blue = (byte)int_value;
            }
            else
            {
                // Aqua to blue. (0, 255, 255) to (0, 0, 255).
                int_value -= 768;
                red = 0;
                green = (byte)(255 - int_value);
                blue = 255;
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
            if (m_inModalSizeLoop)
            {
                Core.Logger.LogDebug("Modal Loop Painter HERE!!!");
                Core.Logger.LogDebug($"---Client Area: {m_window.ClientSize.Width}, {m_window.ClientSize.Height}");
            }

            //if (m_hasLock)
            //    return;

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

            var rec = m_window.ClientArea;
            var brush2 = CreateSolidBrush(ColorTranslator.ToWin32(Color.Orange));
            CheckOperation(FillRect(m_memHdc, ref rec, brush2));
            //DeleteObject(brush2);
            //BitBlt(m_hdc, 0, 0, rec.Width, -rec.Height, m_memHdc, 0, 0, BitBltFlags.SRCCOPY);
            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRender, this);
        }

        /// <inheritdoc />
        protected override void PlatformRendered()
        {
            //if (m_hasLock)
            //    return;

            if (m_inModalSizeLoop)
            {
                Core.Logger.LogDebug("Modal Loop Painter HERE!!!");
                Core.Logger.LogDebug($"---Client Area: {m_window.ClientSize.Width}, {m_window.ClientSize.Height}");
            }

            //SetBuffer(); 
            //CheckOperation(SelectObject(m_memHdc, m_oldBmpPtr));
            var rec = m_window.ClientArea;
            //SetStretchBltMode(m_memHdc, StretchBltMode.STRETCH_DELETESCANS);
            SwapBuffers();
            
            //BitBlt(m_hdc, 0, 0, rec.Width, -rec.Height, m_memHdc, 0, 0, BitBltFlags.SRCCOPY);
            //try
            //{
            //m_hasLock = m_semaphore.Wait(m_lockTimeout);
            //m_hasLock = false;
            //m_spin.TryEnter(ref m_hasLock);
            //if (!m_hasLock)
            //    return;
            //TryLock();
            //Monitor.Enter(m_lock);
            //lock (m_lock)
            //{
            //Core.Dispatcher.InvokeAsync(SetBuffer);
            //Validate();
            //if (GetUpdateRect(m_window.Handle.Pointer, out var rec, false))
            //   Validate(ref rec);

            //DestroyHDC(m_window.Handle.Pointer);
            EndPaint(m_window.Handle.Pointer, ref m_paintStruct);

            
                    //m_oldBuffer?.Dispose();
                   // m_oldBuffer = m_buffer.Copy();
                    //m_buffer?.Dispose();
                    Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendered, this);
                    //Monitor.Exit(m_lock);
            //    }
            //}
            //finally
            //{
            //    if (m_hasLock)
            //    {
            //        //if (m_spin.IsHeldByCurrentThread)
            //        //    m_spin.Exit();
            //        m_lockSlim.ExitWriteLock();
            //        m_hasLock = false;
            //    }
            //}
        }

        protected override void PlatformRendering()
        {
            //if (m_hasLock)
            //    return;

            //try
            //{
                //m_spin.TryEnter(ref m_hasLock);
                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //if (!m_hasLock)
                //    return;
                //TryLock();
                //Monitor.Enter(m_lock);
                //lock (m_lock)
                //{
                    if (m_inModalSizeLoop)
                    {
                        Core.Logger.LogDebug("Modal Loop Painter HERE!!!");
                        Core.Logger.LogDebug($"---Client Area: {m_window.ClientSize.Width}, {m_window.ClientSize.Height}");
                    }

                    try
                    {
                        //CheckOperation(SelectObject(m_memHdc, m_bmpPtr));
                        m_hdc = BeginPaint(m_window.Handle.Pointer, out m_paintStruct);
                        m_memHdc = CreateCompatibleDC(m_hdc);
                        if (m_bmpPtr != IntPtr.Zero)
                            CheckOperation(DeleteObject(m_bmpPtr));

                        m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientArea.Width, m_window.ClientArea.Height);
                        CheckOperation(m_bmpPtr != IntPtr.Zero);

                        m_oldBmpPtr = SelectObject(m_memHdc, m_bmpPtr);
                        CheckOperation(m_oldBmpPtr != IntPtr.Zero);

                //CreateHDC(m_window.Handle.Pointer);

                //        CheckOperation(DeleteObject(m_bmpPtr));
                //        m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientArea.Width, m_window.ClientArea.Height);
                //        CheckOperation(m_bmpPtr != IntPtr.Zero);

                //        m_oldBmpPtr = SelectObject(m_memHdc, m_bmpPtr);
                //        CheckOperation(m_oldBmpPtr != IntPtr.Zero);
                //m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
                //CheckOperation(m_bmpPtr != IntPtr.Zero);
                var rec = m_window.ClientArea;

                //CheckOperation(m_hdc != IntPtr.Zero);
                        //m_memHdc = CreateCompatibleDC(m_hdc);
                        //CheckOperation(m_memHdc != IntPtr.Zero);
                        //m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
                        //CheckOperation(m_bmpPtr != IntPtr.Zero);
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
                        //SetViewportExtEx(m_memHdc,
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
            //    }
            //}
            //finally
            //{
            //    if (m_hasLock)
            //    {
            //        //m_semaphore.Release();
            //        //if (m_spin.IsHeldByCurrentThread)
            //        //    m_spin.Exit();
            //        m_lockSlim.ExitWriteLock();
            //        m_hasLock = false;
            //    }
            //}
        }

        protected override bool PlatformValidate()
        {
            return ValidateRect(m_window.Handle.Pointer, IntPtr.Zero);
        }

        protected void SwapBuffers()
        {
            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            try
            {
                var rec = m_window.ClientArea;
                //SetStretchBltMode(m_memHdc, StretchBltMode.STRETCH_DELETESCANS);
                CheckOperation(StretchBlt(m_hdc,
                                          0,
                                          0,
                                          rec.Width,
                                          rec.Height,
                                          m_memHdc,
                                          0,
                                          0,
                                          rec.Width,
                                          rec.Height,
                                          BitBltFlags.SRCCOPY));
                BitBlt(m_memHdc,
                       0,
                       0,
                       rec.Width,
                       rec.Height,
                       IntPtr.Zero,
                       0,
                       0,
                       BitBltFlags.BLACKNESS);
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }

        protected void SetBuffer()
        {
            //!m_bufferChanged ^
            //if ((m_tmpBuffer is null || m_buffer is null) || m_buffer.Equals(m_tmpBuffer))
                //return;

            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            try
            {
                //if (!m_buffer.Equals(m_tmpBuffer))//(m_bufferChanged)
                {
                    //m_oldBuffer?.Dispose();
                    m_oldBuffer = m_buffer;
                    m_buffer = m_tmpBuffer;
                    //m_tmpBuffer?.Dispose();
                    m_bufferChanged = false;
                }

                //m_buffer = GenerateBuffer();

                var pb = m_buffer?.PixelBuffer;
                if (pb?.Contents.Length <= 0)
                    return;

                var bitCount = SetBitsToBitmap(m_memHdc,
                                                m_window.ClientSize.Width,
                                                m_window.ClientSize.Height,
                                                pb?.Contents.ToArray(),
                                                out var bmi,
                                                m_bmpPtr);

                CheckOperation(bitCount != 0);
                //CheckLastError();
                //CheckOperation(SetRgbBitsToDevice(m_memHdc,
                //                                  m_window.ClientArea.Width,
                //                                  m_window.ClientArea.Height, pb.Handle.Pointer,
                //                                  0, 0, 0, 0));
                //
                // BitBlt(m_hdc, 0, 0, rec.Width, -rec.Height, m_memHdc, 0, 0, BitBltFlags.SRCCOPY);
                //CheckOperation(StretchBlt(m_hdc,
                //                          0,
                //                          0,
                //                          m_window.ClientArea.Width,
                //                          m_window.ClientArea.Height,
                //                          m_memHdc,
                //                          0,
                //                          0,
                //                          pb.Width,
                //                          pb.Height,
                //                          BitBltFlags.SRCCOPY));
                m_bufferChanged = false;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }

        private IntPtr OnGetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            //Core.Logger.LogDebug($"WINDOWS HOOK MESSAGE: {message.Id}");
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    OsRender();
                    break;
                case WindowsMessageIds.ERASEBKGND:
                    //Render();
                    res = new IntPtr(1);
                    break;
                case WindowsMessageIds.MOVING:
                case WindowsMessageIds.MOVE:
                case WindowsMessageIds.SIZE:
                case WindowsMessageIds.WINDOWPOSCHANGED:
                case WindowsMessageIds.WINDOWPOSCHANGING:
                case WindowsMessageIds.ENTERSIZEMOVE:
                case WindowsMessageIds.EXITSIZEMOVE:
                    //SetViewportOrgEx(m_hdc,
                    //                 m_window.ClientArea.Left,
                    //                 m_window.ClientArea.Top,
                    //                 null);
                    break;
                case WindowsMessageIds.TIMER:
                    if (m_inModalSizeLoop)
                    {
                        //Invalidate();if (m_bmpPtr == IntPtr.Zero)
                        //m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
                        //CheckOperation(m_bmpPtr != IntPtr.Zero);
                        //SetViewportOrgEx(m_hdc,
                        //                 m_window.ClientArea.Left,
                        //                 m_window.ClientArea.Top,
                        //                 null);
                        //StretchDIBits(m_memHdc,
                        //              0,
                        //              0,
                        //              m_iFrameDestWidth,
                        //              m_iFrameDestHeight,
                        //              0,
                        //              0,
                        //              m_iFrameSourceWidth,
                        //              m_iFrameSourceHeight,
                        //              pBuffer,
                        //              &binfoFrame,
                        //              DIB_RGB_COLORS,
                        //              SRCCOPY);
                        //SetViewportExtEx(m_hdc, 
                        //                 m_window.ClientArea.Right, 
                        //                 m_window.ClientArea.Bottom, 
                        //                 null);
                        //Invalidate();
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
                case WindowsMessageIds.CREATE:
                    CreateHDC(m_window.Handle.Pointer);
                    break;
                case WindowsMessageIds.DESTROY:
                    DestroyHDC(m_window.Handle.Pointer, true);
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
                    res = new IntPtr(1);
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
                    //SetTimer(m_window.Handle.Pointer, new IntPtr(12345), 50, null);
                    //Invalidate();
                    break;
                case WindowsMessageIds.EXITSIZEMOVE:
                    m_inModalSizeLoop = false;
                    //KillTimer(m_window.Handle.Pointer, new IntPtr(12345));
                    //Invalidate();
                    break;
                case WindowsMessageIds.TIMER:
                case WindowsMessageIds.NCCALCSIZE:
                case WindowsMessageIds.CAPTURECHANGED:
                case WindowsMessageIds.NCHITTEST:
                case WindowsMessageIds.GETMINMAXINFO:
                case WindowsMessageIds.NCPAINT:
                    break;
                case WindowsMessageIds.CREATE:
                    CreateHDC(m_window.Handle.Pointer);
                    break;
                case WindowsMessageIds.DESTROY:
                    DestroyHDC(m_window.Handle.Pointer, true);
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
            if (!m_isInitialized || m_isDisposed)
                return;

            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            try
            {
                if (m_hdc == IntPtr.Zero)
                    m_hdc = GetDC(hwnd);
                CheckOperation(m_hdc != IntPtr.Zero);

                if (m_memHdc == IntPtr.Zero)
                    m_memHdc = CreateCompatibleDC(m_hdc);
                CheckOperation(m_memHdc != IntPtr.Zero);

                if (m_bmpPtr == IntPtr.Zero)
                    m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientArea.Width, m_window.ClientArea.Height);
                CheckOperation(m_bmpPtr != IntPtr.Zero);
            }
            catch (Win32Exception winEx)
            {
                if (m_hdc != IntPtr.Zero && winEx.NativeErrorCode != 1400)
                    throw;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }

        private void DestroyHDC(IntPtr hwnd, bool delete = false)
        {
            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            try
            {
                if (m_hdc != IntPtr.Zero)
                {
                    ReleaseDC(hwnd, m_hdc);

                    if (delete)
                        m_hdc = IntPtr.Zero;
                }

                if (m_memHdc != IntPtr.Zero)
                {
                    if (delete)
                    {
                        ReleaseDC(hwnd, m_memHdc);
                        CheckOperation(DeleteDC(m_memHdc));
                        m_memHdc = IntPtr.Zero;
                    }
                }

                if (m_bmpPtr != IntPtr.Zero)
                {
                    if (delete)
                    {
                        CheckOperation(DeleteObject(m_bmpPtr));
                        m_bmpPtr = IntPtr.Zero;
                    }
                }
            }
            catch (Win32Exception winEx)
            {
                //if (m_hdc != IntPtr.Zero && winEx.NativeErrorCode != 1400)
                    throw;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }
        #endregion

        /// <inheritdoc />
        public event EventHandler OsRendered;

        /// <inheritdoc />
        public event EventHandler OsRendering;

        /// <inheritdoc />
        public void OsRender()
        {
            //^ m_isValid

            if (!m_isEnabled ^ m_isRendering ^ !m_isInitialized ^ m_isDisposed)
                return;

            if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
                return;

            try
            {
                CheckEnabled();

                m_isRendering = true;
                PlatformRendering();
                OsRendering.Raise(this, null);
                PlatformRender();
                PlatformRendered();
                OsRendered.Raise(this, null);
                m_isRendering = false;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }

            ///CheckEnabled();
        }

        private NFrameBuffer GenerateBuffer()
        {
            var rec = m_window.ClientArea;

            var b = new NFrameBuffer(rec.Width, rec.Height);
            var tmpb = new byte[b.PixelBuffer.Length];
            // Set the pixel colors.
            double[,] values = new double[rec.Width, rec.Height];
            for (int ix = 0; ix < rec.Width; ix++)
            {
                double x = 0 + ix;
                for (int iz = 0; iz < rec.Height; iz++)
                {
                    double z = 0 + iz;
                    values[ix, iz] = F(x, z);
                }
            }

            for (int ix = 0; ix < rec.Width; ix++)
            {
                for (int iz = 0; iz < rec.Height; iz++)
                {
                    byte red, green, blue;
                    MapRainbowColor(values[ix, iz],
                                    0,
                                    rec.Height,
                                    out red,
                                    out green,
                                    out blue);
                    b.PixelBuffer.SetPixel(ix, iz, red, green, blue, 255, ref tmpb);
                }
            }

            b.PixelBuffer.SetContent(tmpb);
            return b;
        }

        public new void Render()
        {
            if (!m_isEnabled ^ m_isRendering ^ !m_isInitialized ^ m_isDisposed)
                return;

            if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
                return;

            Core.Logger.LogDebug("---GDI Render Loop.");

            try
            {
                CheckEnabled();

                m_isRendering = true;
                //Core.MessagePump.PushFrameBuffer(this, GenerateBuffer());
                //m_bufferChanged = true;
                m_hdc = GetDC(m_window.Handle.Pointer);
                m_memHdc = CreateCompatibleDC(m_hdc);
                //CreateHDC(m_window.Handle.Pointer);
                if (m_bmpPtr != IntPtr.Zero)
                    CheckOperation(DeleteObject(m_bmpPtr));

                m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientArea.Width, m_window.ClientArea.Height);
                CheckOperation(m_bmpPtr != IntPtr.Zero);

                m_oldBmpPtr = SelectObject(m_memHdc, m_bmpPtr);
                CheckOperation(m_oldBmpPtr != IntPtr.Zero);
                //PlatformRender();
                SetBuffer();
                SwapBuffers();
                //ReleaseDC(m_window.Handle.Pointer, m_hdc);
                DestroyHDC(m_window.Handle.Pointer, true);
                //Invalidate();
                //base.Render();
                m_isRendering = false;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }
    }
}