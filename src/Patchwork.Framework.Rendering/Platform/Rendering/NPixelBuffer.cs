#region Usings
using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class NPixelBuffer : NMemoryChunk
    {
        #region Members
        private int m_height;
        private int m_rowBytes;
        private int m_width;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INHandle Handle
        {
            get { return m_handle; }
        }

        public int Height
        {
            get { return m_height; }
        }

        public int RowBytes
        {
            get { return m_rowBytes; }
        }

        public int Width
        {
            get { return m_width; }
        }
        #endregion

        public NPixelBuffer(int width, int height)
        {
            EnsureSize(width, height);
        }

        internal NPixelBuffer(IntPtr handle, int width, int height, int rowBytes, int length)
            : base(handle, length)
        {
            m_width = width;
            m_height = height;
            m_rowBytes = rowBytes;
        }

        #region Methods
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
            if (m_isReadOnly ^ m_hasLock)
                return;

            var stride = 4 * ((width * 32 + 31) / 32);
            var length = height * stride;
            if (length != m_length)
                Resize(length);

            m_rowBytes = stride;
            m_width = width;
            m_height = height;
        }

        public NPixelBuffer Copy()
        {

            try
            {
                //if (m_hasLock)
                //    return null;

                //if (!m_spin.IsHeldByCurrentThread)
                //m_spin.TryEnter(ref m_hasLock);
                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //if (!m_hasLock)
                //    return null;

                TryLock();
                //Monitor.Enter(m_lock);
                lock (m_lock)
                {
                    NPixelBuffer pb;
                    lock (m_handle)
                    {
                        var ptr = Marshal.AllocHGlobal(m_length);
                        var arry = new byte[m_length];

                        Marshal.Copy(m_handle.Pointer, arry, 0, m_length);
                        Marshal.Copy(arry, 0, ptr, m_length);
                        pb = new NPixelBuffer(ptr, m_width, m_height, m_rowBytes, m_length);
                        pb.m_isReadOnly = false;
                    }

                    //Monitor.Exit(m_lock);
                    return pb;
                }
            }
            finally
            {
                if (m_hasLock)
                {
                    //if (m_spin.IsHeldByCurrentThread)
                    //    m_spin.Exit();
                    //Monitor.Exit(m_lock);
                    //m_semaphore.Release();
                    m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
        }
        #endregion
    }
}