#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput
    {
        #region Members
        public IntPtr ExtraInfo;
        public KeyboardInputFlags Flags;
        public ushort ScanCode;
        public uint Time;
        public ushort VirtualKeyCode;
        #endregion

        #region Properties
        public VirtualKey Key
        {
            get { return (VirtualKey)VirtualKeyCode; }
            set { VirtualKeyCode = (ushort)value; }
        }
        #endregion
    }
}