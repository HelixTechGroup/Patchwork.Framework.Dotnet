#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TrackMouseEventOptions
    {
        #region Members
        public const uint DefaultHoverTime = 0xFFFFFFFF;
        public TrackMouseEventFlags Flags;
        public uint HoverTime;
        public uint Size;
        public IntPtr TrackedHwnd;
        #endregion
    }
}