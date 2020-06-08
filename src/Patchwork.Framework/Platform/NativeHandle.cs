#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform
{
    public class NativeHandle : INativeHandle
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

        public NativeHandle(IntPtr handle, string descriptor)
        {
            m_handle = handle;
            m_handleDescriptor = descriptor;
        }
    }
}