#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Explicit)]
    public struct InputPacket
    {
        #region Members
        [FieldOffset(0)]
        public HardwareInput HardwareInput;

        [FieldOffset(0)]
        public KeyboardInput KeyboardInput;

        [FieldOffset(0)]
        public MouseInput MouseInput;
        #endregion
    }
}