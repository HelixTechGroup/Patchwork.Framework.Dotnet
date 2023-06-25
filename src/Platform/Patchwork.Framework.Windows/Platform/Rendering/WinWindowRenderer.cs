#region Usings
//using Patchwork.Framework.Platform.Interop.GdiPlus;
//using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;
using System;
using System.Drawing;
using System.Threading;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;
using Shin.Framework.Threading;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Utilities;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class WinWindowRenderer : NWindowRenderer, 
                                     INOperatingSystemRenderer, 
                                     INSwapChainRenderer
    {
        #region Events
        /// <inheritdoc />
        public event EventHandler OsRendered;

        /// <inheritdoc />
        public event EventHandler OsRendering;
        #endregion

        #region Members
        //protected static NFrameBuffer m_buffer;
        //protected static NFrameBuffer m_tmpBuffer;
        //protected D2D1HwndRenderTarget m_renderTarget;
        //protected WindowsEventHook m_eventHook;
        //protected bool m_bufferChanged;
        protected bool m_inModalSizeLoop;
        //protected static readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(0);
        //[ThreadStatic]
        //protected static SpinLock m_spin = new SpinLock();
        //protected WindowsProcessHook m_hook;
        //protected WindowsProcessHook m_hook2;
        //protected WindowsProcessHook m_hook3;

        //protected static NFrameBuffer m_oldBuffer;
        protected bool m_isOsRender;
        private INSwapChain m_swapchain;
        #endregion

        public WinWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
            m_priority = RenderPriority.Highest;
            m_level = RenderStage.Hal;
            m_ownsRenderLoop = true;
            //m_hook = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_GETMESSAGE);
            //m_hook.ProcessMessage += OnGetMsg;
            //m_hook2 = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_CALLWNDPROC);
            //m_hook2.ProcessMessage += OnRetMsg;
            //m_hook3 = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_CALLWNDPROCRET);
            //m_hook3.ProcessMessage += OnProcRet;

            //m_eventHook = new WindowsEventHook(window as IWindowsProcess, SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE);
            //m_eventHook.ProcessEvent += OnEventMsg;
            //m_oldBuffer = 
            //renderDevice.ProcessMessage += OnProcessMessage;
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

            //var rect = m_window.ClientArea;
            var ret = InvalidateRect(m_window.Handle.Pointer, IntPtr.Zero, shouldErase);
            //if (ret) 
            //    Core.Dispatcher.InvokeAsync(() => UpdateWindow(m_window.Handle.Pointer));
            //CheckLastError();

            return ret;
            //return true;
        }

        /// <inheritdoc />
        public void OsRender()
        {
            //^ m_isValid
            if (!m_isEnabled ^ m_isRendering ^ !m_isInitialized ^ m_isDisposed)
                return;

            if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
                return;

            lock(m_lock)
            {
                try
                {
                    m_isRendering = true;

                    PlatformRendering();
                    Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendering, this);
                    OsRendering.Raise(this, null);
                    PlatformRender();
                    Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRender, this);
                    PlatformRendered();
                    Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendered, this);
                    OsRendered.Raise(this, null);
                }
                finally
                {
                    m_lockSlim.TryExit(SynchronizationAccess.Write);
                    m_isRendering = false;
                }
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
            return ReferenceEquals(this, obj) || obj is WinWindowRenderer other && Equals(other);
        }

        public bool Equals(WinWindowRenderer obj)
        {
            return Equals(obj.Window.Handle, m_window.Handle);
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            //m_hook.Dispose();
            //m_hook2.Dispose();
            //m_hook3.Dispose();
            //m_renderTarget.Dispose();
            //m_surface?.Dispose();

            //m_eventHook.Dispose();

            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            //DeleteObject(m_brush);

            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            //m_hook.Initialize();
            //m_hook2.Initialize();
            //m_hook3.Initialize();
            m_size = m_window.ClientSize;
            ((INSwapChainDevice)m_device)?.Create();
            m_swapchain = ((INSwapChainDevice)m_device)?.CreateSwapChain(m_window);

            //m_buffer = m_oldBuffer = m_tmpBuffer = new NFrameBuffer(m_size.Width, m_size.Height);
            //m_renderTarget = //m_device.GetResource<D2D1HwndRenderTarget>();
            //m_renderTarget.SetWindow(m_window);
            //m_brush = m_device.Create<D2D1SolidBrush>(Color.Green);

            //m_renderTarget = m_device.Adapter.Factory.CreateResource<D2D1HwndRenderTarget>(m_window);
            //m_surface.Create(m_device, m_window);
            //m_brush = CreateSolidBrush(ColorTranslator.ToWin32(Color.Orange));
            //m_eventHook.Initialize();
            //m_window.SizeChanged += OnSizeChanged;
            //CheckOperation(UpdateWindow(m_window.Handle.Pointer));
            Invalidate();
            //m_bufferChanged = true;
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
                                Invalidate();

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
                    }

                    break;
                case MessageIds.Rendering:
                    //Core.Logger.LogDebug("Found Rendering Messages.");
                    var data = message.RawData as IRenderMessageData;
                    switch (data?.MessageId)
                    {
                        case RenderMessageIds.None:
                            break;
                    }

                    break;
            }
        }

        /// <inheritdoc />
        protected override void OnSizeChanged(object sender, PropertyChangedEventArgs<Size> e)
        {
            //if (!m_renderTarget.IsCreated)
            //{
            //    return;
            //if (!m_renderTarget.IsDisposed)
            //    m_renderTarget.Dispose();

            //m_renderTarget.Create();
            //}

            //Core.Logger.LogDebug($"=== D2D1 window changed event ===");
            //Core.Logger.LogDebug($"---Window Client Area: {m_window.ClientSize}");
            ////Core.Logger.LogDebug(@$"---D2D1 RenderTarget Size: {m_device..Resource.Size}");
            //Core.Logger.LogDebug($"---Renderer Client Area: {m_size}");
            //Core.Logger.LogDebug($"---Event Client Area: {e.RequestedValue}");

            //var state = m_renderTarget.Resource.CheckWindowState();
            //switch (state)
            //{
            //    case WindowState.None:
            //        return;
            //}

            //m_size = m_window.ClientSize;
            //m_size = new Size((int)(e.CurrentValue.Width*96), 
            //                  (int)(e.CurrentValue.Height*96));
            lock(m_lock)
            {
                m_size = e.RequestedValue;
            //    ((INContextDevice)m_device).CurrentContext?.Resize(m_size);
            }
            //var rec = new Rectangle(m_window.Position, m_size);

            //try
            //{
            ////    //m_renderTarget.Resource.Flush(out var tag1, out var tag2);
            //    m_renderTarget.Resource.Resize(m_size);
            //}
            //catch (SharpGenException sgEx)
            //{
            //    var res = Result.GetResultFromException(sgEx);
            //    Core.Logger.LogDebug(@$"SharpGenException: {sgEx.ResultCode}");
            //    Core.Logger.LogDebug(@$"Result: {res.Code}");
            //}
            //finally
            {
                //m_device.Context.CurrentContext.Resize(m_window, e.RequestedValue);
                //Invalidate();
            }

            //Invalidate();
            //CheckOperation(UpdateWindow(m_window.Handle.Pointer));
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

            //var rec = m_renderTarget.Resource.Size;
            //var brush = m_renderTarget.Resource.CreateSolidColorBrush(Color.Green.ToColor4());
            //var borderBrush = m_renderTarget.Resource.CreateSolidColorBrush(Color.DarkMagenta.ToColor4());
            ////brush.Color = new Color4(new Vortice.Mathematics.Color(Color.Green.ToArgb()));
            //var recsize = 200f;
            //var recf = new RectF(rec.Width / 2f + recsize,
            //                     rec.Height / 2f + recsize,
            //                     rec.Width / 2f - recsize,
            //                     rec.Height / 2f - recsize);
            //m_renderTarget.Resource.FillRectangle(recf, brush);
            //m_renderTarget.Resource.DrawRectangle(recf,
            //                                      borderBrush);
            //brush.Dispose();
            //borderBrush.Dispose();
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
            var rec = m_window.ClientArea;
            //SwapBuffers();

            ((INContextDevice)m_device).CurrentContext?.End();
            ((INContextDevice)m_device).CurrentContext?.Flush();
            //((INContextDevice)m_device).CurrentContext?.Resize(m_size);
            //((INContextDevice)m_device).CurrentContext?.Flush();
            //var res = m_renderTarget.Resource.EndDraw();
            //if (res != Result.Ok)
            //{
            //    m_renderTarget.Dispose();
            //    Core.Logger.LogDebug($@"---D2D1 RenderTarget.EndDraw Result: {res.Code}");
            //}

            //Core.Logger.LogDebug($@"---D2D1 RenderTarget.EndDraw Result: {res.Code}");
            if (!ValidateRect(m_window.Handle.Pointer, IntPtr.Zero))
                CheckLastError();
        }

        protected override void PlatformRendering()
        {
            if (m_inModalSizeLoop)
            {
                Core.Logger.LogDebug("Modal Loop Painter HERE!!!");
                Core.Logger.LogDebug($"---Client Area: {m_window.ClientSize.Width}, {m_window.ClientSize.Height}");
            }

            //((INContextDevice)m_device).CreateContext(m_window);
            //((INContextDevice)m_device).CurrentContext.Initialize();

            //m_renderTarget.Create();

            ////try
            ////{
            ////    m_renderTarget.Resource.Resize(m_size);
            ////}
            ////catch
            ////{
            ////    //m_renderTarget.Dispose();
            ////m_renderTarget.Create(true);
            ////m_renderTarget.Resource.Resize(m_window.ClientSize);
            ////}

            ////m_renderTarget.Create(true);
            ////m_renderTarget.Resource.Resize(m_window.ClientSize);
            ////var rec = m_renderTarget.Resource.Size;
            ////Core.Logger.LogDebug(@$"D2D1 RenderTarget Size: {m_renderTarget.Resource.Size}");
            ////m_renderTarget.Resource.Resize(m_size);

            //Core.Logger.LogDebug(@$"D2D1 RenderTarget Size: {m_renderTarget.Resource.Size}");
            //m_renderTarget.Resource.BeginDraw();
            //        m_renderTarget.Resource.Transform = Matrix3x2.Identity;
            //m_renderTarget.Resource.Clear(Color.Blue.ToColor4());
            ((INContextDevice)m_device).CurrentContext?.Resize(m_size);
            ((INContextDevice)m_device).CurrentContext.Begin();
            ((INContextDevice)m_device).CurrentContext?.Clear(Color.Blue);
        }

        protected override bool PlatformValidate()
        {
            return true;
            //return ValidateRect(m_window.Handle.Pointer, IntPtr.Zero);
        }

        private IntPtr OnProcRet(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            var hdc = new NHandle();
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
                    //Invalidate();
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
                    //m_device.Context.Bind(m_window);
                    //m_hdc = m_device.Context.Create(m_window);
                    //CreateHDC(m_window.Handle.Pointer);
                    break;
                case WindowsMessageIds.DESTROY:
                    //m_device.Context.Unbind(m_window);
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
                    break;
                case WindowsMessageIds.DESTROY:
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
                    OsRender();
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

        //private D2D1SolidBrush m_brush;
        /// <inheritdoc />
        public INSwapChain SwapChain
        {
            get { return m_swapchain; }
        }
    }
}