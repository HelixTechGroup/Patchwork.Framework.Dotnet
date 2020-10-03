#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.UxTheme
{
    public static class UxThemeHelpers
    {
        #region Methods
        public static unsafe HResult SetWindowThemeNonClientAttributes(IntPtr hwnd,
                                                                       WindowThemeNcAttributeFlags mask,
                                                                       WindowThemeNcAttributeFlags attributes)
        {
            var opts = new WindowThemeAttributeOptions
                       {
                           Mask = (uint)mask,
                           Flags = (uint)attributes
                       };
            return UxThemeMethods.SetWindowThemeAttribute(hwnd,
                                                          WindowThemeAttributeType.WTA_NONCLIENT,
                                                          new IntPtr(&opts),
                                                          (uint)Marshal.SizeOf<WindowThemeAttributeOptions>());
        }
        #endregion
    }
}