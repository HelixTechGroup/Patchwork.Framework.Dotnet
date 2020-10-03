#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [Flags]
    public enum MouseInputXButtonFlag
    {
        /// <summary>
        ///     The first X button was clicked.
        /// </summary>
        XBUTTON1 = 0x0001,

        /// <summary>
        ///     The second X button was clicked.
        /// </summary>
        XBUTTON2 = 0x0002
    }
}