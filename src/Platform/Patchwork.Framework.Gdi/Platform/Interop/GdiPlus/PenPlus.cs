using System;
using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;
using Color = System.Drawing.Color;

namespace Patchwork.Framework.Platform.Interop.GdiPlus
{
    public class PenPlus: IDisposable
    {
        public PenPlus(Color color, float width)
        {
            Unit unit = Unit. UnitWorld;
            nativePen = null;
            lastResult = NativeMethods.GdiPlus.GdipCreatePen1(color.ToArgb(), width, unit, out nativePen);
        }

        public PenPlus(BrushPlus brush, float width)
        {
            Unit unit = Unit.UnitWorld;
            nativePen = null;
            lastResult = NativeMethods.GdiPlus.GdipCreatePen2(brush.nativeBrush,  width, unit, out nativePen);
        }
        public PenPlus(Color color, float width, bool opaque)
        {
            int c = color.ToArgb();
            if (opaque) c |= (0xff << 24);
            Unit unit = Unit.UnitWorld;
            nativePen = null;
            lastResult = NativeMethods.GdiPlus.GdipCreatePen1(c, width, unit, out nativePen);
        }

        ~PenPlus()
        {
            Dispose(true);
        }

        public PenPlus Clone()
        {
            GpPen clonePen = null;

            lastResult = NativeMethods.GdiPlus.GdipClonePen(nativePen, out clonePen);

            return new PenPlus(clonePen, lastResult);
        }

        public void SetWidth(float width)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenWidth(nativePen, width));
        }

        private void SetStatus(GpStatus gpStatus)
        {
            if (gpStatus != GpStatus.Ok) throw GdiPlusStatusException.Exception(gpStatus);
        }

        public float GetWidth()
        {
            float width;

            SetStatus(NativeMethods.GdiPlus.GdipGetPenWidth(nativePen, out width));

            return width;
        }

        // Set/get line caps: start, end, and dash

        // Line cap and join APIs by using LineCap and LineJoin enums.

        public void SetLineCap(LineCap startCap,
                          LineCap endCap,
                          DashCap dashCap)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenLineCap197819(nativePen,
                                                                    startCap, endCap, dashCap));
        }

        public void SetStartCap(LineCap startCap)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenStartCap(nativePen, startCap));
        }

        public void SetEndCap(LineCap endCap)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenEndCap(nativePen, endCap));
        }

        //public void SetDashCap(DashCap dashCap)
        //{
        //    SetStatus(NativeMethods.GdipSetPenDashCap197819(nativePen,
        //                               dashCap));
        //}

        public LineCap GetStartCap()
        {
            LineCap startCap;

            SetStatus(NativeMethods.GdiPlus.GdipGetPenStartCap(nativePen, out startCap));

            return startCap;
        }

        public LineCap GetEndCap()
        {
            LineCap endCap;

            SetStatus(NativeMethods.GdiPlus.GdipGetPenEndCap(nativePen, out endCap));

            return endCap;
        }

        public DashCap GetDashCap()
        {
            DashCap dashCap;

            SetStatus(NativeMethods.GdiPlus.GdipGetPenDashCap197819(nativePen,
                                                                    out dashCap));

            return dashCap;
        }

        public void SetLineJoin(LineJoin lineJoin)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenLineJoin(nativePen, lineJoin));
        }

        public LineJoin GetLineJoin()
        {
            LineJoin lineJoin;

            SetStatus(NativeMethods.GdiPlus.GdipGetPenLineJoin(nativePen, out lineJoin));

            return lineJoin;
        }

        public void SetCustomStartCap(CustomLineCap customCap)
        {
            GpCustomLineCap nativeCap = new GpCustomLineCap();
            if (customCap != null)
                nativeCap = customCap.nativeCap;

            SetStatus(NativeMethods.GdiPlus.GdipSetPenCustomStartCap(nativePen,
                                                                     nativeCap));
        }

        public void GetCustomStartCap(out CustomLineCap customCap)
        {
            customCap = new CustomLineCap();
            SetStatus(NativeMethods.GdiPlus.GdipGetPenCustomStartCap(nativePen,
                                                                     out customCap.nativeCap));
        }

        public void SetCustomEndCap(CustomLineCap customCap)
        {
            GpCustomLineCap nativeCap = new GpCustomLineCap();
            if (customCap != null)
                nativeCap = customCap.nativeCap;

            SetStatus(NativeMethods.GdiPlus.GdipSetPenCustomEndCap(nativePen,
                                                                   nativeCap));
        }

        public void GetCustomEndCap(out CustomLineCap customCap)
        {
            customCap = new CustomLineCap();
            SetStatus(NativeMethods.GdiPlus.GdipGetPenCustomEndCap(nativePen,
                                                                   out customCap.nativeCap));
        }

        public void SetMiterLimit(float miterLimit)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenMiterLimit(nativePen,
                                                                 miterLimit));
        }

        public float GetMiterLimit()
        {
            float miterLimit;

            SetStatus(NativeMethods.GdiPlus.GdipGetPenMiterLimit(nativePen, out miterLimit));

            return miterLimit;
        }

        public void SetAlignment(PenAlignment penAlignment)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenMode(nativePen, penAlignment));
        }

        public PenAlignment GetAlignment()
        {
            PenAlignment penAlignment;

            SetStatus(NativeMethods.GdiPlus.GdipGetPenMode(nativePen, out penAlignment));

            return penAlignment;
        }

        //PenType GetPenType()
        //{
        //    PenType type;
        //    SetStatus(NativeMethods.GdipGetPenFillType(nativePen, out type));

        //    return type;
        //}

        public void SetColor(Color color)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenColor(nativePen,
                                                            color.ToArgb()));
        }

        public void SetBrush(BrushPlus brush)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenBrushFill(nativePen,
                                                                brush.nativeBrush));
        }

        public Color GetColor()
        {

            int argb;
            SetStatus(NativeMethods.GdiPlus.GdipGetPenColor(nativePen,  out argb));
            return Color.FromArgb(argb);
        }

        public DashStyle GetDashStyle()
        {
            DashStyle dashStyle;

            SetStatus(NativeMethods.GdiPlus.GdipGetPenDashStyle(nativePen, out dashStyle));

            return dashStyle;
        }

        public void SetDashStyle(DashStyle dashStyle)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenDashStyle(nativePen,
                                                                dashStyle));
        }

        public float GetDashOffset()
        {
            float dashOffset;

            SetStatus(NativeMethods.GdiPlus.GdipGetPenDashOffset(nativePen, out dashOffset));

            return dashOffset;
        }

        public void SetDashOffset(float dashOffset)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenDashOffset(nativePen,
                                                                 dashOffset));
        }

        public void SetDashPattern(float[] dashArray)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPenDashArray(nativePen,
                                                                dashArray,
                                                                dashArray.Length));
        }

        public int GetDashPatternCount()
        {
            int count = 0;

            SetStatus(NativeMethods.GdiPlus.GdipGetPenDashCount(nativePen, out count));

            return count;
        }

        public void GetDashPattern(float[] dashArray)
        {
            if (dashArray == null || dashArray.Length == 0)
                SetStatus(GpStatus.InvalidParameter);

            SetStatus(NativeMethods.GdiPlus.GdipGetPenDashArray(nativePen,
                                                                dashArray,
                                                                dashArray.Length));
        }


        public GpStatus GetLastStatus()
        {
            GpStatus lastStatus = lastResult;
            lastResult = GpStatus.Ok;

            return lastStatus;
        }




        protected PenPlus(GpPen nativePen, GpStatus status)
        {
            lastResult = status;
            SetNativePen(nativePen);
        }

        void SetNativePen(GpPen nativePen)
        {
            this.nativePen = nativePen;
        }


        internal GpPen nativePen;
        protected GpStatus lastResult;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            // free native resources if there are any.
            if ((IntPtr)nativePen!= IntPtr.Zero)
            {
                NativeMethods.GdiPlus.GdipDeletePen(nativePen);
                nativePen = new GpPen();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
