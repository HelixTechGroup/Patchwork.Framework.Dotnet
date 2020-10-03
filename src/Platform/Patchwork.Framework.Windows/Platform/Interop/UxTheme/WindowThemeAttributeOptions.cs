#region Usings
using System.Runtime.InteropServices;
#endregion

// ReSharper disable InconsistentNaming

namespace Patchwork.Framework.Platform.Interop.UxTheme
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowThemeAttributeOptions
    {
        #region Members
        public uint Flags;
        public uint Mask;
        #endregion
    }

    #region Parts and States
    #endregion
}