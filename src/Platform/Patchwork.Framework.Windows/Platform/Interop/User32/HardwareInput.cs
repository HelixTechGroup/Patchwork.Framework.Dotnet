using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        public uint Message;
        public ushort Low;
        public ushort High;

        public uint WParam
        {
            get { return ((uint) this.High << 16) | this.Low; }
            set
            {
                this.Low = (ushort) value;
                this.High = (ushort) (value >> 16);
            }
        }
    }
}