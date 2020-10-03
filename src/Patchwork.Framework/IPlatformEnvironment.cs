#region Usings
#endregion

#region Usings
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework
{
    public interface IPlatformEnvironment
    {
        #region Properties
        ApplicationType ApplicationType { get; }

        string CurrentDirectory { get; }

        string DirectorySeparator { get; }

        bool Is64BitProcess { get; }

        bool IsRuntimeCodeGenerationSupported { get; }

        string NewLine { get; }

        IOsInformation OperatingSystem { get; }

        string PathSeparator { get; }

        string RootDirectory { get; }

        IRuntimeInformation Runtime { get; }
        #endregion

        #region Methods
        void DetectApplication();

        void DetectPlatform();
        #endregion
    }
}