using System;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [Flags]
    public enum CreateWindowFlags
    {
        /// <summary>
        ///     Use default values
        /// </summary>
        CW_USEDEFAULT = unchecked((int) 0x80000000)
    }
}