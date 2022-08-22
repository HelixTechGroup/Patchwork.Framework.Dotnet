#region Usings
using System;
using Patchwork.Framework.Platform.Windowing;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Helpers;
using static Patchwork.Framework.Platform.Interop.Utilities;
#endregion

namespace Patchwork.Framework.Platform.Rendering;

public sealed class GdiAdapter : NRenderAdapter
{
    /// <inheritdoc />
    public GdiAdapter(INRenderDevice device, INResourceFactory factory) : base(device, factory) { }

    #region Methods
    /// <inheritdoc />
    protected override void PlatformFlush() { }

    /// <inheritdoc />
    protected override void PlatformSwapBuffers() { }
    #endregion
}