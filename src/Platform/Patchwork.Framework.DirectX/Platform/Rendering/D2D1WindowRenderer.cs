#region Usings
//using Patchwork.Framework.Platform.Interop.GdiPlus;
//using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop.Gdi32;
using Patchwork.Framework.Platform.Interop.GdiPlus;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Rendering.Resources;
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
    public class D2D1WindowRenderer : NWindowRenderer, INFrameBufferRenderer, INOperatingSystemRenderer
    {
        #region Members
        protected INHandle m_memHdc;
        protected INHandle m_hdc;
        protected IntPtr m_bmpPtr;
        protected static NFrameBuffer m_buffer;
        protected static NFrameBuffer m_tmpBuffer;
        //protected WindowsEventHook m_eventHook;
        protected bool m_bufferChanged;
        protected bool m_inModalSizeLoop;
        //protected static readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(0);
        //[ThreadStatic]
        //protected static SpinLock m_spin = new SpinLock();
        protected WindowsProcessHook m_hook;
        protected WindowsProcessHook m_hook2;
        protected WindowsProcessHook m_hook3;

        protected GdiSurface m_surface;
        protected static NFrameBuffer m_oldBuffer;
        protected PaintStruct m_paintStruct;
        protected IntPtr m_oldBmpPtr;
        protected bool m_isOsRender;
        private IntPtr m_brush;
        #endregion

        public D2D1WindowRenderer(INRenderDevice<D2D1Adapter> renderDevice, INWindow window) : base(renderDevice, window)
        {
            m_priority = RenderPriority.Highest;
            m_level = RenderStage.Os;
            m_ownsRenderLoop = true;
            m_hook = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_GETMESSAGE);
            m_hook.ProcessMessage += OnGetMsg;
            m_hook2 = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_CALLWNDPROC);
            m_hook2.ProcessMessage += OnRetMsg;
            m_hook3 = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_CALLWNDPROCRET);
            m_hook3.ProcessMessage += OnProcRet;

            //m_eventHook = new WindowsEventHook(window as IWindowsProcess, SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE);
            //m_eventHook.ProcessEvent += OnEventMsg;
            //m_oldBuffer = 
            renderDevice.ProcessMessage += OnProcessMessage;
        }

        #region Methods
        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_hook.Initialize();
            m_hook2.Initialize();
            m_hook3.Initialize();
            m_size = m_window.ClientSize;
            m_buffer = m_oldBuffer = m_tmpBuffer = new NFrameBuffer(m_size.Width, m_size.Height);
            m_surface = m_device.Adapter.CreateResource<GdiSurface>(m_device, m_window);
            //m_surface.Create(m_device, m_window);
            m_brush = CreateSolidBrush(ColorTranslator.ToWin32(Color.Orange));
            //m_eventHook.Initialize();

            CheckOperation(UpdateWindow(m_window.Handle.Pointer));
            Invalidate();
            //m_bufferChanged = true;
        }

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
            m_hook3.Dispose();
            m_surface?.Dispose();
            
            //m_eventHook.Dispose();

            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            DeleteObject(m_brush);

            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        protected override void OnSizeChanged(object sender, PropertyChangedEventArgs<Size> e)
        {
            Core.Logger.LogDebug($"=== GDI window changed event ===");
            Core.Logger.LogDebug($"---Window Client Area: {m_window.ClientSize.Width}, {m_window.ClientSize.Height}");
            Core.Logger.LogDebug($"---GDI Client Area: {m_surface.Size.Width}, {m_surface.Size.Height}");
            Core.Logger.LogDebug($"---Renderer Client Area: {m_size.Width}, {m_size.Height}");
            Core.Logger.LogDebug($"---Event Client Area: {e.CurrentValue.Width}, {e.CurrentValue.Height}");

            m_size = e.CurrentValue;
            var rec = new Rectangle(m_window.Position, m_size);
            Invalidate(ref rec, true);
            //CheckOperation(UpdateWindow(m_window.Handle.Pointer));
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
                                //OnSizeChanged(this, new PropertyChangedEventArgs<Size>(m_window.ClientSize, m_window.ClientSize, m_window.ClientSize));

                                //UpdateWindow(m_window.Handle.Pointer);
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
                                if (data.FrameBuffer is null)// || m_tmpBuffer.Equals(data.FrameBuffer))
                                    break;

                                //m_oldBuffer?.Dispose();
                                //m_oldBuffer = m_buffer.Copy();
                                //m_buffer.Dispose();
                                //m_buffer = data.FrameBuffer.Copy();
                                //m_oldBuffer = m_buffer;
                                //m_tmpBuffer?.Dispose();
                                m_tmpBuffer = data.FrameBuffer;
                                
                                m_bufferChanged = true;
                                Invalidate();
                                //UpdateWindow(m_window.Handle.Pointer);
                                //Render();
                                //data.FrameBuffer.Dispose();
                            }

                            //Invalidate();
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

            var rec = m_surface.RenderArea;
            CheckOperation(FillRect(m_memHdc.Pointer, ref rec, m_brush));

            //DeleteObject(brush2);
            //BitBlt(m_hdc, 0, 0, rec.Width, -rec.Height, m_memHdc, 0, 0, BitBltFlags.SRCCOPY);
            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRender, this);
        }

        /// <inheritdoc />
        protected override void  PlatformRendered()
        {
            //if (m_hasLock)
            //    return;

            if (m_inModalSizeLoop)
            {
                Core.Logger.LogDebug("Modal Loop Painter HERE!!!");
                Core.Logger.LogDebug($"---Client Area: {m_window.ClientSize.Width}, {m_window.ClientSize.Height}");
            }

            //SetBuffer();
            var rec = m_window.ClientArea;
            //SetStretchBltMode(m_hdc.Pointer, StretchBltMode.STRETCH_HALFTONE);

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
            //if (m_memHdc != IntPtr.Zero)
            //    DeleteDC(m_memHdc);

            SwapBuffers();

            //var p1 = new Point(0);
            //var p2 = new Point(rec.Left, rec.Top);
            //var bf = new BlendFunction()
            //{
            //    BlendOp = 0,
            //    BlendFlags = 0,
            //    SourceConstantAlpha = byte.MaxValue,
            //    AlphaFormat = AlphaFormat.AC_SRC_ALPHA
            //};

            //UpdateLayeredWindow(m_window.Handle.Pointer,
            //                    m_hdc.Pointer,
            //                    ref p1,
            //                    ref m_size,
            //                    m_memHdc.Pointer,
            //                    ref p2,
            //                    0,
            //                    ref bf,
            //                    2);

            if (m_oldBmpPtr  != IntPtr.Zero)
                CheckOperation(SelectObject(m_memHdc.Pointer, m_oldBmpPtr));
            //m_device.UnbindWindow(m_window);
            //DestroyHDC(m_window.Handle.Pointer, true);
            //ReleaseDC(m_window.Handle.Pointer, m_memHdc);

            CheckOperation(m_memHdc.Pointer != IntPtr.Zero);
            DeleteDC(m_memHdc.Pointer);
            m_surface.Dispose();
            if (!m_isOsRender)
                m_device.Context.Destroy(m_window);

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
                        //m_memHdc = CreateCompatibleDC(m_hdc);
                        //if (m_bmpPtr != IntPtr.Zero)
                        //    CheckOperation(DeleteObject(m_bmpPtr));

                        if (!m_isOsRender)
                            m_hdc = m_device.Context[m_window];

                        if (m_hdc.Pointer != IntPtr.Zero)
                            m_memHdc = m_device.Context.Clone(m_window);

                        SetStretchBltMode(m_hdc.Pointer, StretchBltMode.STRETCH_HALFTONE);
                        CheckOperation(SetBrushOrgEx(m_hdc.Pointer, 0, 0, null));
                SetStretchBltMode(m_memHdc.Pointer, StretchBltMode.STRETCH_HALFTONE);
                        CheckOperation(SetBrushOrgEx(m_memHdc.Pointer, 0, 0, null));

                //m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientArea.Width, m_window.ClientArea.Height);
                CheckOperation(m_surface.Handle.Pointer != IntPtr.Zero);

                        //if (m_surface is null || m_surface.IsDisposed || m_surface.Handle is null || m_surface.Handle.IsDisposed)
                        //    m_surface = m_device.Adapter.CreateResource<GdiSurface>(m_device, m_window);

                        //m_surface.Create(m_device, m_window);
                        CheckOperation(m_memHdc.Pointer != IntPtr.Zero);

                        //CreateHDC(m_window.Handle.Pointer);
                        m_oldBmpPtr = SelectObject(m_memHdc.Pointer, m_surface.Handle.Pointer);
                //CheckOperation(m_oldBmpPtr != IntPtr.Zero);

                //CreateHDC(m_window.Handle.Pointer);

                //        CheckOperation(DeleteObject(m_bmpPtr));
                //        m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientArea.Width, m_window.ClientArea.Height);
                //        CheckOperation(m_bmpPtr != IntPtr.Zero);

                //        m_oldBmpPtr = SelectObject(m_memHdc, m_bmpPtr);
                //        CheckOperation(m_oldBmpPtr != IntPtr.Zero);
                //m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
                //CheckOperation(m_bmpPtr != IntPtr.Zero);
                var rec = m_surface.RenderArea;//m_window.ClientArea;

                        //CheckOperation(m_hdc != IntPtr.Zero);
                                //m_memHdc = CreateCompatibleDC(m_hdc);
                                //CheckOperation(m_memHdc != IntPtr.Zero);
                                //m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
                                //CheckOperation(m_bmpPtr != IntPtr.Zero);
                                //m_bmp = Image.FromHbitmap(m_bmpPtr);
                        SetMapMode(m_hdc.Pointer, MapModes.MM_ANISOTROPIC);
                        //SetWindowExtEx(m_hdc, 100, 100, null);
                        //SetViewportOrgEx(m_hdc,
                        //                 ,
                        //                ,
                        //                 null);
                        //SetViewportOrgEx(m_memHdc,
                        //                 ,
                        //                 ,
                        //                 null);
                        SetViewportExtEx(m_hdc.Pointer,
                                         rec.Right,
                                         rec.Bottom,
                                         null);
                        SetViewportExtEx(m_memHdc.Pointer,
                                         rec.Right,
                                         rec.Bottom,
                                         null);
                    }
                    catch (Win32Exception winEx)
                    {
                        if (m_hdc.Pointer != IntPtr.Zero && winEx.NativeErrorCode != 1400)
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
                CheckOperation(m_hdc.Pointer != IntPtr.Zero);
                CheckOperation(m_memHdc.Pointer != IntPtr.Zero);
                CheckOperation(StretchBlt(m_hdc.Pointer,
                                          0,
                                          0,
                                          rec.Width,
                                          rec.Height,
                                          m_memHdc.Pointer,
                                          0,
                                          0,
                                          rec.Width,
                                          rec.Height,
                                          BitBltFlags.SRCCOPY));

                //BitBlt(m_hdc.Pointer,
                //       0,
                //       0,
                //       rec.Width,
                //       rec.Height,
                //       m_memHdc.Pointer,
                //       0,
                //       0,
                //       BitBltFlags.SRCCOPY);
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
                //m_tmpBuffer = GenerateBuffer();

                if (!m_buffer.Equals(m_tmpBuffer))//(m_bufferChanged)
                {
                    //m_oldBuffer?.Dispose();
                    m_oldBuffer = m_buffer;
                    m_buffer = m_tmpBuffer;
                    //m_tmpBuffer?.Dispose();
                    m_bufferChanged = false;
                }


                var pb = m_buffer?.PixelBuffer;
                if (pb is null || pb?.Contents.Length <= 0)
                    return;

                //var bitCount = SetBitsToBitmap(m_memHdc,
                //                                m_window.ClientSize.Width,
                //                                m_window.ClientSize.Height,
                //                                pb?.Contents.ToArray(),
                //                                out var bmi,
                //                                m_bmpPtr);

                //CheckOperation(bitCount != 0);

                //var rec = m_window.ClientArea;
                //var bi = new BitmapInfoHeader
                //         {
                //             Size = (uint)Marshal.SizeOf<BitmapInfoHeader>(),
                //             Width = rec.Width,
                //             Height = -rec.Height,
                //             CompressionMode = BitmapCompressionMode.BI_RGB,
                //             BitCount = 32,
                //             Planes = 1
                //         };

                //unsafe
                //{
                    var contents = pb.Contents.ToArray();
                //    fixed (byte* ptr = &contents)
                //    {
                //GCHandle pinnedArray = GCHandle.Alloc(contents, GCHandleType.Pinned);
                //IntPtr ptr = pinnedArray.AddrOfPinnedObject();

                var retry = false;
                do
                {
                    for (var i = 0; i <= 5; i++)
                    {
                        try
                        {
                            var bitCount = SetBitsToBitmap(m_memHdc.Pointer,
                                                            pb.Width,
                                                            pb.Height,
                                                            contents,
                                                            out var bmi,
                                                            m_surface.Handle.Pointer);

                            //var bitCount = SetBitsToBitmap(m_memHdc,
                            //                               pb.Width,
                            //                               pb.Height,
                            //                               (IntPtr)ptr,
                            //                               out var bmi,
                            //                               m_bmpPtr);

                            //CheckOperation(bitCount != 0);
                            if (bitCount == 0)
                            {
                                m_device.Context.Destroy(m_window);
                                var e = (int)GetLastError();
                                Core.Logger.LogDebug($@"Win32 Error Code: {e}");

                                ReleaseDC(m_window.Handle.Pointer, m_memHdc.Pointer);
                                e = (int)GetLastError();
                                Core.Logger.LogDebug($@"Win32 Error Code: {e}");

                                m_device.Context.Create(m_window);
                                e = (int)GetLastError();
                                Core.Logger.LogDebug($@"Win32 Error Code: {e}");

                                m_memHdc = m_device.Context.Clone(m_window);
                                e = (int)GetLastError();
                                Core.Logger.LogDebug($@"Win32 Error Code: {e}");

                                //DestroyHDC(m_window.Handle.Pointer, true);
                                //CreateHDC(m_window.Handle.Pointer);
                                m_surface.Create(m_device, m_window);
                                e = (int)GetLastError();
                                Core.Logger.LogDebug($@"Win32 Error Code: {e}");
                                //m_oldBmpPtr = SelectObject(m_memHdc.Pointer, m_surface.Handle.Pointer);
                                //CheckOperation(m_oldBmpPtr != IntPtr.Zero);

                                if (i < 5)
                                    retry = true;

                                continue;
                            }

                            //CheckOperation(bitCount != pb.Length);
                            retry = false;
                            break;
                        }
                        catch (Win32Exception wex)
                        {
                            if (wex.NativeErrorCode == 1425 || wex.NativeErrorCode == 6)
                            {
                                m_device.Context.Destroy(m_window);
                                ReleaseDC(m_window.Handle.Pointer, m_memHdc.Pointer);

                                m_device.Context.Create(m_window);
                                m_memHdc = m_device.Context.Clone(m_window);

                                //DestroyHDC(m_window.Handle.Pointer, true);
                                //CreateHDC(m_window.Handle.Pointer);
                                m_surface.Create(m_device, m_window);
                                //m_oldBmpPtr = SelectObject(m_memHdc.Pointer, m_surface.Handle.Pointer);
                                //CheckOperation(m_oldBmpPtr != IntPtr.Zero);

                                if (i < 5)
                                    retry = true;
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                } while (retry);

                //var bitCount = SetDIBitsToDevice(m_memHdc,
                //                                 0,
                //                                 0,
                //                                 (uint)rec.Width,
                //                                 (uint)rec.Height,
                //                                 0,
                //                                 0,
                //                                 0,
                //                                 (uint)rec.Height,
                //                                 (IntPtr)ptr,
                //                                 new IntPtr(&bi),
                //                                 DibBmiColorUsageFlag.DIB_RGB_COLORS);

                //pinnedArray.Free();
                //    }
                //}

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
            catch (Win32Exception wex)
            {
                if (wex.NativeErrorCode != 122)
                    throw;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }

        private IntPtr OnProcRet(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            var hdc = new NHandle();
            //Core.Logger.LogDebug($"WINDOWS HOOK MESSAGE: {message.Id}");
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    hdc = (NHandle)BeginPaint(m_window.Handle.Pointer, out m_paintStruct);
                    OsRender(hdc);
                    EndPaint(m_window.Handle.Pointer, ref m_paintStruct);
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
                    //hdc = m_device.Context[m_window].Pointer;
                    //var prvPnts = new [] { Size.Empty };
                    //SetViewportOrgEx(hdc,
                    //                 m_window.ClientArea.Left,
                    //                 m_window.ClientArea.Top,
                    //                 out var pnts);
                    //SetViewportExtEx(hdc,
                    //                 m_window.ClientArea.Right,
                    //                 m_window.ClientArea.Bottom,
                    //                 prvPnts);
                    Invalidate();
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
                        //Render();
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
                    UpdateWindow(m_window.Handle.Pointer);
                    //m_hdc = m_device.Context.Create(m_window);
                    //CreateHDC(m_window.Handle.Pointer);
                    break;
                case WindowsMessageIds.DESTROY:
                    m_device.Context.Destroy(m_window);
                    //DestroyHDC(m_window.Handle.Pointer, true);
                    break;
            }

            return res;
        }

        private IntPtr OnGetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            var hdc = new NHandle();
            //Core.Logger.LogDebug($"WINDOWS HOOK MESSAGE: {message.Id}");
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    hdc = (NHandle)BeginPaint(m_window.Handle.Pointer, out m_paintStruct);
                    OsRender(hdc);
                    EndPaint(m_window.Handle.Pointer, ref m_paintStruct);
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
                    hdc = m_device.Context[m_window].Pointer;
                    SetViewportOrgEx(hdc,
                                     m_window.ClientArea.Left,
                                     m_window.ClientArea.Top,
                                     out var pnts);
                    Invalidate();
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
                        //Render();
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
                    UpdateWindow(m_window.Handle.Pointer);
                    //m_hdc = m_device.Context.Create(m_window);
                    //CreateHDC(m_window.Handle.Pointer);
                    break;
                case WindowsMessageIds.DESTROY:
                    m_device.Context.Destroy(m_window);
                    //DestroyHDC(m_window.Handle.Pointer, true);
                    break;
            }

            return res;
        }

        private IntPtr OnRetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            var hdc = new NHandle();
            //Core.Logger.LogDebug($"WINDOWS HOOK MESSAGE: {message.Id}");
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    hdc = (NHandle)BeginPaint(m_window.Handle.Pointer, out m_paintStruct);
                    OsRender(hdc);
                    EndPaint(m_window.Handle.Pointer, ref m_paintStruct);
                    break;
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
                    SetTimer(m_window.Handle.Pointer, new IntPtr(123456), 50, null);
                    //Invalidate();
                    break;
                case WindowsMessageIds.EXITSIZEMOVE:
                    m_inModalSizeLoop = false;
                    KillTimer(m_window.Handle.Pointer, new IntPtr(123456));
                    hdc = m_device.Context[m_window].Pointer;
                    SetViewportOrgEx(hdc,
                                     m_window.ClientArea.Left,
                                     m_window.ClientArea.Top,
                                     out var pnts);
                    Invalidate();
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
                    UpdateWindow(m_window.Handle.Pointer);
                    //m_device.Context.Create(m_window);
                    //CreateHDC(m_window.Handle.Pointer);
                    break;
                case WindowsMessageIds.DESTROY:
                    //m_device.Context.Destroy(m_window);
                    //DestroyHDC(m_window.Handle.Pointer, true);
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
        #endregion

        /// <inheritdoc />
        public event EventHandler OsRendered;

        /// <inheritdoc />
        public event EventHandler OsRendering;

        /// <inheritdoc />
        public void OsRender()
        {
            OsRender(new NHandle());
        }

        public void OsRender(INHandle hdc)
        {
            //^ m_isValid
            if (!m_isEnabled ^ m_isRendering ^ !m_isInitialized ^ m_isDisposed)
                return;

            if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
                return;

            try
            {
                CheckEnabled();

                m_isOsRender = (hdc.Pointer != IntPtr.Zero);
                m_isRendering = true;

                if (m_isOsRender)
                    m_hdc = hdc;

                PlatformRendering();
                OsRendering.Raise(this, null);
                PlatformRender();
                PlatformRendered();
                OsRendered.Raise(this, null);
                m_isRendering = false;
                m_isOsRender = false;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }

            ///CheckEnabled();
        }

        //public new void Render() { }
        
        //public new void Render()
        //{
        //    //m_tmpBuffer = GenerateBuffer();
        //    //m_bufferChanged = true;
        //    //Core.MessagePump.PushFrameBuffer(this, GenerateBuffer());
        //    base.Render();
        //}

        //public new void Render()
        //{
        //    //return;

        //    if (!m_isEnabled ^ m_isRendering ^ !m_isInitialized ^ m_isDisposed)
        //        return;

        //    if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
        //        return;

        //    Core.Logger.LogDebug("---GDI Render Loop.");

        //    try
        //    {
        //        CheckEnabled();

        //        m_isRendering = true;
        //        //Core.MessagePump.PushFrameBuffer(this, GenerateBuffer());
        //        //m_bufferChanged = true;
        //        //if (m_hdc)
        //        m_hdc = GetDC(m_window.Handle.Pointer);
        //        m_memHdc = CreateCompatibleDC(m_hdc);

        //        //CreateHDC(m_window.Handle.Pointer);
        //        if (m_bmpPtr != IntPtr.Zero)
        //            CheckOperation(DeleteObject(m_bmpPtr));

        //        m_bmpPtr = CreateCompatibleBitmap(m_hdc, m_window.ClientArea.Width, m_window.ClientArea.Height);
        //        CheckOperation(m_bmpPtr != IntPtr.Zero);

        //        m_oldBmpPtr = SelectObject(m_memHdc, m_bmpPtr);
        //        CheckOperation(m_oldBmpPtr != IntPtr.Zero);
        //        //PlatformRender();
        //        SetBuffer();
        //        SwapBuffers();
        //        //ReleaseDC(m_window.Handle.Pointer, m_hdc);
        //        //ReleaseDC(m_window.Handle.Pointer, m_memHdc);
        //        DestroyHDC(m_window.Handle.Pointer, true);
        //        //Invalidate();
        //        //base.Render();
        //        m_isRendering = false;
        //    }
        //    finally
        //    {
        //        m_lockSlim.TryExit(SynchronizationAccess.Write);
        //    }
        //}

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is GdiWindowRenderer other && Equals(other);
            //return base.Equals(obj);
        }

        public bool Equals(GdiWindowRenderer obj)
        {
            return Equals(obj.Window.Handle, m_window.Handle);
        }
    }
}