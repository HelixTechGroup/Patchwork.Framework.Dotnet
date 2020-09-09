using System;

namespace Patchwork.Framework.Platform.Interop.Kernel32
{
    [Flags]
    public enum DesiredAccess : uint
    {
        GenericRead = 0x80000000,
        GenericWrite = 0x40000000,
        GenericExecute = 0x20000000,
        GenericAll = 0x10000000
    }
}