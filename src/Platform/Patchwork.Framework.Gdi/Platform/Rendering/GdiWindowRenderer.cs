#region Usings
using System;
using System.Drawing;
using Hatzap.Rendering;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Helpers;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class GdiWindowRenderer : NWindowRenderer, IFrameBufferRenderer
    {
        #region Members
        protected NFrameBuffer m_buffer;
        protected IntPtr m_hdc;
        protected WindowsProcessHook m_hook;
        protected PaintStruct m_paintStruct;
        #endregion

        public GdiWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
            m_hook = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_GETMESSAGE);
            m_hook.ProcessMessage += OnGetMsg;
            m_buffer = new NFrameBuffer(m_window.ClientSize.Width, m_window.ClientSize.Height);
            renderDevice.ProcessMessage += OnProcessMessage;
        }

        #region Methods
        public bool Validate(ref Rectangle rect)
        {
            return ValidateRect(m_window.Handle.Pointer, ref rect);
        }

        public bool Invalidate(ref Rectangle rect, bool shouldErase)
        {
            return InvalidateRect(m_window.Handle.Pointer, ref rect, shouldErase);
        }

        public bool Invalidate(bool shouldErase)
        {
            return InvalidateRect(m_window.Handle.Pointer, IntPtr.Zero, shouldErase);
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_hook.Initialize();
        }

        /// <inheritdoc />
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e) { }

        protected override bool PlatformInvalidate()
        {
            return Invalidate(false);
        }

        /// <inheritdoc />
        protected override void PlatformRender()
        {
            if (m_hdc != IntPtr.Zero)
                return;

            if (GetUpdateRect(m_window.Handle.Pointer, out var rec, false))
                Validate();

            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRender, this);
        }

        /// <inheritdoc />
        protected override void PlatformRendered()
        {
            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendered, this);
            EndPaint(m_window.Handle.Pointer, ref m_paintStruct);
        }

        protected override void PlatformRendering()
        {
            m_hdc = BeginPaint(m_window.Handle.Pointer, out m_paintStruct);
            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendering, this);
            //var bmp = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
        }

        protected override bool PlatformValidate()
        {
            return ValidateRect(m_window.Handle.Pointer, IntPtr.Zero);
        }

        private IntPtr OnGetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    Render();
                    break;
                case WindowsMessageIds.NCCALCSIZE:
                case WindowsMessageIds.SIZE:
                case WindowsMessageIds.MOVE:
                case WindowsMessageIds.WINDOWPOSCHANGED:
                case WindowsMessageIds.WINDOWPOSCHANGING:
                case WindowsMessageIds.ERASEBKGND:
                case WindowsMessageIds.DISPLAYCHANGE:
                case WindowsMessageIds.CAPTURECHANGED:
                case WindowsMessageIds.NCHITTEST:
                case WindowsMessageIds.GETMINMAXINFO:
                case WindowsMessageIds.NCPAINT:
                    break;
            }

            return res;
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
                        case RenderMessageIds.SetFrameBuffer:
                            SetRgbBitsToDevice(m_hdc, data.FrameBuffer.PixelBuffer.Width, data.FrameBuffer.PixelBuffer.Height, data.FrameBuffer.PixelBuffer.Handle.Pointer);
                            break;
                    }

                    break;
            }
        }
        #endregion
    }
}