using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Shin.Framework;
using Shin.Framework.Extensions;
using Shin.Framework.Runtime;
using Shin.Framework.Threading;

namespace Patchwork.Framework.Platform
{
    public class NMemoryChunk : Disposable, INObject, IEquatable<NMemoryChunk>
    {
        #region Members
        protected INHandle m_handle;
        protected SafeMemoryBuffer m_buffer;
        //protected int m_length;
        protected bool m_isReadOnly;

        //protected static readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(0, int.MaxValue);
        //protected static readonly object m_lock = new object();
        protected readonly ReaderWriterLockSlim m_lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        //[ThreadStatic]
        //protected static SpinLock m_spin = new SpinLock();
        #endregion

        #region Properties
        public ReadOnlySpan<byte> Contents
        {
            get
            {
                return GetContent();
            }
            //internal set
            //{
            //    SetContent(value);
            //}
        }

        /// <inheritdoc />
        public INHandle Handle
        {
            get { return m_handle; }
        }

        public int Length
        {
            get { return (int)m_buffer.ByteLength; }
        }

        public bool IsReadOnly
        {
            get { return m_isReadOnly; }
        }
        #endregion

        internal NMemoryChunk(IntPtr handle, int length)
        {
            //try
            //{
            //if (m_hasLock)
            //    m_spin.Exit(true);
            //m_hasLock = false;
            //m_spin.TryEnter(ref m_hasLock);
            //Monitor.TryEnter(m_lock, ref m_hasLock);
            //m_lock.TryLock();
            //lock (m_lock)
            //{
            m_buffer = new SafeMemoryBuffer(handle, length);
            m_handle = new NHandle(handle);
            m_isReadOnly = true;
            //m_length = length;
            //}
            //}
            //finally
            //{
            //    if (m_hasWriteLock)
            //    {
            //        //if (m_spin.IsHeldByCurrentThread)
            //        //    m_spin.Exit(true);
            //        //Monitor.Exit(m_lock);
            //        //m_semaphore.Release();
            //        m_lockSlim.ExitWriteLock();
            //        m_hasWriteLock = false;
            //    }
            //}
        }

        public NMemoryChunk(int length)
        {
            //try
            //{
            //if (m_hasLock)
            //    m_spin.Exit(true);
            //m_hasLock = false;
            //m_spin.TryEnter(ref m_hasLock);
            //Monitor.TryEnter(m_lock, ref m_hasLock);
            //TryLock();
            //lock (m_lock)
            //{
            //m_hasLock = m_semaphore.Wait(m_lockTimeout);
            //if (!m_hasLock)
            //    return;
            if (length <= 0)
                return;

            //m_length = length;
            m_buffer = new SafeMemoryBuffer(length);
            m_handle = new NHandle(m_buffer.DangerousGetHandle());
            GC.AddMemoryPressure(length);
            //Monitor.Exit(m_lock);
            //}
            //}
            //finally
            //{
            //    if (m_hasWriteLock)
            //    {
            //if (m_spin.IsHeldByCurrentThread)
            //    m_spin.Exit(true);
            //Monitor.Exit(m_lock);
            //m_semaphore.Release();
            //        m_lockSlim.ExitWriteLock();
            //        m_hasWriteLock = false;
            //    }
            //}
        }

        public NMemoryChunk()
        {
            m_handle = new NHandle();
            m_buffer = new SafeMemoryBuffer(true);
            m_buffer = new SafeMemoryBuffer(1);
            //m_buffer.Initialize(1);

        }

        public NMemoryChunk(ICollection<byte> collection) : this()
        {
            m_buffer = new SafeMemoryBuffer(collection.Count); 
            SetContent(collection as byte[]);
        }

        #region Methods
        public void Resize(int length)
        {
            if (m_isReadOnly)
                return;

            Throw.If(length <= 0).ArgumentOutOfRangeException(nameof(length));

            if ((int)m_buffer.ByteLength == length)
                return;

            m_lock.TryEnter(SynchronizationAccess.Write);
            try
            {
                //m_spin.TryEnter(ref m_hasLock);
                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //Monitor.TryEnter(m_lock, ref m_hasLock);
                //if (!m_hasLock)
                //    return;
                //lock (m_lock)
                //{
                //    lock (m_handle)
                //    {
                //m_handle.Dispose();
                //m_buffer.Dispose();
                //m_buffer = new SafeMemoryBuffer(length);
                var c = new byte[(int)m_buffer.ByteLength];
                if ((int)m_buffer.ByteLength > 0)
                {
                    GC.RemoveMemoryPressure((int)m_buffer.ByteLength);
                    //m_buffer.ReadArray(0, c, 0, m_length);
                    //Monitor.Exit(m_lock); 
                }

                m_buffer.Initialize((ulong)length);

                //m_buffer.WriteArray(0, c, 0, m_length);
                //Interlocked.Exchange(ref c, null);
                //m_handle = new NHandle(Marshal.AllocHGlobal(length), ""); //Marshal.AllocHGlobal(bufferLength);
                //Interlocked.Exchange(ref m_length, length);
                //m_length = length;
                GC.AddMemoryPressure((int)m_buffer.ByteLength);
                //    }
                //}
            }
            finally
            {
                m_lock.TryExit(SynchronizationAccess.Write);
                //if (m_hasWriteLock)
                //{
                //if (m_spin.IsHeldByCurrentThread)
                //    m_spin.Exit();
                //Monitor.Exit(m_lock);
                //m_semaphore.Release();
                //    m_lockSlim.ExitWriteLock();
                //    m_hasWriteLock = false;
                //}
            }

        }

        //public NMemoryChunk Lock()
        //{
        //    if (m_hasWriteLock)
        //        return null;

        //    //m_hasLock = m_semaphore.Wait(m_lockTimeout);
        //    //if (!m_hasLock)
        //    //    return null;

        //    try
        //    {
        //        TryLock();
        //        return this;
        //    }
        //    catch
        //    {
        //        //m_semaphore.Release();
        //        m_lockSlim.ExitWriteLock();
        //        m_hasWriteLock = false;
        //        throw;
        //    }
        //}

        //public void Unlock()
        //{
        //    //m_semaphore.Release();
        //    m_lockSlim.ExitWriteLock();
        //    m_hasWriteLock = false;
        //}

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            //try
            //{
            //m_spin.Enter(ref m_hasLock);
            //m_hasLock = m_semaphore.Wait(m_lockTimeout);
            //Monitor.TryEnter(m_lock, ref m_hasLock);
            //if (!m_hasLock)
            //    return;

            //TryLock();
            //lock (m_lock)
            //{
            //m_handle.Dispose();

            if (m_buffer.ByteLength > 0)
                GC.RemoveMemoryPressure((int)m_buffer.ByteLength);

            m_buffer.Dispose();
            //m_length = 0;
            //    }
            //}
            //finally
            //{
            //    if (m_hasWriteLock)
            //    {
            //if (m_spin.IsHeldByCurrentThread)
            //    m_spin.Exit();
            //Monitor.Exit(m_lock);
            //m_semaphore.Release();
            //        m_lockSlim.ExitWriteLock();
            //        m_hasWriteLock = false;
            //    }
            //}

            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            //m_buffer.ReleasePointer();
            base.DisposeUnmanagedResources();
        }

        public static NMemoryChunk Empty
        {
            get
            {
                return new NMemoryChunk();
            }
        }
        #endregion

        /// <inheritdoc />
        public bool Equals(NMemoryChunk other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(m_handle, other.m_handle); /*&&
                   m_buffer.ByteLength == other.m_buffer.ByteLength; //m_length == other.m_length;*/
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is NMemoryChunk other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(m_handle, m_buffer);
        }

        public static bool operator ==(NMemoryChunk left, NMemoryChunk right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NMemoryChunk left, NMemoryChunk right)
        {
            return !Equals(left, right);
        }

        public void SetContent(byte[] value)
        {
            //lock (m_buffer)
            //{
            if (value.Length <= 0)
                return;

            m_lock.TryEnter(SynchronizationAccess.Write);
            try
            {
                Resize(value.Length);
                m_buffer.WriteArray(0, value, 0, value.Length);
                //unsafe
                //{
                //    Marshal.Copy(value, 0, m_handle.Pointer, m_length);
                //var pointer = m_handle.Pointer;
                //var managedArray = new byte[m_length];
                //for (int i = 0; i < m_length; ++i)
                //    managedArray[i] = pointer[i + 1];
                //managedArray = Marshal.PtrToStructure<byte[]>(pointer);
                //Marshal.Copy(pointer, managedArray, 0, m_length);
                //}
            }
            finally
            {
                m_lock.TryExit(SynchronizationAccess.Write);
            }
        }

        protected virtual ReadOnlySpan<byte> GetContent()
        {
            //Move this lock!!!
            //lock(m_buffer)

            m_lock.TryEnter();
            try
            {
                //var tmp = Array.Empty<byte>();
                //lock (m_buffer)
                //{
                if ((m_buffer.IsInvalid || m_buffer.IsClosed) || m_buffer.ByteLength <= 0)
                    return Array.Empty<byte>();

                var len = (int)m_buffer.ByteLength;
                unsafe
                { 
                    byte* ptr = null;
                    m_buffer.AcquirePointer(ref ptr);
                    var span = new ReadOnlySpan<byte>(ptr, len);
                    m_buffer.ReleasePointer();
                    return span;
                }

                //var arry = new byte[len];
                //if (!m_buffer.IsInvalid && !m_buffer.IsClosed)
                //    m_buffer.ReadArray(0, arry, 0, (int)len);

                //return arry;
                //unsafe
                //{
                //    var pointer = m_handle.Pointer;
                //    var managedArray = new byte[m_length];
                //    //for (int i = 0; i < m_length; ++i)
                //    //    managedArray[i] = pointer[i + 1];
                //    //managedArray = Marshal.PtrToStructure<byte[]>(pointer);
                //    Marshal.Copy(pointer, managedArray, 0, m_length);
                //    return managedArray;
                //}
                //}
            }
            finally
            {
                m_lock.TryExit();
            }
        }
    }
}