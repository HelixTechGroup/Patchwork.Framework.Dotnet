#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyState
    {
        #region Members
        public short Value;
        #endregion

        #region Properties
        public bool IsPressed
        {
            // Note: The boolean check is performed on int, not short.
            get { return (Value & 0x8000) > 0; }
            set
            {
                if (value) Value = unchecked((short)(Value | 0x8000));
                else Value = unchecked((short)(Value & 0x7fff));
            }
        }

        public bool IsToggled
        {
            get { return (Value & 0x1) == 1; }
            set
            {
                if (value) Value = unchecked((short)(Value | 0x1));
                else Value = unchecked((short)(Value & 0xfffe));
            }
        }
        #endregion

        public KeyState(short value)
        {
            Value = value;
        }
    }
}