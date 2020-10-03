namespace Patchwork.Framework.Platform.Interop.Kernel32
{
    public enum SystemInvalidResult
    {
        INVALID_ATOM = 0,
        INVALID_OS_COUNT = 0xffff,
        INVALID_FILE_SIZE = unchecked((int)0xFFFFFFFF),
        INVALID_SET_FILE_POINTER = -1,
        INVALID_FILE_ATTRIBUTES = -1,
        INVALID_HANDLE_VALUE = -1
    }
}