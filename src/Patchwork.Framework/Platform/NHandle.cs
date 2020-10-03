#region Usings
using System;
using System.Runtime.InteropServices;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public class NHandle : Disposable, INHandle
    {
        #region Members
        private readonly IntPtr m_handle;
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
            get { return m_handle; }
        }
        #endregion

        public NHandle(IntPtr handle, string descriptor)
        {
            m_handle = handle;
            m_handleDescriptor = descriptor;
        }

        public NHandle(IntPtr handle) : this(handle, "") { }

        public NHandle() : this(IntPtr.Zero) { }

        #region Methods
        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            Marshal.FreeHGlobal(m_handle);
            base.DisposeUnmanagedResources();
        }
        #endregion
    }
}