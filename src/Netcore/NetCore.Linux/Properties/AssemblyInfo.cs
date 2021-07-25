#region Usings
using System.Reflection;
using Patchwork.Framework;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Platform;
#endregion

[assembly: AssemblyPlatform(OsType.Linux, 
                            1, 
                            "Linux", 
                            typeof(LinuxApplication), 
                            typeof(LinuxThreadDispatcher), 
                            typeof(LinuxOSInformation))]