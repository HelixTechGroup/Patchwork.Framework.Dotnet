#region Usings
using System.Reflection;
using Patchwork.Framework;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Manager;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Threading;
using Patchwork.Framework.Platform.Windowing;
#endregion

[assembly: AssemblyPlatform(OsType.Windows,
                            1,
                            "Windows",
                            typeof(WinApplication),
                            typeof(WinThreadDispatcher),
                            typeof(WinOSInformation))]
[assembly: AssemblyWindowing(OsType.Windows,
                             1,
                             "Windows",
                             typeof(WinWindow))]
[assembly: AssemblyRendering(OsType.Windows,
                             1,
                             "Gdi",
                             typeof(GdiRenderDevice))]
[assembly: AssemblyRendering(OsType.Windows,
                             1,
                             "Skia",
                             typeof(SkiaRenderDevice))]