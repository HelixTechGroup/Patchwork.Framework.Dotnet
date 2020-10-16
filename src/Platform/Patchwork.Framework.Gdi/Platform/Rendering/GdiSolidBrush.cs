using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Platform.Interop.GdiPlus;

namespace Patchwork.Framework.Platform.Rendering
{
    public class GdiSolidBrush : SolidBrush<GpBrush>
    {
        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            if (m_color.IsEmpty && m_resource == IntPtr.Zero)
            {
                Status status = GDIPlus.GdipCreateSolidFill(m_color.ToArgb(), out nativeObject);
                GDIPlus.CheckStatus(status);
            }
            else
            {
                int val;
                Status status = GDIPlus.GdipGetSolidFillColor(ptr, out val);
                GDIPlus.CheckStatus(status);
                m_color = Color.FromArgb(val);
            }
        }

        /// <inheritdoc />
        protected override object PlatformClone()
        {
            IntPtr clonePtr;
            Status status = GDIPlus.GdipCloneBrush(nativeObject, out clonePtr);
            GDIPlus.CheckStatus(status);
            // we loose the named color in this case (but so does MS SD)
            return new SolidBrush(clonePtr);
        }

        /// <inheritdoc />
        public GdiSolidBrush(Color color) : base(color) { }

        /// <inheritdoc />
        public GdiSolidBrush(IntPtr ptr) : base(ptr) { }

        /// <inheritdoc />
        protected override Color PlatformGetColor()
        {
            return m_color;
        }

        /// <inheritdoc />
        protected override void PlatformSetColor(Color value)
        {
            m_color = value;
            Status status = GDIPlus.GdipSetSolidFillColor(nativeObject, value.ToArgb());
            GDIPlus.CheckStatus(status);
        }
    }
}
