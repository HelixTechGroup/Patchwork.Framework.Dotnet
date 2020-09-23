#region Usings
using System.Reflection;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Platform;
#endregion

[assembly: AssemblyTitle("Patchwork.Framework")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyVersion("0.0.0.1")]
[assembly: AssemblyFileVersion("0.0.0.1")]
[assembly: AssemblyPlatform(OperatingSystemType.Windows, 
                            1, 
                            "Windows", 
                            typeof(WinApplication), 
                            typeof(WinThreadDispatcher), 
                            typeof(WinOperatingSystemInformation))]
[assembly: AssemblyWindowing(OperatingSystemType.Windows,
                             1,
                             "Windows", 
                             typeof(WindowsWindowManager))]
[assembly: AssemblyRendering(OperatingSystemType.Windows,
                            1,
                            "Windows")]