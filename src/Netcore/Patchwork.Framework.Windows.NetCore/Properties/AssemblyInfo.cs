#region Usings
using System.Reflection;
using Patchwork.Framework;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Manager;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Threading;
#endregion

[assembly: AssemblyTitle("Patchwork.Framework")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyVersion("0.0.0.1")]
[assembly: AssemblyFileVersion("0.0.0.1")]
[assembly: AssemblyPlatform(OsType.Windows,
                            1,
                            "Windows",
                            typeof(WinApplication),
                            typeof(WinThreadDispatcher),
                            typeof(WinOSInformation))]
[assembly: AssemblyWindowing(OsType.Windows,
                             1,
                             "Windows",
                             typeof(WindowsWindowManager))]
//[assembly: AssemblyRendering(OsType.Windows,
//                             1,
//                             "Gdi",
//                             typeof(PlatformRenderManager),
//                             typeof(GdiRenderDevice))]
//[assembly: AssemblyRendering(OsType.Windows,
//                             1,
//                             "Skia",
//                             null,
//                             typeof(SkiaRenderDevice))]