#region Usings
#endregion

namespace Patchwork.Framework.Environment
{
    public interface IApplicationEnvironment
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