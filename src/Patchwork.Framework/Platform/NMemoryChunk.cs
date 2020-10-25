using System;
using System.Runtime.InteropServices;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public class NMemoryChunk : Disposable, INObject
    {
        #region Members
        protected INHandle m_handle;
        protected int m_length;
        protected bool m_isReadOnly;
        #endregion

        #region Properties
        public byte[] Contents
        {
            get
            {
                lock (m_handle)
                {
                    if (m_length <= 0)
                        return Array.Empty<byte>();

                    unsafe
                    {
                        var pointer = m_handle.Pointer;
                        var managedArray = new byte[m_length];
                        //for (int i = 0; i < m_length; ++i)
                        //    managedArray[i] = pointer[i + 1];
                        //managedArray = Marshal.PtrToStructure<byte[]>(pointer);
                        Marshal.Copy(pointer, managedArray, 0, m_length);
                        return managedArray;
                    }
                }
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

        public bool IsReadOnly
        {
            get { return m_isReadOnly; }
        }
        #endregion
        internal NMemoryChunk(IntPtr handle, int length)
        {
            m_handle = new NHandle(handle);
            m_isReadOnly = true;
            m_length = length;
        }

        public NMemoryChunk(int length)
        {
            m_length = length;
            m_handle = new NHandle(Marshal.AllocHGlobal(m_length), "");
            GC.AddMemoryPressure(m_length);
        }

        public NMemoryChunk()
        {
            m_handle = new NHandle();
        }

        #region Methods
        public void Resize(int length)
        {
            if (m_isReadOnly)
                return;

            Throw.If(length <= 0).ArgumentOutOfRangeException(nameof(length));

            m_handle.Dispose();
            if (m_length > 0)
                GC.RemoveMemoryPressure(m_length);
            m_handle = new NHandle(Marshal.AllocHGlobal(length), ""); //Marshal.AllocHGlobal(bufferLength);
            m_length = length;
            GC.AddMemoryPressure(m_length);
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_handle.Dispose();
            if (m_length > 0)
                GC.RemoveMemoryPressure(m_length);
            m_length = 0;

            base.DisposeManagedResources();
        }
        #endregion
    }
}