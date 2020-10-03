#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.Kernel32
{
    [Flags]
    public enum GetModuleHandleFlags
    {
        /// <summary>
        ///     The lpModuleName parameter is an address in the module.
        /// </summary>
        GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS = 0x00000004,

        /// <summary>
        ///     The module stays loaded until the process is terminated, no matter how many times FreeLibrary is called.
        ///     This option cannot be used with GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT.
        /// </summary>
        GET_MODULE_HANDLE_EX_FLAG_PIN = 0x00000001,

        /// <summary>
        ///     The reference count for the module is not incremented. This option is equivalent to the behavior of
        ///     GetModuleHandle. Do not pass the retrieved module handle to the FreeLibrary function; doing so can cause the DLL to
        ///     be unmapped prematurely. For more information, see Remarks. This option cannot be used with
        ///     GET_MODULE_HANDLE_EX_FLAG_PIN.
        /// </summary>
        GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT = 0x00000002
    }
}