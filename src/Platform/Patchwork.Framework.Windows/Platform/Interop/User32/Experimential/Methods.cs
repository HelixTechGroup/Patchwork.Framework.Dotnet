using System;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32.Experimential
{
    public static class User32ExperimentalMethods
    {
        [DllImport(Methods.LibraryName, CharSet = Properties.BuildCharSet)]
        internal static extern bool SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
    }
}