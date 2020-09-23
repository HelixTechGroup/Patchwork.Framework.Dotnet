#region Usings
#endregion

using Patchwork.Framework.Environment;

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

        IOperatingSystemInformation OperatingSystem { get; }

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