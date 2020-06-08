using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyState
    {
        public short Value;

        public KeyState(short value)
        {
            this.Value = value;
        }

        public bool IsPressed
        {
            // Note: The boolean check is performed on int, not short.
            get { return (this.Value & 0x8000) > 0; }
            set
            {
                if (value) this.Value = unchecked ((short) (this.Value | 0x8000));
                else this.Value = unchecked ((short) (this.Value & 0x7fff));
            }
        }

        public bool IsToggled
        {
            get { return (this.Value & 0x1) == 1; }
            set
            {
                if (value) this.Value = unchecked ((short) (this.Value | 0x1));
                else this.Value = unchecked ((short) (this.Value & 0xfffe));
            }
        }
    }
}