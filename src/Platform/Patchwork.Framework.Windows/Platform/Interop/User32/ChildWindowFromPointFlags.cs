#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [Flags]
    public enum ChildWindowFromPointFlags
    {
        /// <summary>
        ///     Does not skip any child windows
        /// </summary>
        CWP_ALL = 0x0000,

        /// <summary>
        ///     Skips disabled child windows
        /// </summary>
        CWP_SKIPDISABLED = 0x0002,

        /// <summary>
        ///     Skips invisible child windows
        /// </summary>
        CWP_SKIPINVISIBLE = 0x0001,

        /// <summary>
        ///     Skips transparent child windows
        /// </summary>
        CWP_SKIPTRANSPARENT = 0x0004
    }
}