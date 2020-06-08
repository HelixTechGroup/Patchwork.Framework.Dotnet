using Patchwork.Framework.Platform.Interop.User32;

namespace Patchwork.Framework.Platform
{
    public interface IWindowsProcess : INativeObject
    {
        WindowProc Process { get; }
    }
}
