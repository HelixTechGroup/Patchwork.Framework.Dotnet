using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [StructLayout(LayoutKind.Explicit)]
    public struct InputPacket
    {
        [FieldOffset(0)] public MouseInput MouseInput;
        [FieldOffset(0)] public KeyboardInput KeyboardInput;
        [FieldOffset(0)] public HardwareInput HardwareInput;
    }
}