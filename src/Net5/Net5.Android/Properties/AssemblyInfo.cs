using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;
using Patchwork.Framework;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Threading;
using Patchwork.Framework.Platform.Windowing;

[assembly: AssemblyPlatform(OsType.Android,
                            1,
                            "Android",
                            typeof(AndroidApplication),
                            typeof(AndroidThreadDispatcher),
                            typeof(AndroidOSInformation))]
[assembly: AssemblyWindowing(OsType.Android,
                             1,
                             "Android",
                             typeof(AndroidWindow))]
//[assembly: AssemblyRendering(OsType.Windows,
//                             1,
//                             "Gdi",
//                             typeof(GdiRenderDevice))]
[assembly: AssemblyRendering(OsType.Android,
                             1,
                             "Skia",
                             typeof(SkiaRenderDevice))]
