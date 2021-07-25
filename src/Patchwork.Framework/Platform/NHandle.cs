#region Usings
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Shin.Framework;
using Shin.Framework.Extensions;
using Shin.Framework.Runtime;
#endregion

namespace Patchwork.Framework.Platform
{
    public class NHandle : Disposable, INHandle, IEquatable<NHandle>
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
                return m_handle.DangerousGetHandle();

                //lock (m_handle)
                //{
                //try
                //    {
                    //Lock();
                        
                        //return m_handle.DangerousGetHandle();
 
                    //}
                    //finally
                    //{
                    //    m_handle.Unlock();
                    //}
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
            m_handle = new SafeMemoryHandle(handle);
            m_handleDescriptor = descriptor;
        }

        public NHandle(IntPtr handle) : this(handle, string.Empty) { }

        public NHandle() : this(IntPtr.Zero) { }

        #region Methods
        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            //m_handle.Close();
            m_handle.Dispose();
            //m_handle.SetHandleAsInvalid();
            
            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            //if (m_handle.IsClosed)
            //    Marshal.FreeHGlobal(m_handle.DangerousGetHandle());
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

        /// <inheritdoc />
        public bool Equals(NHandle other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return m_handle.Equals(other.m_handle);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is NHandle other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return m_handle.GetHashCode();
        }

        public static bool operator ==(NHandle left, NHandle right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NHandle left, NHandle right)
        {
            return !Equals(left, right);
        }
    }
}