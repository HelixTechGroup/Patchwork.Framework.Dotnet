using System;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [Flags]
    public enum ClassLongFlags
    {
        /// <summary>
        ///     Sets the size, in bytes, of the extra memory associated with the class. Setting this value does not change the
        ///     number of extra bytes already allocated.
        /// </summary>
        GCL_CBCLSEXTRA = -20,

        /// <summary>
        ///     Sets the size, in bytes, of the extra window memory associated with each window in the class. Setting this value
        ///     does not change the number of extra bytes already allocated. For information on how to access this memory, see
        ///     SetWindowLong.
        /// </summary>
        GCL_CBWNDEXTRA = -18,

        /// <summary>
        ///     Replaces a handle to the background brush associated with the class.
        /// </summary>
        GCL_HBRBACKGROUND = -10,

        /// <summary>
        ///     Replaces a handle to the cursor associated with the class.
        /// </summary>
        GCL_HCURSOR = -12,

        /// <summary>
        ///     Replaces a handle to the icon associated with the class.
        /// </summary>
        GCL_HICON = -14,

        /// <summary>
        ///     Replace a handle to the small icon associated with the class.
        /// </summary>
        GCL_HICONSM = -34,

        /// <summary>
        ///     Replaces a handle to the module that registered the class.
        /// </summary>
        GCL_HMODULE = -16,

        /// <summary>
        ///     Replaces the address of the menu name string. The string identifies the menu resource associated with the class.
        /// </summary>
        GCL_MENUNAME = -8,

        /// <summary>
        ///     Replaces the window-class style bits.
        /// </summary>
        GCL_STYLE = -26,

        /// <summary>
        ///     Replaces the address of the window procedure associated with the class.
        /// </summary>
        GCL_WNDPROC = -24
    }
}