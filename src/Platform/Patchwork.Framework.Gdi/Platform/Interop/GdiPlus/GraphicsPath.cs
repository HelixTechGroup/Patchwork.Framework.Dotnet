using System;
using System.Drawing;
using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;

namespace Patchwork.Framework.Platform.Interop.GdiPlus
{
    public class GraphicsPath : IDisposable
    {
        public GraphicsPath() : this(FillMode.Alternate) { }
        public GraphicsPath(FillMode fillMode)
        {
            nativePath = null;
            lastResult = NativeMethods.GdiPlus.GdipCreatePath(fillMode, out nativePath);
        }


        ~GraphicsPath()
        {
            Dispose(true);
        }

        public void Clear()
        {
            NativeMethods.GdiPlus.GdipDeletePath(nativePath);
            NativeMethods.GdiPlus.GdipCreatePath(FillMode.Alternate, out nativePath);
        }


        private void SetStatus(GpStatus gpStatus)
        {
            if (gpStatus != GpStatus.Ok) throw GdiPlusStatusException.Exception(gpStatus);
        }

        public GraphicsPath Clone()
        {
            GpPath clonepath = null;

            SetStatus(NativeMethods.GdiPlus.GdipClonePath(nativePath, out clonepath));

            return new GraphicsPath(clonepath);
        }

        // Reset the path object to empty (and fill mode to FillModeAlternate)

        public void Reset()
        {
            SetStatus(NativeMethods.GdiPlus.GdipResetPath(nativePath));
        }

        private FillMode GetFillMode()
        {
            FillMode fillmode = FillMode.Alternate;

            SetStatus(NativeMethods.GdiPlus.GdipGetPathFillMode(nativePath, out fillmode));

            return fillmode;
        }

        private void SetFillMode(FillMode fillmode)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPathFillMode(nativePath, fillmode));
        }

        public FillMode FillMode
        {
            get
            {
                return GetFillMode();
            }
            set
            {
                SetFillMode(value);
            }
        }

        public void StartFigure()
        {
            SetStatus(NativeMethods.GdiPlus.GdipStartPathFigure(nativePath));
        }

        public void CloseFigure()
        {
            SetStatus(NativeMethods.GdiPlus.GdipClosePathFigure(nativePath));
        }

        public void CloseAllFigures()
        {
            SetStatus(NativeMethods.GdiPlus.GdipClosePathFigures(nativePath));
        }


        public void GetLastPoint(out PointF lastPoint)
        {
            SetStatus(NativeMethods.GdiPlus.GdipGetPathLastPoint(nativePath,
                                                                 out lastPoint));
        }

        public void AddLine(PointF pt1, PointF pt2)
        {
            AddLine(pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        public void AddLine(float x1, float y1, float x2, float y2)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathLine(nativePath, x1, y1, x2, y2));
        }

        public void AddLines(PointF[] points)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathLine2(nativePath, points, points.Length));
        }

        public void AddLine(Point p1, Point p2)
        {
            AddLines(new Point[] { p1, p2 });
        }

        public void AddLine(int x1, int y1, int x2, int y2)
        {
            Point p1 = new Point(x1, y1);
            Point p2 = new Point(x2, y1);
            AddLine(p1, p2);
        }

        public void AddLinesArray(Point[] points)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathLine2I(nativePath, points, points.Length));
        }

        public void AddLines(params Point[] points)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathLine2I(nativePath, points, points.Length));
        }

        public void AddArc(RectangleF rect, float startAngle, float sweepAngle)
        {
            AddArc(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void AddArc(
            float x,
            float y,
            float width,
            float height,
            float startAngle,
            float sweepAngle)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathArc(nativePath, x, y, width, height, startAngle, sweepAngle));
        }

        public void AddArc(Rectangle rect,  float startAngle, float sweepAngle)
        {
            AddArc(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void AddArc(int x,
                   int y,
                   int width,
                   int height,
                   float startAngle,
                   float sweepAngle)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathArcI(nativePath, x,y,width,height,startAngle,sweepAngle));
        }



        public void AddBeziers(PointF[] points)
        {
            int count = points.Length;
            count -= (count - 4) % 3;
            SetStatus(NativeMethods.GdiPlus.GdipAddPathBeziers(nativePath, points, count));
        }



        public void AddBeziers(Point[] points)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathBeziersI(nativePath, points, points.Length));
        }


        public void AddRectangle(RectangleF rect)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathPolygon(nativePath,
                                                               new PointF[]
                                                               {
                                                                   new PointF(rect.X, rect.Y),
                                                                   new PointF(rect.X, rect.Y + rect.Height),
                                                                   new PointF(rect.X + rect.Width, rect.Y + rect.Height),
                                                                   new PointF(rect.X + rect.Width, rect.Y),
                                                               },
                                                               4));
        }

        public void AddRectangles(RectangleF[] rects)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathRectangles(nativePath, rects, rects.Length));
        }


        public void AddRectangles(Rectangle[] rects)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathRectanglesI(nativePath,  rects, rects.Length));
        }

        public void AddEllipse(RectangleF rect)
        {
            AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void AddEllipse(float x, float y, float width, float height)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathEllipse(nativePath, x, y, width, height));
        }

        public void AddEllipse(Rectangle rect)
        {
            AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void AddEllipse(int x, int y, int width, int height)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathEllipseI(nativePath, x, y, width, height));
        }

        public void AddPie(RectangleF rect, float startAngle, float sweepAngle)
        {
            AddPie(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void AddPie(float x,
                   float y,
                   float width,
                   float height,
                   float startAngle,
                   float sweepAngle)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathPie(nativePath, x, y, width,
                                                           height, startAngle,
                                                           sweepAngle));
        }

        public void AddPie(Rectangle rect,
                   float startAngle,
                   float sweepAngle)
        {
            AddPie(rect.X,
                          rect.Y,
                          rect.Width,
                          rect.Height,
                          startAngle,
                          sweepAngle);
        }

        public void AddPie(int x,
                   int y,
                   int width,
                   int height,
                   float startAngle,
                   float sweepAngle)
        {
            SetStatus(NativeMethods.GdiPlus.GdipAddPathPieI(nativePath, x, y, width, height, startAngle, sweepAngle));
        }


        public void AddPath(GraphicsPath addingPath, bool connect)
        {
            GpPath nativePath2 = null;
            if (addingPath != null) nativePath2 = addingPath.nativePath;

            SetStatus(NativeMethods.GdiPlus.GdipAddPathPath(nativePath, nativePath2, connect));
        }



        public int GetPointCount()
        {
            int count = 0;

            SetStatus(NativeMethods.GdiPlus.GdipGetPointCount(nativePath, out count));

            return count;
        }

        public void GetPathTypes(byte[] types)
        {
            SetStatus(NativeMethods.GdiPlus.GdipGetPathTypes(nativePath, types, types.Length));
        }

        public void GetPathPoints(PointF[] points)
        {
            SetStatus(NativeMethods.GdiPlus.GdipGetPathPoints(nativePath, points, points.Length));
        }

        public void GetPathPoints(Point[] points)
        {
            SetStatus(NativeMethods.GdiPlus.GdipGetPathPointsI(nativePath, points, points.Length));
        }

        public GpStatus GetLastStatus()
        {
            GpStatus lastStatus = lastResult;
            lastResult = GpStatus.Ok;

            return lastStatus;
        }

        public bool IsVisible(GraphicsPlus g, PointF point)
        {
            return IsVisible(g, point.X, point.Y);
        }

        public bool IsVisible(GraphicsPlus g, float x, float y)
        {
            bool ret;
            SetStatus(NativeMethods.GdiPlus.GdipIsVisiblePathPoint(nativePath, x, y, g.nativeGraphics, out ret));
            return ret;
        }

        public bool IsVisible(GraphicsPlus g, Point point)
        {
            return IsVisible(g, point.X, point.Y);
        }

        public bool IsVisible(GraphicsPlus g, int x, int y)
        {
            bool ret;
            NativeMethods.GdiPlus.GdipIsVisiblePathPoint(nativePath, x, y, g.nativeGraphics, out ret);
            return ret;
        }

        public bool IsOutlineVisible(GraphicsPlus g, PointF point, PenPlus pen)
        {
            return IsOutlineVisible(g, point.X, point.Y, pen);
        }

        public bool IsOutlineVisible(GraphicsPlus g, float x, float y, PenPlus pen)
        {
            bool ret;
            SetStatus(NativeMethods.GdiPlus.GdipIsOutlineVisiblePathPoint(nativePath, x, y, pen.nativePen, g.nativeGraphics, out ret));
            return ret;
        }

        public bool IsOutlineVisible(GraphicsPlus g, Point point, PenPlus pen)
        {
            return IsOutlineVisible(g, point.X, point.Y, pen);
        }



        public GraphicsPath(GraphicsPath path)
        {
            GpPath clonepath = null;
            SetStatus(NativeMethods.GdiPlus.GdipClonePath(path.nativePath, out clonepath));
            SetNativePath(clonepath);
        }

        protected GraphicsPath(GpPath nativePath)
        {
            lastResult = GpStatus.Ok;
            SetNativePath(nativePath);
        }

        void SetNativePath(GpPath nativePath)
        {
            this.nativePath = nativePath;
        }


        internal GpPath nativePath;
        protected GpStatus lastResult;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            if ((IntPtr)nativePath != IntPtr.Zero)
            {
                NativeMethods.GdiPlus.GdipDeletePath(nativePath);
                nativePath = new GpPath();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
