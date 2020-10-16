#region Usings
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Patchwork.Framework.Platform.Interop.GdiPlus;
using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;
using CustomLineCap = System.Drawing.Drawing2D.CustomLineCap;
#endregion

namespace Patchwork.Framework.Platform
{
    public class GdiPen : Pen<GpPen>
    {
        /// <inheritdoc />
        public GdiPen(Brush brush) : base(brush) { }

        /// <inheritdoc />
        public GdiPen(Color color) : base(color) { }

        /// <inheritdoc />
        public GdiPen(Brush brush, float width) : base(brush, width) { }

        /// <inheritdoc />
        public GdiPen(Color color, float width) : base(color, width) { }

        #region Methods
        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            if (!isModifiable)
                throw new ArgumentException("This Pen object can't be modified.");

            if (m_resource != IntPtr.Zero)
            {
                var status = GdiPlus.GdipDeletePen(m_resource);
                m_resource = null;
                GdiPlus.CheckStatus(status);
            }

            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            GpStatus status;
            if (m_brush != null)
            {
                status = GdiPlus.GdipCreatePen1(m_color.ToArgb(), m_width, GraphicsUnit.World, out m_resource);
                GdiPlus.CheckStatus(status);
                return;
            }

            status = GdiPlus.GdipCreatePen2(m_brush.Resource, m_width, GraphicsUnit.World, out m_resource);
            GdiPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override object PlatformClone()
        {
            var status = GdiPlus.GdipClonePen(m_resource, out var ptr);
            GdiPlus.CheckStatus(status);
            var p = new GdiPen(ptr);
            p.m_startCap = m_startCap;
            p.m_endCap = m_endCap;

            return p;
        }

        /// <inheritdoc />
        protected override Brush PlatformGetBrush()
        {
            IntPtr brush;
            Status status = GdiPlus.GdipGetPenBrushFill(nativeObject, out brush);
            GDIPlus.CheckStatus(status);
            return new SolidBrush(brush);
        }

        /// <inheritdoc />
        protected override Color PlatformGetColor()
        {
            if (m_color.Equals(Color.Empty))
            {
                int c;
                Status status = GDIPlus.GdipGetPenColor(nativeObject, out c);
                GDIPlus.CheckStatus(status);
                m_color = Color.FromArgb(c);
            }

            return m_color;
        }

        /// <inheritdoc />
        protected override float[] PlatformGetCompoundArray()
        {
            int count;
            Status status = GDIPlus.GdipGetPenCompoundCount(nativeObject, out count);
            GDIPlus.CheckStatus(status);

            var compArray = new float[count];
            status = GDIPlus.GdipGetPenCompoundArray(nativeObject, compArray, count);
            GDIPlus.CheckStatus(status);

            return compArray;
        }

        /// <inheritdoc />
        protected override CustomLineCap PlatformGetCustomEndCap()
        {
            return m_endCap;
        }

        /// <inheritdoc />
        protected override CustomLineCap PlatformGetCustomStartCap()
        {
            return m_startCap;
        }

        /// <inheritdoc />
        protected override DashCap PlatformGetDashCap()
        {
            DashCap retval;
            Status status = GDIPlus.GdipGetPenDashCap197819(nativeObject, out retval);
            GDIPlus.CheckStatus(status);
            return retval;
        }

        /// <inheritdoc />
        protected override float PlatformGetDashOffset()
        {
            float retval;
            Status status = GDIPlus.GdipGetPenDashOffset(nativeObject, out retval);
            GDIPlus.CheckStatus(status);
            return retval;
        }

        /// <inheritdoc />
        protected override float[] PlatformGetDashPattern()
        {
            int count;
            Status status = GDIPlus.GdipGetPenDashCount(nativeObject, out count);
            GDIPlus.CheckStatus(status);

            float[] pattern;
            // don't call GdipGetPenDashArray with a 0 count
            if (count > 0)
            {
                pattern = new float[count];
                status = GDIPlus.GdipGetPenDashArray(nativeObject, pattern, count);
                GDIPlus.CheckStatus(status);
            }
            else if (DashStyle == DashStyle.Custom)
            {
                // special case (not handled inside GDI+)
                pattern = new float[1];
                pattern[0] = 1.0f;
            }
            else
                pattern = new float[0];

            return pattern;
        }

        /// <inheritdoc />
        protected override DashStyle PlatformGetDashStyle()
        {
            DashStyle retval;
            Status status = GDIPlus.GdipGetPenDashStyle(nativeObject, out retval);
            GDIPlus.CheckStatus(status);
            return retval;
        }

        /// <inheritdoc />
        protected override LineCap PlatformGetEndCap()
        {
            LineCap retval;
            Status status = GDIPlus.GdipGetPenEndCap(nativeObject, out retval);
            GDIPlus.CheckStatus(status);

            return retval;
        }

        /// <inheritdoc />
        protected override LineJoin PlatformGetLineJoin()
        {
            LineJoin result;
            Status status = GDIPlus.GdipGetPenLineJoin(nativeObject, out result);
            GDIPlus.CheckStatus(status);
            return result;
        }

        /// <inheritdoc />
        protected override float PlatformGetMiterLimit()
        {
            float result;
            Status status = GDIPlus.GdipGetPenMiterLimit(nativeObject, out result);
            GDIPlus.CheckStatus(status);
            return result;
        }

        /// <inheritdoc />
        protected override PenAlignment PlatformGetPenAlignment()
        {
            var status = GDIPlus.GdipGetPenMode(m_resource, out var retval);
            GDIPlus.CheckStatus(status);
            return retval;
        }

        /// <inheritdoc />
        protected override PenType PlatformGetPenType()
        {
            PenType type;
            Status status = GDIPlus.GdipGetPenFillType(nativeObject, out type);
            GDIPlus.CheckStatus(status);

            return type;
        }

        /// <inheritdoc />
        protected override LineCap PlatformGetStartCap()
        {
            LineCap retval;
            Status status = GDIPlus.GdipGetPenStartCap(nativeObject, out retval);
            GDIPlus.CheckStatus(status);

            return retval;
        }

        /// <inheritdoc />
        protected override Matrix PlatformGetTransform()
        {
            var matrix = new Matrix();
            Status status = GDIPlus.GdipGetPenTransform(nativeObject, matrix.nativeMatrix);
            GDIPlus.CheckStatus(status);

            return matrix;
        }

        /// <inheritdoc />
        protected override float PlatformGetWidth()
        {
            float f;
            Status status = GDIPlus.GdipGetPenWidth(nativeObject, out f);
            GDIPlus.CheckStatus(status);
            return f;
        }

        /// <inheritdoc />
        protected override void PlatformMultipleTransdorm(Matrix matrix, MatrixOrder order)
        {
            Status status = GDIPlus.GdipMultiplyPenTransform(nativeObject, matrix.nativeMatrix, order);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformResetTransform()
        {
            Status status = GDIPlus.GdipResetPenTransform(nativeObject);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformRotateTransform(float angle, MatrixOrder order)
        {
            Status status = GDIPlus.GdipRotatePenTransform(nativeObject, angle, order);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformScaleTransform(float sx, float sy, MatrixOrder order)
        {
            Status status = GDIPlus.GdipScalePenTransform(nativeObject, sx, sy, order);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetBrush(Brush value)
        {
            Status status = GDIPlus.GdipSetPenBrushFill(nativeObject, value.nativeObject);
            GDIPlus.CheckStatus(status);
            m_color = Color.Empty;
        }

        /// <inheritdoc />
        protected override void PlatformSetColor(Color value)
        {
            Status status = GDIPlus.GdipSetPenColor(nativeObject, value.ToArgb());
            GDIPlus.CheckStatus(status);
            m_color = value;
        }

        /// <inheritdoc />
        protected override void PlatformSetCompoundArray(float[] value)
        {
            Status status = GDIPlus.GdipSetPenCompoundArray(nativeObject, value, value.Length);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetCustomEndCap(CustomLineCap value)
        {
            Status status = GDIPlus.GdipSetPenCustomEndCap(nativeObject, value.nativeObject);
            GDIPlus.CheckStatus(status);
            m_endCap = value;
        }

        /// <inheritdoc />
        protected override void PlatformSetCustomStartCap(CustomLineCap value)
        {
            Status status = GDIPlus.GdipSetPenCustomStartCap(nativeObject, value.nativeObject);
            GDIPlus.CheckStatus(status);
            m_startCap = value;
        }

        /// <inheritdoc />
        protected override void PlatformSetDashCap(DashCap value)
        {
            Status status = GDIPlus.GdipSetPenDashCap197819(nativeObject, value);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetDashOffset(float value)
        {
            Status status = GDIPlus.GdipSetPenDashOffset(nativeObject, value);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetDashPattern(float[] value)
        {
            var length = value.Length;
            if (length == 0)
                throw new ArgumentException("Invalid parameter.");
            foreach (var val in value)
            {
                if (val <= 0)
                    throw new ArgumentException("Invalid parameter.");
            }

            Status status = GDIPlus.GdipSetPenDashArray(nativeObject, value, value.Length);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetDashStyle(DashStyle value)
        {
            Status status = GDIPlus.GdipSetPenDashStyle(nativeObject, value);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetEndCap(LineCap value)
        {
            Status status = GDIPlus.GdipSetPenEndCap(nativeObject, value);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetLineCap(LineCap startCap, LineCap endCap, DashCap dashCap)
        {
            Status status = GDIPlus.GdipSetPenLineCap197819(nativeObject, startCap, endCap, dashCap);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetLineJoin(LineJoin value)
        {
            Status status = GDIPlus.GdipSetPenLineJoin(nativeObject, value);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetMiterLimit(float value)
        {
            Status status = GDIPlus.GdipSetPenMiterLimit(nativeObject, value);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetPenAlignment(PenAlignment alignment)
        {
            Status status = GDIPlus.GdipSetPenMode(nativeObject, value);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetStartCap(LineCap value)
        {
            Status status = GDIPlus.GdipSetPenStartCap(nativeObject, value);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetTransform(Matrix value)
        {
            Status status = GDIPlus.GdipSetPenTransform(nativeObject, value.nativeMatrix);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSetWidth(float value)
        {
            Status status = GDIPlus.GdipSetPenWidth(nativeObject, value);
            GDIPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformTranslateTransform(float dx, float dy, MatrixOrder order)
        {
            Status status = GDIPlus.GdipTranslatePenTransform(nativeObject, dx, dy, order);
            GDIPlus.CheckStatus(status);
        }
        #endregion
    }
}