using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Platform.Interop.GdiPlus;
using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;

namespace Patchwork.Framework.Platform.Rendering
{
    public class GdiBrush : Brush<GpBrush>
    {
        public GdiBrush()
        {

        }

        /// <inheritdoc />
        /// <inheritdoc />
        protected override object PlatformClone()
        {
            return new GdiBrush();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            // NOTE: this has been known to fail in the past (cairo)
            // but it's the only way to reclaim brush related memory
            if (m_resource != IntPtr.Zero)
            {
                GpStatus status = GdiPlus.GdipDeleteBrush(m_resource);
                m_resource = IntPtr.Zero;
                GdiPlus.CheckStatus(status);
            }

            base.DisposeUnmanagedResources();
        }
    }
}
