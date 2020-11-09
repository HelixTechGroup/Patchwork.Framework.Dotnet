using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public class NMemoryChunk : Disposable, INObject
    {
        #region Members
        protected INHandle m_handle;
        protected int m_length;
        protected bool m_isReadOnly;
        protected readonly ReaderWriterLockSlim m_lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        //protected static readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(0, int.MaxValue);
        protected static readonly object m_lock = new object();
        //[ThreadStatic]
        //protected static SpinLock m_spin = new SpinLock();
        protected bool m_hasLock = false;
        protected readonly int m_lockTimeout = 50;
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
            try
            {
                //if (m_hasLock)
                //    m_spin.Exit(true);
                //m_hasLock = false;
                //m_spin.TryEnter(ref m_hasLock);
                //Monitor.TryEnter(m_lock, ref m_hasLock);
                TryLock();
                lock(m_lock)
                {
                    m_handle = new NHandle(handle);
                    m_isReadOnly = true;
                    m_length = length;
                }
            }
            finally
            {
                if (m_hasLock)
                {
                    //if (m_spin.IsHeldByCurrentThread)
                    //    m_spin.Exit(true);
                    //Monitor.Exit(m_lock);
                    //m_semaphore.Release();
                    m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
        }

        public NMemoryChunk(int length)
        {
            try
            {
                //if (m_hasLock)
                //    m_spin.Exit(true);
                //m_hasLock = false;
                //m_spin.TryEnter(ref m_hasLock);
                //Monitor.TryEnter(m_lock, ref m_hasLock);
                TryLock();
                lock (m_lock)
                { 
                        //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                        //if (!m_hasLock)
                        //    return;

                        m_length = length;
                        m_handle = new NHandle(Marshal.AllocHGlobal(m_length), "");
                        GC.AddMemoryPressure(m_length);
                        //Monitor.Exit(m_lock);
                }
            }
            finally
            {
                if (m_hasLock)
                {
                    //if (m_spin.IsHeldByCurrentThread)
                    //    m_spin.Exit(true);
                    //Monitor.Exit(m_lock);
                    //m_semaphore.Release();
                    m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
        }

        public NMemoryChunk()
        {
            m_handle = new NHandle();
        }

        #region Methods
        public void Resize(int length)
        {
            if (m_isReadOnly ^ m_hasLock)
                return;

            Throw.If(length <= 0).ArgumentOutOfRangeException(nameof(length));

            try
            {
                //m_spin.TryEnter(ref m_hasLock);
                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //Monitor.TryEnter(m_lock, ref m_hasLock);
                //if (!m_hasLock)
                //    return;
                TryLock();
                lock (m_lock)
                {
                    lock (m_handle)
                    {
                        m_handle.Dispose();
                        if (m_length > 0)
                            GC.RemoveMemoryPressure(m_length);
                        m_handle = new NHandle(Marshal.AllocHGlobal(length), ""); //Marshal.AllocHGlobal(bufferLength);
                        m_length = length;
                        GC.AddMemoryPressure(m_length);
                        //Monitor.Exit(m_lock);
                    }
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

        public NMemoryChunk Lock()
        {
            if (m_hasLock)
                return null;

            //m_hasLock = m_semaphore.Wait(m_lockTimeout);
            //if (!m_hasLock)
            //    return null;

            try
            {
                TryLock();
                return this;
            }
            catch
            {
                //m_semaphore.Release();
                m_lockSlim.ExitWriteLock();
                m_hasLock = false;
                throw;
            }
        }

        public void Unlock()
        {
            //m_semaphore.Release();
            m_lockSlim.ExitWriteLock();
            m_hasLock = false;
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            try
            {
                //m_spin.Enter(ref m_hasLock);
                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //Monitor.TryEnter(m_lock, ref m_hasLock);
                //if (!m_hasLock)
                //    return;

                TryLock();
                lock (m_lock)
                {
                    m_handle.Dispose();
                    if (m_length > 0)
                        GC.RemoveMemoryPressure(m_length);
                    m_length = 0;
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

            base.DisposeManagedResources();
        }

        protected bool TryLock(int maxRetries = 3, int retryDelay = 50, int lockTimeout = 50)
        {
            for (var i = 0; i <= maxRetries; i++)
            {
                if (m_hasLock)
                    Thread.Sleep(retryDelay);

                m_hasLock = m_lockSlim.TryEnterWriteLock(lockTimeout);
                //m_hasLock = true;
                //m_semaphore.Wait();
                if (!m_hasLock)
                    Thread.Sleep(retryDelay);
                else
                    return true;

                //if (m_hasLock)
                //    return true;
                //    return null;
            }

            return false;
        }
        #endregion
    }
}