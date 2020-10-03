#region Usings
using System;
using System.Drawing;
using System.Threading.Tasks;
using Hatzap.Rendering;
using Patchwork.Framework.Extensions;
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
        /// <inheritdoc />
        public SkiaWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
            renderDevice.ProcessMessage += OnProcessMessage;
        }

        #region Methods
        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_buffer = new NFrameBuffer(m_window.ClientSize.Width, m_window.ClientSize.Height);
            var info = new SKImageInfo(m_window.ClientSize.Width, m_window.ClientSize.Height);
            m_surface = SKSurface.Create(info, m_buffer.Handle.Pointer);
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_surface.Dispose();
            m_surface.Dispose();
            base.DisposeManagedResources();
        }

        protected void CreateSurface()
        {
            m_buffer.PixelBuffer.EnsureSize(m_window.ClientSize.Width, m_window.ClientSize.Height);
            var info = new SKImageInfo(m_window.ClientSize.Width, m_window.ClientSize.Height);
            m_surface = SKSurface.Create(info, m_buffer.PixelBuffer.Handle.Pointer);
        }

        /// <inheritdoc />
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e)
        {
            CreateSurface();
        }

        /// <inheritdoc />
        protected override bool PlatformInvalidate()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformRender()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformRendered()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformRendering()
        {
            m_surface.Canvas.Clear();
        }

        /// <inheritdoc />
        protected override bool PlatformValidate()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnProcessMessage(IPlatformMessage message)
        {
            switch (message.Id)
            {
                case MessageIds.Rendering:
                    Core.Logger.LogDebug("Found Rendering Messages.");
                    var data = (message as IPlatformMessage<IRenderMessageData>).Data;
                    switch (data?.MessageId)
                    {
                        case RenderMessageIds.None:
                            break;
                        //case RenderMessageIds.OsRendering:
                        case RenderMessageIds.OsRender:
                        //case RenderMessageIds.OsRendered:
                            Core.MessagePump.PushFrameBuffer(this, m_buffer);
                            break;
                    }

                    break;
            }
        }
        #endregion
    }
}