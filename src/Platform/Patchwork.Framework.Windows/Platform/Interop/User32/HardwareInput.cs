#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        #region Members
        public ushort High;
        public ushort Low;
        public uint Message;
        #endregion

        #region Properties
        public uint WParam
        {
            get { return ((uint)High << 16) | Low; }
            set
            {
                Low = (ushort)value;
                High = (ushort)(value >> 16);
            }
        }
        #endregion
    }
}