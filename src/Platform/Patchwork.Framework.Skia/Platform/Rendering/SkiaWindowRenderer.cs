#region Usings
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Extenstions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;
using Shin.Framework.Runtime;
using SkiaSharp;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class SkiaWindowRenderer : NWindowRenderer
    {
        protected static readonly object m_lock = new object();
        protected readonly ReaderWriterLockSlim m_lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        protected bool m_hasLock = false;
        protected readonly int m_lockTimeout = 100;
        //protected static readonly Mutex m_mutex = new Mutex(true);
        //protected static readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(0);
        protected NFrameBuffer m_buffer;
        protected NFrameBuffer m_oldBuffer;
        protected SKSurface m_surface;
        //protected SKCanvas m_canvas;
        protected SKImage m_map;
        protected SKPixmap m_pixMap;
        protected SafeHandle m_pointer;
        protected bool m_hasRendered;

        /// <inheritdoc />
        public SkiaWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
            m_hasLock = false;
            //m_mutex = new Mutex(true);
        }

        #region Methods
        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_buffer = new NFrameBuffer();
            m_oldBuffer = new NFrameBuffer();
            CreateSurface();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            m_pointer.Dispose();
            //Marshal.Release(m_pointer);
            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            ReleaseSurface();

            base.DisposeManagedResources();
        }

        protected void CreateSurface()
        {
            try
            {
                if (m_hasLock)
                    return;

                //m_hasLock = Monitor.TryEnter(m_lock, m_lockTimeout);
                //m_hasLock = m_mutex.WaitOne(m_lockTimeout);
                //m_hasLock = m_semaphore.AvailableWaitHandle.WaitOne(m_lockTimeout);
                //m_semaphore.WaitAsync();
                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //m_hasLock = m_lockSlim.TryEnterWriteLock(m_lockTimeout);
                //if (!m_hasLock)
                //    return;
                m_hasLock = TryLock();
                //m_hasLock = true;
                lock (m_buffer)
                {
                    m_buffer.EnsureSize(m_window.ClientSize.Width, m_window.ClientSize.Height);
                    var info = new SKImageInfo(m_window.ClientSize.Width, m_window.ClientSize.Height, SKColorType.Bgra8888);
                    m_map = SKImage.Create(info);
                    lock (m_map)
                    {
                        //m_pixMap = new SKPixmap(info, m_buffer.PixelBuffer.Handle.Pointer, m_buffer.PixelBuffer.RowBytes);
                        m_pixMap = m_map.PeekPixels();
                        lock (m_pixMap)
                        {
                            if (!m_hasLock)
                                return;

                            var surface = SKSurface.Create(m_pixMap);
                            //Throw.If(surface is null).InvalidOperationException();
                            
                            m_surface = surface;
                        }
                    }
                }
            }
            finally
            {
                if (m_hasLock)
                {
                    //Monitor.Exit(m_lock);
                    //m_mutex.ReleaseMutex();
                    //m_semaphore.Release();
                    if(m_lockSlim.IsWriteLockHeld)
                        m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
        }

        protected void ReleaseSurface()
        {
            try
            {
                if (m_hasLock || m_surface is null)
                    return;
                //m_hasLock = Monitor.TryEnter(m_lock, m_lockTimeout);
                //m_hasLock = m_mutex.WaitOne(m_lockTimeout);
                // m_hasLock = m_semaphore.AvailableWaitHandle.WaitOne();
                //m_semaphore.WaitAsync();
                //m_hasLock = true;
                //m_semaphore.WaitAsync();
                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //m_hasLock = m_lockSlim.TryEnterWriteLock(m_lockTimeout);
                //if (!m_hasLock)
                //    return;
                m_hasLock = TryLock();
                //lock (m_surface)
                //{
                //    lock (m_pixMap)
                //    {
                //using var pixels = m_pixMap;
                //var p = new SafeMemoryHandle(Marshal.AllocHGlobal(m_pixMap.BytesSize));
                //lock (p)
                //{
                //using var ptr = m_pointer;
                //var pixels = m_map.PeekPixels();
                //using var pixels = m_map.PeekPixels().WithColorType(SKColorType.Rgb888x);

                //m_pixMap.ReadPixels(m_pixMap.Info, p.DangerousGetHandle(), m_pixMap.RowBytes);
                //var ba = new byte[pixels.BytesSize];
                //m_buffer = new NFrameBuffer(pixels.Width, pixels.Height);
                //Marshal.Copy(pixels.GetPixels(), ba, 0, pixels.BytesSize);
                //Marshal.Copy(ba, 0, ptr, pixels.BytesSize);
                //lock (m_buffer)
                //{
                if (!m_hasLock)
                                    return;

                                var snap = m_surface.Snapshot().PeekPixels();
                                m_buffer.SetPixelBuffer(snap.GetPixels()/*m_pixMap.GetPixels()*/,
                                                        snap.Width,
                                                        snap.Height,
                                                        snap.RowBytes,
                                                        snap.BytesSize);
                                Core.MessagePump.PushFrameBuffer(this, m_buffer.Copy());

                                //lock (m_oldBuffer)
                                //{
                                    m_oldBuffer?.Dispose();
                                    m_oldBuffer = m_buffer.Copy();
                                //}
                                m_buffer?.Dispose();
                            //}
                        //}
                        //p.Dispose();
                    //}
                //}
            }
            finally
            {
                m_surface?.Dispose();
                m_pixMap?.Dispose();
                m_map?.Dispose();

                if (m_hasLock)
                {
                    //Monitor.Exit(m_lock);
                    //m_mutex.ReleaseMutex();
                    //m_semaphore.Release();
                    m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e)
        {
            //Invalidate();
            //CreateSurface();
            
            Render();
        }

        /// <inheritdoc />
        protected override bool PlatformInvalidate()
        {
            return true;
        }

        /// <inheritdoc />
        protected override void PlatformRender()
        {
            Core.Logger.LogDebug("---Skia Rendering Messages.");
            m_hasRendered = false;
            try
            {
                if (m_surface is null)
                    CreateSurface();

                if (m_hasLock || m_surface is null)
                    return;

                //m_hasLock = Monitor.TryEnter(m_lock, m_lockTimeout);
                //m_hasLock = m_mutex.WaitOne(m_lockTimeout);
                //m_semaphore.WaitAsync();
                //var l = m_semaphore.WaitAsync(m_lockTimeout);
                //m_semaphore.WaitAsync();
                //m_hasLock = true;

                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //m_hasLock = m_lockSlim.TryEnterWriteLock(m_lockTimeout);
                //if (!m_hasLock)
                //    return;
                m_hasLock = TryLock();
                Monitor.TryEnter(m_lock, m_lockTimeout);
                lock (m_lock)
                { 
                //lock (m_surface)
                //{
                //m_hasLock = l.ConfigureAwait(false).GetAwaiter().GetResult();

                //lock (m_window)
                //{ 
                // the rectangle
                var rect = SKRect.Create(m_window.ClientArea.X.ToFloat() / 2,
                                                    m_window.ClientArea.Y.ToFloat() / 2,
                                                    m_window.ClientArea.Width.ToFloat() / 2,
                                                    m_window.ClientArea.Height.ToFloat() / 2);
                        //var rect = SKRect.Create(0, 0, 300, 300);

                        //var info = new SKImageInfo(m_window.ClientSize.Width, m_window.ClientSize.Height);
                        //using var surface = SKSurface.Create(info, m_buffer.PixelBuffer.Handle.Pointer);
                        //using var surface = SKSurface.Create(m_pixMap); //SKSurface.Create(info, m_buffer.PixelBuffer.Handle.Pointer);
                        //if (surface == null)
                        //    return;

                        using var paint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = Color.Blue.ToSKColor()
                        };

                        if (!m_hasLock)
                            return;

                        m_surface.Canvas.Clear(Color.BlueViolet.ToSKColor());
                        // draw fill
                        m_surface.Canvas.DrawRect(rect, paint);
                        //handler(surface);
                        m_hasRendered = true;

                        // the brush (fill with blue)
                        //var paint = new SKPaint
                        //{
                        //    Style = SKPaintStyle.Fill,
                        //    Color = Color.Blue.ToSKColor()
                        //};

                        // draw fill
                        //m_canvas.DrawRect(rect, paint);

                        //var c = Color.Black.ToSKColor();
                        //lock(m_surface)
                        //{
                        //    m_canvas.Clear(c);
                        //}  
                   //}     
                }
            }
            finally
            {
                if (m_hasLock)
                {
                    Monitor.Exit(m_lock);
                    //m_mutex.ReleaseMutex();
                    //m_semaphore.Release();
                    m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
        }

        /// <inheritdoc />
        protected override void PlatformRendered()
        {
            ReleaseSurface();
            Validate();
        }

        /// <inheritdoc />
        protected override void PlatformRendering()
        {
            CreateSurface();
        }

        /// <inheritdoc />
        protected override bool PlatformValidate()
        {
            return true;
        }

        protected override void OnProcessMessage(IPlatformMessage message)
        {
            switch (message.Id)
            {
                case MessageIds.Rendering:
                    var data = message.RawData as IRenderMessageData;
                    switch (data?.MessageId)
                    {
                        case RenderMessageIds.None:
                            break;
                        //case RenderMessageIds.OsRendering:
                        case RenderMessageIds.OsRender:
                        //case RenderMessageIds.OsRendered:
                            //Render();
                            //Core.MessagePump.PushFrameBuffer(this, m_buffer);
                            break;
                    }

                    break;
            }

            base.OnProcessMessage(message);
        }

        protected bool TryLock(int maxRetries = 3, int retryDelay = 50, int lockTimeout = 50)
        {
            for (var i = 0; i <= maxRetries; i++)
            {
                if (m_hasLock)
                    Thread.Sleep(retryDelay);

                var hasLock = m_lockSlim.TryEnterWriteLock(lockTimeout);
                //m_hasLock = true;
                //m_semaphore.Wait();
                if (hasLock) 
                    return true;

                Thread.Sleep(retryDelay);
                continue;

                //if (m_hasLock)
                //    return true;
                //    return null;
            }

            return false;
        }
        #endregion
    }
}