using System;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct TrackMouseEventOptions
    {
        public uint Size;
        public TrackMouseEventFlags Flags;
        public IntPtr TrackedHwnd;
        public uint HoverTime;

        public const uint DefaultHoverTime = 0xFFFFFFFF;
    }
}