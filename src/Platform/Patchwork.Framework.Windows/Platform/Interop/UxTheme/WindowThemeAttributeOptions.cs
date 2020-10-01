using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace Patchwork.Framework.Platform.Interop.UxTheme
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowThemeAttributeOptions
    {
        public uint Flags;
        public uint Mask;
    }

    #region Parts and States
    #endregion
}