using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using Shin.Framework;
using Shin.Framework.Extensions;
using Shin.Framework.Threading;

namespace Patchwork.Framework.Platform.Rendering
{
    public class NFrameBuffer : Disposable, IEquatable<NFrameBuffer>
    {
        protected NPixelBuffer m_pixelBuffer;
        //protected PixelFormat m_format;
        protected int m_height;
        protected int m_width;
        protected readonly ReaderWriterLockSlim m_lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

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

        public NFrameBuffer() : this(1, 1) { }

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

        public void SetPixelBuffer(int width, int height, ICollection<byte> collection)
        {
            m_height = height;
            m_width = width;
            m_pixelBuffer = new NPixelBuffer(m_width, m_height, collection);
        }

        public void SetPixelBuffer(ICollection<byte> collection)
        {
            m_pixelBuffer = new NPixelBuffer(m_width, m_height, collection);
        }

        public void SetPixelBuffer(NPixelBuffer buffer)
        {
            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            try
            {
                m_width = buffer.Width;
                m_height = buffer.Height;
                m_pixelBuffer = buffer;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }

        public void SetPixelBuffer(IntPtr handle, int width, int height, int rowBytes, int length)
        {
            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            try
            {
                m_height = height;
                m_width = width;
                m_pixelBuffer = new NPixelBuffer(handle, m_width, m_height, rowBytes, length);
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_pixelBuffer?.Dispose();
            m_height = 0;
            m_width = 0;
            base.DisposeManagedResources();
        }

        public bool CheckSize(int width, int height)
        {
            m_lockSlim.TryEnter();
            try
            {
                return (width <= 0 && height <= 0) || 
                       (width == m_width && height == m_height);
            }
            finally
            {
                m_lockSlim.TryExit();
            }
        }

        public void EnsureSize(int width, int height)
        {
            m_lockSlim.TryEnter();
            try
            {
                if (CheckSize(width, height))
                    return;
                Resize(width, height);
            }
            finally
            {
                m_lockSlim.TryExit();
            }
        }

        public void Resize(int width, int height)
        {
            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            try
            {
                m_pixelBuffer.Resize(width, height);
                m_width = width;
                m_height = height;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }

        public NFrameBuffer Copy()
        {

            m_lockSlim.TryEnter();
            try
            {
                var pb = m_pixelBuffer.Copy();
                return new NFrameBuffer(m_width, m_height, pb);
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }


        /// <inheritdoc />
        public bool Equals(NFrameBuffer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(m_pixelBuffer, other.m_pixelBuffer) && m_height == other.m_height && m_width == other.m_width;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is NFrameBuffer other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(m_pixelBuffer, m_height, m_width);
        }

        public static bool operator ==(NFrameBuffer left, NFrameBuffer right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NFrameBuffer left, NFrameBuffer right)
        {
            return !Equals(left, right);
        }
    }
}
