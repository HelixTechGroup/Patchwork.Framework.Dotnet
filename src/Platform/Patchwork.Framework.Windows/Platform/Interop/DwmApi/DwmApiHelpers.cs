using System;
using System.Drawing;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace Patchwork.Framework.Platform.Interop.DwmApi
{
    public static class DwmApiHelpers
    {
        public static unsafe HResult DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttributeType dwAttribute,
            [In] ref int attrValue)
        {
            fixed (int* ptr = &attrValue) return DwmApiMethods.DwmSetWindowAttribute(hwnd, dwAttribute, new IntPtr(ptr), sizeof(int));
        }

        public static unsafe HResult DwmGetWindowAttribute(IntPtr hwnd, DwmWindowAttributeType dwAttribute,
            out Rectangle rect)
        {
            fixed (Rectangle* ptr = &rect) return DwmApiMethods.DwmGetWindowAttribute(hwnd, dwAttribute, new IntPtr(ptr), (uint) sizeof(Rectangle));
        }
    }
}