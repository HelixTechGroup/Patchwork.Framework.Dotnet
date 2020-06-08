#region Usings
using System;
#endregion

namespace Patchwork.Framework.Environment
{
    public interface IOperatingSystemInformation
    {
        #region Properties
        bool Is64Bit { get; }
        bool IsUnixBased { get; }
        string Name { get; }
        PlatformType Platform { get; }
        OperatingSystemType Type { get; }
        Version Version { get; }
        #endregion

        #region Methods
        void DetectOperatingSystem();
        #endregion
    }
}