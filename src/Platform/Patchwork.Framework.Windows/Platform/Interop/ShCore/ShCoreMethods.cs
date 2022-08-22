#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.ShCore
{
    public static class ShCoreMethods
    {
        #region Members
        public const string LibraryName = "shcore";
        #endregion

        #region Methods
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern HResult GetDpiForMonitor(IntPtr hmonitor,
                                                      MonitorDpiType dpiType,
                                                      out uint dpiX,
                                                      out uint dpiY);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool SetProcessDpiAwareness(ProcessDpiAwareness awareness);
        #endregion
    }
}