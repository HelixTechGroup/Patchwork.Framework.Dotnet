using System;

namespace Patchwork.Framework.Platform.Interop.Kernel32 {
    [Flags]
    public enum HandleInfoFlags
    {
        /// <summary>
        ///     If this flag is set, a child process created with the bInheritHandles parameter of
        ///     CreateProcess set to TRUE will inherit the object handle.
        /// </summary>
        HANDLE_FLAG_INHERIT = 0x00000001,

        /// <summary>
        ///     If this flag is set, calling the
        ///     CloseHandle function will not close the object handle.
        /// </summary>
        HANDLE_FLAG_PROTECT_FROM_CLOSE = 0x00000002
    }
}