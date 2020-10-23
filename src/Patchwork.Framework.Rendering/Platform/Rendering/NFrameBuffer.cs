using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Shin.Framework;

namespace Patchwork.Framework.Platform.Rendering
{
    public class NFrameBuffer : Disposable
    {
        protected NPixelBuffer m_pixelBuffer;
        //protected PixelFormat m_format;
        protected int m_height;
        protected int m_width;

        public NPixelBuffer PixelBuffer
        {
            get { return m_pixelBuffer; }
        }

        public int Height
        {
            get { return m_height; }
        }

        public int Width
        {
            get { return m_width; }
        }

        //public PixelFormat Format
        //{
        //    get { return m_format; }
        //}

        public NFrameBuffer() : this(0, 0) { }

        protected NFrameBuffer(int width, int height, NPixelBuffer pixelBuffer)
        {
            m_height = height;
            m_width = width;
            m_pixelBuffer = pixelBuffer;
        }

        public NFrameBuffer(int width, int height)
        {
            m_height = height;
            m_width = width;
            m_pixelBuffer = new NPixelBuffer(m_width, m_height);
        }

        public void SetPixelBuffer(NPixelBuffer buffer)
        {
            m_width = buffer.Width;
            m_height = buffer.Height;
            m_pixelBuffer = buffer;
        }

        public void SetPixelBuffer(IntPtr handle, int width, int height, int rowBytes, int length)
        {
            m_height = height;
            m_width = width;
            m_pixelBuffer = new NPixelBuffer(handle, m_width, m_height, rowBytes, length);
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_pixelBuffer.Dispose();
            m_height = 0;
            m_width = 0;
            base.DisposeManagedResources();
        }

        public bool CheckSize(int width, int height)
        {
            return width == m_width && height == m_height;
        }

        public void EnsureSize(int width, int height)
        {
            if (CheckSize(width, height))
                return;
            Resize(width, height);
        }

        public void Resize(int width, int height)
        {
            m_pixelBuffer.Resize(width, height);
            m_width = width;
            m_height = height;
        }

        public NFrameBuffer Copy()
        {
            var pb = m_pixelBuffer.Copy();
            return new NFrameBuffer(m_width, m_height, pb);
        }
    }
}
