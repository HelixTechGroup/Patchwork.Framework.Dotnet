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
        protected SKSurface m_surface;
        protected SKCanvas m_canvas;
        protected SKImage m_map;

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
                //m_map = new SKPixmap(info, m_buffer.PixelBuffer.Handle.Pointer);
                //m_surface = SKSurface.Create(info, m_buffer.PixelBuffer.Handle.Pointer);
                m_surface = SKSurface.Create(m_map.PeekPixels());
                m_canvas = m_surface.Canvas;
                m_map = m_surface.Snapshot();
            }
        }

        /// <inheritdoc />
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e)
        {
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
            lock (m_canvas)
            {
                // the rectangle
                var rect = SKRect.Create(m_window.ClientArea.X, 
                                            m_window.ClientArea.Y, 
                                            m_window.ClientArea.Width, 
                                            m_window.ClientArea.Height);

                var skPainted = false;
                try
                {
                    var info = new SKImageInfo(m_window.ClientSize.Width, m_window.ClientSize.Height);
                    using var surface = SKSurface.Create(info, m_buffer.PixelBuffer.Handle.Pointer);
                    if (surface == null)
                        return;

                    //var paint = new SKPaint
                    //{
                    //    Style = SKPaintStyle.Fill,
                    //    Color = Color.BlanchedAlmond.ToSKColor()
                    //};

                    // draw fill
                    //surface.Canvas.DrawRect(rect, paint);
                    surface.Canvas.Clear(Color.BlueViolet.ToSKColor());
                    //handler(surface);
                    skPainted = true;
                }
                finally
                {
                    if (skPainted)
                    {
                        //var ren = Core.Renderer.GetRenderer<IFrameBufferRenderer>();
                        //ren.Invalidate();
                        //var tmp = new NFrameBuffer(m_window.ClientSize.Width, m_window.ClientSize.Height);
                        //tmp.SetPixelBuffer(ptr, m_map.Width, m_map.Height, pixels.RowBytes, pixels.BytesSize);
                        Core.MessagePump.PushFrameBuffer(this, m_buffer);
                    }
                }

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
            //lock(m_canvas)
            //{
            //    m_canvas.Flush();
                
            //    m_map = m_surface.Snapshot();
            //}

            //lock(m_map)
            //{
            //    //var pixels = m_map.PeekPixels();
            //    using var pixels = m_map.PeekPixels().WithColorType(SKColorType.Rgb888x);
            //    IntPtr ptr = Marshal.AllocHGlobal(pixels.BytesSize);
            //    pixels.ReadPixels(pixels.Info, ptr, pixels.RowBytes);
            //    var tmp = new NFrameBuffer(m_window.ClientSize.Width, m_window.ClientSize.Height);
            //    tmp.SetPixelBuffer(ptr, m_map.Width, m_map.Height, pixels.RowBytes, pixels.BytesSize);
            //    Core.MessagePump.PushFrameBuffer(this, tmp);
            //    //    //m_buffer.Dispose();
            //}

            //m_canvas?.Dispose();
            //m_map?.Dispose();
            m_surface?.Dispose();
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
                    Core.Logger.LogDebug("Found Rendering Messages.");
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