using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Rendering;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;

namespace Patchwork.Framework.Platform
{
    public sealed class WinRenderAdapter : NRenderAdapter
    {
        /// <inheritdoc />
        protected override void PlatformFlush()
        {
            
        }

        /// <inheritdoc />
        protected override void PlatformSwapBuffers()
        {
            
        }
    }
}
