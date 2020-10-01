#region Usings
using System;
#endregion

namespace Patchwork.Framework.Environment
{
    public interface IOSInformation
    {
        #region Properties
        bool Is64Bit { get; }
        bool IsUnixBased { get; }
        string Name { get; }
        PlatformType Platform { get; }
        OSType Type { get; }
        Version Version { get; }
        #endregion

        #region Methods
        void DetectOperatingSystem();
        #endregion
    }
}