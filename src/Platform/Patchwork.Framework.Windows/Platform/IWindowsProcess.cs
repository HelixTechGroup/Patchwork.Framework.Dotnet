#region Usings
using Patchwork.Framework.Platform.Interop.User32;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface IWindowsProcess : INObject
    {
        #region Properties
        WindowProc Process { get; }
        #endregion
    }
}