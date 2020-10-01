#region Usings
using System.Reflection;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Manager;
using Patchwork.Framework.Platform;
#endregion

[assembly: AssemblyTitle("Patchwork.Framework")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyVersion("0.0.0.1")]
[assembly: AssemblyFileVersion("0.0.0.1")]
[assembly: AssemblyPlatform(OSType.Windows, 
                            1, 
                            "Windows", 
                            typeof(WinApplication), 
                            typeof(WinThreadDispatcher), 
                            typeof(WinOSInformation))]
[assembly: AssemblyWindowing(OSType.Windows,
                             1,
                             "Windows", 
                             typeof(WindowsWindowManager))]
[assembly: AssemblyRendering(OSType.Windows,
                            1,
                            "Windows")]