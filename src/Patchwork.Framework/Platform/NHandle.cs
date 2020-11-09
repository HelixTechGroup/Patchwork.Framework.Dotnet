#region Usings
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Shin.Framework;
using Shin.Framework.Extensions;
using Shin.Framework.Runtime;
#endregion

namespace Patchwork.Framework.Platform
{
    public class NHandle : Disposable, INHandle
    {
        #region Members
        //private readonly IntPtr m_handle;
        private readonly SafeMemoryHandle m_handle;
        private readonly string m_handleDescriptor;
        #endregion

        #region Properties
        /// <inheritdoc />
        public string HandleDescriptor
        {
            get { return m_handleDescriptor; }
        }

        /// <inheritdoc />
        public IntPtr Pointer
        {
            get
            {
                //lock (m_handle)
                //{
                    try
                    {
                    //Lock();
                        return m_handle.Lock().DangerousGetHandle();
                        //return m_handle.DangerousGetHandle();
 
                    }
                    finally
                    {
                        m_handle.Unlock();
                    }
                //}
            }
        }

        /// <inheritdoc />
        public INHandle Lock()
        {
            m_handle.Lock();
            return this;
        }

        /// <inheritdoc />
        public void Unlock()
        {
            m_handle.Unlock();
        }
        #endregion

        public NHandle(IntPtr handle, string descriptor)
        {
            m_handle = new SafeMemoryHandle(handle, false);
            m_handleDescriptor = descriptor;
        }

        public NHandle(IntPtr handle) : this(handle, "") { }

        public NHandle() : this(IntPtr.Zero) { }

        #region Methods
        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            //Marshal.FreeHGlobal(m_handle);
            //if (m_handle.IsInvalid)
            //    m_handle.Close();
            base.DisposeUnmanagedResources();
        }
        #endregion

        public static implicit operator IntPtr(NHandle obj)
        {
            try
            {
                return obj.Lock().Pointer;
            }
            finally
            {
                obj.Unlock();
            }
        }

        public static implicit operator NHandle(IntPtr obj)
        {
            return new NHandle(obj);
        }
    }
}