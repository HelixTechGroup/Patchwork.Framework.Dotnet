using System;

namespace Patchwork.Framework.Platform.Interop.Kernel32 {
    [Flags]
    public enum LibrarySearchFlags
    {
        /// <summary>
        ///     If this value is used, the application's installation directory is searched.
        /// </summary>
        LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,

        /// <summary>
        ///     This value is a combination of LOAD_LIBRARY_SEARCH_APPLICATION_DIR,
        ///     LOAD_LIBRARY_SEARCH_SYSTEM32, and
        ///     LOAD_LIBRARY_SEARCH_USER_DIRS.
        ///     This value represents the recommended maximum number of directories an application should include in its
        ///     DLL search path.
        /// </summary>
        LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,

        /// <summary>
        ///     If this value is used, %windows%\system32 is searched.
        /// </summary>
        LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,

        /// <summary>
        ///     If this value is used, any path explicitly added using the
        ///     AddDllDirectory or
        ///     SetDllDirectory function is searched. If more than
        ///     one directory has been added, the order in which those directories are searched is unspecified.
        /// </summary>
        LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400
    }
}