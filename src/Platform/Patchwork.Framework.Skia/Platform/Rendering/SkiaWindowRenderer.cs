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
using Shin.Framework.Threading;
using SkiaSharp;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class SkiaWindowRenderer : NWindowRenderer
    {
        //protected static readonly object m_lock = new object();
        //protected readonly ReaderWriterLockSlim m_lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        //protected bool m_hasLock = false;
        //protected readonly int m_lockTimeout = 100;
        //protected static readonly Mutex m_mutex = new Mutex(true);
        //protected static readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(0);
        protected static NFrameBuffer m_buffer;
        protected static NFrameBuffer m_oldBuffer;
        protected SKSurface m_surface;
        //protected SKCanvas m_canvas;
        protected SKImage m_map;
        protected SKPixmap m_pixMap;
        protected SafeHandle m_pointer;
        protected bool m_hasRendered;
        protected bool m_bufferChanged;

        /// <inheritdoc />
        public SkiaWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
            m_hasLock = false;
            m_level = RenderStage.Hal;
            //m_mutex = new Mutex(true);
        }

        #region Methods
        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_buffer = m_oldBuffer = new NFrameBuffer();
            m_bufferChanged = true;
            //CreateSurface();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            //Marshal.Release(m_pointer);
            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_pointer?.Dispose();
            m_buffer?.Dispose();
            m_oldBuffer?.Dispose();

            base.DisposeManagedResources();
        }

        protected void CreateSurface()
        {

            if (!m_isInitialized)
                return;
            //try
            //{
            //if (m_hasLock)
            //    return;

            //m_hasLock = Monitor.TryEnter(m_lock, m_lockTimeout);
            //m_hasLock = m_mutex.WaitOne(m_lockTimeout);
            //m_hasLock = m_semaphore.AvailableWaitHandle.WaitOne(m_lockTimeout);
            //m_semaphore.WaitAsync();
            //m_hasLock = m_semaphore.Wait(m_lockTimeout);
            //m_hasLock = m_lockSlim.TryEnterWriteLock(m_lockTimeout);
            //if (!m_hasLock)
            //    return;
            //m_hasLock = TryLock();
            //Monitor.TryEnter(m_lock, m_lockTimeout);
            //lock (m_lock)
            //{
            //m_hasLock = true;
            //lock (m_buffer)
            //{

            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            try
            {
                var rec = m_window.ClientArea;
                if (m_buffer.CheckSize(rec.Width, rec.Height))
                    return;

                m_bufferChanged = true;
                m_surface?.Dispose();
                m_buffer.EnsureSize(m_window.ClientSize.Width, m_window.ClientSize.Height);
                var info = new SKImageInfo(m_window.ClientSize.Width, m_window.ClientSize.Height, SKColorType.Bgra8888);
                //m_map = SKImage.Create(info);
                //m_pixMap = new SKPixmap(info, m_buffer.PixelBuffer.Handle.Pointer, m_buffer.PixelBuffer.RowBytes);
                //m_pixMap = m_map.PeekPixels();

                var surface = SKSurface.Create(info);
                //var surface = SKSurface.Create(m_pixMap);
                //var surface = SKSurface.Create(info, m_buffer.PixelBuffer.Handle.Pointer);
                //Throw.If(surface is null).InvalidOperationException();

                m_surface = surface;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
            
        }

        protected void SetBuffer()
        {
            //if (!m_bufferChanged)
            //    return;

            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            try
            {
                //if (!m_buffer.Equals(m_oldBuffer))
                //{
                var tmpMap = m_surface.PeekPixels();
                var tmpbuffer = new NFrameBuffer(tmpMap.Width, tmpMap.Height);
                var tmp = tmpMap.GetPixelSpan();
                tmpbuffer.SetPixelBuffer(tmp.ToArray());
                    Core.MessagePump.PushFrameBuffer(this, tmpbuffer);
                    m_oldBuffer?.Dispose();
                    m_oldBuffer = m_buffer;
                //}
                m_bufferChanged = false;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }
        protected void SwapBuffer()
        { 
            //try
            //{
            if (m_surface is null)
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
            //m_hasLock = TryLock();
            //Monitor.TryEnter(m_lock, m_lockTimeout);
            //lock(m_lock)
            //{
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
            //if (!m_hasLock)
            //    return;

            lock (m_buffer.PixelBuffer)
            {
                var snap = m_surface.Snapshot().PeekPixels();
                var check = false;
                if (m_buffer.IsDisposed)
                {
                    m_buffer = new NFrameBuffer();
                    check = true;
                }
                else
                {
                    var c = new ReadOnlySpan<byte>();
                    //var c = m_buffer.PixelBuffer.Contents;
                    var sc = snap.GetPixelSpan();
                    if (!c.SequenceEqual(sc))
                        check = true;
                }


                if (check)
                {
                    //m_buffer.SetPixelBuffer(snap.GetPixels()/*m_pixMap.GetPixels()*/,
                    //                        snap.Width,
                    //                        snap.Height,
                    //                        snap.RowBytes,
                    //                        snap.BytesSize);
                    var rec = m_window.ClientArea;
                    m_buffer = NPixelBuffer.GenerateBuffer(rec.Width, rec.Height);

                    //if (m_oldBuffer != m_buffer)
                    //{
                    Core.MessagePump.PushFrameBuffer(this, m_buffer);
                    //m_oldBuffer?.Dispose();
                    m_oldBuffer = m_buffer;
                    //}
                }
            }    
                    //m_buffer.Dispose();

                        //m_surface.Canvas.Flush();
                    //lock (m_oldBuffer)
                    //{
                        
                    //}
                    //m_buffer?.Dispose();
                            //}
                        //}
                        //p.Dispose();
                    //}
                //}
            //    }
            //}
            //finally
            //{
            //    m_surface?.Dispose();
            //    m_pixMap?.Dispose();
            //    m_map?.Dispose();

            //    if (m_hasLock)
            //    {
            //        Monitor.Exit(m_lock);
            //        //m_mutex.ReleaseMutex();
            //        //m_semaphore.Release();
            //        m_lockSlim.ExitWriteLock();
            //        m_hasLock = false;
            //    }
            //}
        }

        /// <inheritdoc />  
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e)
        {

            //Invalidate();
            //CreateSurface();
            
            //Render();
        }

        /// <inheritdoc />
        protected override void OnSizeChanged(object sender, PropertyChangedEventArgs<Size> e)
        {
            m_bufferChanged = true;
        }

        /// <inheritdoc />
        protected override bool PlatformInvalidate()
        {
            //if (m_isInitialized)
            //    Render();

            m_bufferChanged = true;
            return true;
        }

        /// <inheritdoc />
        protected override void PlatformRender()
        {
            //if (!m_bufferChanged)
            //    return;

            Core.Logger.LogDebug("---Skia Rendering Messages.");
            m_hasRendered = false;
            //try
            //{

            if (m_surface is null)
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
                //m_hasLock = TryLock();
                //Monitor.TryEnter(m_lock, m_lockTimeout);
                //lock (m_lock)
                //{ 
                //lock (m_surface)
                //{
                //m_hasLock = l.ConfigureAwait(false).GetAwaiter().GetResult();

                //lock (m_window)
                //{ 
                // the rectangle
                //var rect = SKRect.Create(m_window.ClientArea.X.ToFloat() / 2,
                                                    //m_window.ClientArea.Y.ToFloat() / 2,
                                                    //m_window.ClientArea.Width.ToFloat() / 2,
                                                    //m_window.ClientArea.Height.ToFloat() / 2);
                        var rect = SKRect.Create(0, 0, 300, 300);

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

            //if (!m_hasLock)
            //    return;

            //lock (m_buffer)
            //{
                //lock (m_buffer)
                //{ 
                    m_surface.Canvas.Clear(Color.BlueViolet.ToSKColor());
                    // draw fill
                    m_surface.Canvas.DrawRect(rect, paint);
                    //m_surface.
                    //handler(surface);
                //}
            //}
            
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
            //    }
            //}
            //finally
            //{
            //    if (m_hasLock)
            //    {
            //        Monitor.Exit(m_lock);
            //        //m_mutex.ReleaseMutex();
            //        //m_semaphore.Release();
            //        m_lockSlim.ExitWriteLock();
            //        m_hasLock = false;
            //    }
            //}
        }

        /// <inheritdoc />
        protected override void PlatformRendered()
        {
            //if (!m_bufferChanged)
            //    return;
            SetBuffer();
            //ReleaseSurface();
            //Validate();
        }

        /// <inheritdoc />
        protected override void PlatformRendering()
        {
            if (!m_bufferChanged)
                return;
            CreateSurface();
        }

        /// <inheritdoc />
        protected override bool PlatformValidate()
        {
            m_bufferChanged = false;
            return true;
        }

        #endregion
    }
}