#region Usings
using System.Runtime.InteropServices;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class NMemoryChunk : Disposable, INObject
    {
        #region Members
        protected INHandle m_handle;
        protected int m_length;
        #endregion

        #region Properties
        public byte[] Contents
        {
            get
            {
                var managedArray = new byte[m_length];
                Marshal.Copy(m_handle.Pointer, managedArray, 0, m_length);
                return managedArray;
            }
        }

        /// <inheritdoc />
        public INHandle Handle
        {
            get { return m_handle; }
        }

        public int Length
        {
            get { return m_length; }
        }
        #endregion

        public NMemoryChunk(int length)
        {
            m_length = length;
            m_handle = new NHandle(Marshal.AllocHGlobal(m_length), "");
        }

        public NMemoryChunk()
        {
            m_handle = new NHandle();
        }

        #region Methods
        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_handle.Dispose();
            m_length = 0;

            base.DisposeManagedResources();
        }
        #endregion
    }

    public class NPixelBuffer : NMemoryChunk
    {
        #region Members
        private int m_imageHeight;
        private int m_imageWidth;
        private int m_stride;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INHandle Handle
        {
            get { return m_handle; }
        }

        public int Height
        {
            get { return m_imageHeight; }
        }

        public int Stride
        {
            get { return m_stride; }
        }

        public int Width
        {
            get { return m_imageWidth; }
        }
        #endregion

        public NPixelBuffer(int imageWidth, int imageHeight)
        {
            EnsureSize(imageWidth, imageHeight);
        }

        #region Methods
        public bool CheckSize(int imageWidth, int imageHeight)
        {
            return imageWidth == m_imageWidth && imageHeight == m_imageHeight;
        }

        public void EnsureSize(int imageWidth, int imageHeight)
        {
            if (CheckSize(imageWidth, imageHeight))
                return;
            Resize(imageWidth, imageHeight);
        }

        public void Resize(int imageWidth, int imageHeight)
        {
            var stride = 4 * ((imageWidth * 32 + 31) / 32);
            var length = imageHeight * stride;
            if (length != m_length)
            {
                m_handle.Dispose();
                m_handle = new NHandle(Marshal.AllocHGlobal(length), ""); //Marshal.AllocHGlobal(bufferLength);
                m_length = length;
            }

            m_stride = stride;
            m_imageWidth = imageWidth;
            m_imageHeight = imageHeight;
        }
        #endregion
    }
}