#region Usings
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Extenstions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;
using SkiaSharp;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class SkiaWindowRenderer : NWindowRenderer
    {
        protected NFrameBuffer m_buffer;
        protected NFrameBuffer m_oldBuffer;
        protected SKSurface m_surface;
        protected SKCanvas m_canvas;
        protected SKImage m_map;
        protected SKPixmap m_pixMap;
        protected bool m_hasRendered;

        /// <inheritdoc />
        public SkiaWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
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
        protected override void DisposeManagedResources()
        {
            m_surface.Dispose();
            m_buffer.Dispose();
            base.DisposeManagedResources();
        }

        protected void CreateSurface()
        {
            lock (m_buffer)
            {
                m_surface?.Dispose();
                m_canvas?.Dispose();
                //m_map?.Dispose();

                m_buffer.EnsureSize(m_window.ClientSize.Width, m_window.ClientSize.Height);
                var info = new SKImageInfo(m_window.ClientSize.Width, m_window.ClientSize.Height);
                m_map = SKImage.Create(info);
                m_pixMap = new SKPixmap(info, m_buffer.PixelBuffer.Handle.Pointer, m_buffer.PixelBuffer.RowBytes);
                //m_map = new SKPixmap(info, m_buffer.PixelBuffer.Handle.Pointer);
                //m_surface = SKSurface.Create(info, m_buffer.PixelBuffer.Handle.Pointer);
                m_surface = SKSurface.Create(m_pixMap);
                m_canvas = m_surface.Canvas;
                //m_map = m_surface.Snapshot();
            }
        }

        /// <inheritdoc />
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e)
        {
            Invalidate();
            //CreateSurface();
        }

        /// <inheritdoc />
        protected override bool PlatformInvalidate()
        {
            return true;
        }

        /// <inheritdoc />
        protected override void PlatformRender()
        {
            lock (m_surface)
            {

                // the rectangle
                var rect = SKRect.Create(m_window.ClientArea.X/2, 
                                            m_window.ClientArea.Y/2, 
                                            m_window.ClientArea.Width/2, 
                                            m_window.ClientArea.Height/2);

                var info = new SKImageInfo(m_window.ClientSize.Width, m_window.ClientSize.Height);
                //using var surface = SKSurface.Create(info, m_buffer.PixelBuffer.Handle.Pointer);
                //using var surface = SKSurface.Create(m_pixMap); //SKSurface.Create(info, m_buffer.PixelBuffer.Handle.Pointer);
                //if (surface == null)
                //    return;

                var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.Black.ToSKColor()
                };

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
            }
        }

        /// <inheritdoc />
        protected override void PlatformRendered()
        {
            lock (m_surface)
            {
                //m_surface.Canvas.Flush();
                //m_surface.Canvas.Save();
                
            }

            lock (m_map)
            {
                //var pixels = m_map.PeekPixels();
                //using var pixels = m_map.PeekPixels().WithColorType(SKColorType.Rgb888x);
                var pixels = m_pixMap;
                IntPtr ptr = Marshal.AllocHGlobal(pixels.BytesSize);
                pixels.ReadPixels(pixels.Info, ptr, pixels.RowBytes);
                //var ba = new byte[pixels.BytesSize];
                //m_buffer = new NFrameBuffer(pixels.Width, pixels.Height);
                //Marshal.Copy(pixels.GetPixels(), ba, 0, pixels.BytesSize);
                //Marshal.Copy(ba, 0, ptr, pixels.BytesSize);
                //m_buffer.SetPixelBuffer(pixels.GetPixels(), pixels.Width, pixels.Height, pixels.RowBytes, pixels.BytesSize);
                Core.MessagePump.PushFrameBuffer(this, m_buffer.Copy());
            }

            lock (m_buffer)
            {
                //if (m_oldBuffer == m_buffer)
                //    return;

                //if (m_oldBuffer != m_buffer)
                //{
                //    m_oldBuffer = m_buffer;
                //    Core.MessagePump.PushFrameBuffer(this, m_buffer);
                //    m_buffer.Dispose();

                //    m_map.PeekPixels().GetPixels();
                //}
                //var ren = Core.Renderer.GetRenderer<IFrameBufferRenderer>();
                //ren.Invalidate();
                //var tmp = new NFrameBuffer(m_window.ClientSize.Width, m_window.ClientSize.Height);
                //tmp.SetPixelBuffer(ptr, m_map.Width, m_map.Height, pixels.RowBytes, pixels.BytesSize);
            }

            Validate();
            m_oldBuffer?.Dispose();
            m_oldBuffer = m_buffer.Copy();
            m_buffer?.Dispose();
            //m_canvas?.Dispose();
            //m_map?.Dispose();
            m_surface?.Dispose();
            m_pixMap?.Dispose();
            m_map?.Dispose();
        }

        /// <inheritdoc />
        protected override void PlatformRendering()
        {
            //if (m_surface == null)
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
                    //Core.Logger.LogDebug("Found Rendering Messages.");
                    var data = message.RawData as IRenderMessageData;
                    switch (data?.MessageId)
                    {
                        case RenderMessageIds.None:
                            break;
                        //case RenderMessageIds.OsRendering:
                        case RenderMessageIds.OsRender:
                        //case RenderMessageIds.OsRendered:
                            //Core.MessagePump.PushFrameBuffer(this, m_buffer);
                            break;
                    }

                    break;
            }

            base.OnProcessMessage(message);
        }
        #endregion
    }
}