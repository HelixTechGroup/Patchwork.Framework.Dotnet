using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Color = System.Drawing.Color;

namespace Patchwork.Framework.Platform.Interop.GdiPlus
{
    public class BrushPlus: IDisposable
    {
        ~BrushPlus()
        {
            Dispose(true);
        }

        internal virtual BrushPlus Clone()
        {
            GpBrush brush = new GpBrush();

            SetStatus(NativeMethods.GdiPlus.GdipCloneBrush(nativeBrush, out brush));

            BrushPlus newBrush = new BrushPlus(brush, lastResult);

            if (newBrush == null)
            {
                NativeMethods.GdiPlus.GdipDeleteBrush(brush);
            }

            return newBrush;
        }


        NativeMethods.GdiPlus.BrushType GetBrushType()
        {
            NativeMethods.GdiPlus.BrushType type = (NativeMethods.GdiPlus.BrushType)(-1);

            SetStatus(NativeMethods.GdiPlus.GdipGetBrushType(nativeBrush, out type));

            return type;
        }

        public GpStatus GetLastStatus()
        {
            GpStatus lastStatus = lastResult;
            lastResult = GpStatus.Ok;

            return lastStatus;
        }



        public BrushPlus()
        {
            SetStatus(GpStatus.NotImplemented);
        }




        public BrushPlus(GpBrush nativeBrush, GpStatus status)
        {
            lastResult = status;
            SetNativeBrush(nativeBrush);
        }

        public void SetNativeBrush(GpBrush nativeBrush)
        {
            this.nativeBrush = nativeBrush;
        }

        protected GpStatus SetStatus(GpStatus status)
        {
            if (status != GpStatus.Ok)
                return (lastResult = status);
            else
                return status;
        }

        internal GpBrush nativeBrush;
        protected GpStatus lastResult;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            // free native resources if there are any.
            if ((IntPtr)nativeBrush != IntPtr.Zero)
            {
                NativeMethods.GdiPlus.GdipDeleteBrush(nativeBrush);
                nativeBrush = new GpBrush();
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

    public class SolidBrushPlus : BrushPlus
    {
        public SolidBrushPlus()
        {
        }
        public SolidBrushPlus(Color color)
        {
            GpSolidFill brush;

            lastResult = NativeMethods.GdiPlus.GdipCreateSolidFill(color.ToArgb(), out brush);

            SetNativeBrush(brush);
        }
        public SolidBrushPlus(Color color, bool opaque)
        {
            GpSolidFill brush;

            int c = color.ToArgb();
            if (opaque) c |= (0xff << 24);
            lastResult = NativeMethods.GdiPlus.GdipCreateSolidFill(c, out brush);

            SetNativeBrush(brush);
        }

        GpStatus GetColor(out Color color)
        {
            int argb;

            SetStatus(NativeMethods.GdiPlus.GdipGetSolidFillColor((GpSolidFill)nativeBrush,
                                                                  out argb));

            color = Color.FromArgb(argb);

            return lastResult;
        }

        GpStatus SetColor(Color color)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetSolidFillColor((GpSolidFill)nativeBrush,
                                                                         color.ToArgb()));
        }


    }

    public class TextureBrushPlus : BrushPlus
    {
        public TextureBrushPlus() { }
        public TextureBrushPlus(ImagePlus image,
                     WrapMode wrapMode)
        {
            GpTexture texture;

            NativeMethods.GdiPlus.Unit unit;
            RectangleF rc = image.GetBounds(out unit);
            lastResult = NativeMethods.GdiPlus.GdipCreateTextureIA(
                                                                   image.nativeImage,
                                                                   new GpImageAttributes(),
                                                                   rc.X,
                                                                   rc.Y,
                                                                   rc.Width,
                                                                   rc.Height,
                                                                   out texture);

            SetNativeBrush(texture);
        }

        // When creating a texture brush from a metafile image, the dstRect
        // is used to specify the size that the metafile image should be
        // rendered at in the device units of the destination graphics.
        // It is NOT used to crop the metafile image, so only the width 
        // and height values matter for metafiles.

        TextureBrushPlus(ImagePlus image,
                     WrapMode wrapMode,
                     RectangleF dstRect)
        {
            GpTexture texture;

            lastResult = NativeMethods.GdiPlus.GdipCreateTexture2(
                                                                  image.nativeImage,
                                                                  wrapMode,
                                                                  dstRect.X,
                                                                  dstRect.Y,
                                                                  dstRect.Width,
                                                                  dstRect.Height,
                                                                  out texture);

            SetNativeBrush(texture);
        }

        public TextureBrushPlus(ImagePlus image,
                     RectangleF dstRect,
                     ImageAttributesPlus imageAttributes)
        {
            GpTexture texture;

            lastResult = NativeMethods.GdiPlus.GdipCreateTextureIA(
                                                                   image.nativeImage,
                                                                   (imageAttributes != null) ? imageAttributes.nativeImageAttr : new GpImageAttributes(),
                                                                   dstRect.X,
                                                                   dstRect.Y,
                                                                   dstRect.Width,
                                                                   dstRect.Height,
                                                                   out texture
                                                                  );

            SetNativeBrush(texture);
        }

        TextureBrushPlus(ImagePlus image,
                     Rectangle dstRect,
                     ImageAttributesPlus imageAttributes)
        {
            GpTexture texture;

            lastResult = NativeMethods.GdiPlus.GdipCreateTextureIAI(
                                                                    image.nativeImage,
                                                                    (imageAttributes != null) ? imageAttributes.nativeImageAttr : new GpImageAttributes(),
                                                                    dstRect.X,
                                                                    dstRect.Y,
                                                                    dstRect.Width,
                                                                    dstRect.Height,
                                                                    out texture
                                                                   );

            SetNativeBrush(texture);
        }

        TextureBrushPlus(
            ImagePlus image,
            WrapMode wrapMode,

            Rectangle dstRect
        )
        {
            GpTexture texture;

            lastResult = NativeMethods.GdiPlus.GdipCreateTexture2I(
                                                                   image.nativeImage,
                                                                   wrapMode,
                                                                   dstRect.X,
                                                                   dstRect.Y,
                                                                   dstRect.Width,
                                                                   dstRect.Height,
                                                                   out texture);

            SetNativeBrush(texture);
        }

        TextureBrushPlus(ImagePlus image,
                     WrapMode wrapMode,
                     float dstX,
                     float dstY,
                     float dstWidth,
                     float dstHeight)
        {
            GpTexture texture;

            lastResult = NativeMethods.GdiPlus.GdipCreateTexture2(
                                                                  image.nativeImage,
                                                                  wrapMode,
                                                                  dstX,
                                                                  dstY,
                                                                  dstWidth,
                                                                  dstHeight,
                                                                  out texture);

            SetNativeBrush(texture);
        }

        TextureBrushPlus(ImagePlus image,
                     WrapMode wrapMode,
                     int dstX,
                     int dstY,
                     int dstWidth,
                     int dstHeight)
        {
            GpTexture texture;

            lastResult = NativeMethods.GdiPlus.GdipCreateTexture2I(
                                                                   image.nativeImage,
                                                                   wrapMode,
                                                                   dstX,
                                                                   dstY,
                                                                   dstWidth,
                                                                   dstHeight,
                                                                   out texture);

            SetNativeBrush(texture);
        }
        GpStatus SetWrapMode(WrapMode wrapMode)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetTextureWrapMode((GpTexture)nativeBrush,
                                                                          wrapMode));
        }

        WrapMode GetWrapMode()
        {
            WrapMode wrapMode;

            SetStatus(NativeMethods.GdiPlus.GdipGetTextureWrapMode((GpTexture)nativeBrush,
                                                                   out wrapMode));
            return wrapMode;
        }

        ImagePlus GetImage()
        {
            GpImage image;

            SetStatus(NativeMethods.GdiPlus.GdipGetTextureImage((GpTexture)nativeBrush,
                                                                out image));

            ImagePlus retimage = new ImagePlus(image, lastResult);

            return retimage;
        }


    }

    public class LinearGradientBrush : BrushPlus
    {
        public LinearGradientBrush() { }
        public LinearGradientBrush(PointF point1,
                            PointF point2,
                            Color color1,
                            Color color2)
        {
            GpLineGradient brush;

            lastResult = NativeMethods.GdiPlus.GdipCreateLineBrush(ref point1,
                                                                   ref point2,
                                                                   color1.ToArgb(),
                                                                   color2.ToArgb(),
                                                                   WrapMode.WrapModeTile,
                                                                   out brush);

            SetNativeBrush(brush);
        }

        LinearGradientBrush(Point point1,
                            Point point2,
                            Color color1,
                            Color color2)
        {
            GpLineGradient brush;

            lastResult = NativeMethods.GdiPlus.GdipCreateLineBrushI(ref point1,
                                                                    ref point2,
                                                                    color1.ToArgb(),
                                                                    color2.ToArgb(),
                                                                    WrapMode.Tile,
                                                                    out brush);

            SetNativeBrush(brush);
        }

        #region not completely supported!
        //public LinearGradientBrush(RectangleF rect,
        //                    Color color1,
        //                    Color color2,
        //                    LinearGradientMode mode)
        //{
        //    GpLineGradient brush;

        //    lastResult = GdiPlus.GdipCreateLineBrushFromRect(ref rect,
        //                                                         color1.ToArgb(),
        //                                                         color2.ToArgb(),
        //                                                         mode,
        //                                                         WrapMode.WrapModeTile,
        //                                                         out brush);

        //    SetNativeBrush(brush);
        //}

        //LinearGradientBrush(Rectangle rect,
        //                    Color color1,
        //                    Color color2,
        //                    LinearGradientMode mode)
        //{
        //    GpLineGradient brush;

        //    lastResult = GdiPlus.GdipCreateLineBrushFromRectI(ref rect,
        //                                                          color1.ToArgb(),
        //                                                          color2.ToArgb(),
        //                                                          mode,
        //                                                          WrapMode.WrapModeTile,
        //                                                          out brush);

        //    SetNativeBrush(brush);
        //}

        //LinearGradientBrush(RectangleF rect,
        //                    Color color1,
        //                    Color color2,
        //                    float angle,
        //                    bool isAngleScalable)
        //{
        //    GpLineGradient brush;

        //    lastResult = GdiPlus.GdipCreateLineBrushFromRectWithAngle(ref rect,
        //                                                                  color1.ToArgb(),
        //                                                                  color2.ToArgb(),
        //                                                                  angle,
        //                                                                  isAngleScalable,
        //                                                                  WrapMode.WrapModeTile,
        //                                                                  out brush);

        //    SetNativeBrush(brush);
        //}

        //LinearGradientBrush(Rectangle rect,
        //                    Color color1,
        //                    Color color2,
        //                    float angle,
        //                    bool isAngleScalable)
        //{
        //    GpLineGradient brush = new GpLineGradient();

        //    lastResult = GdiPlus.GdipCreateLineBrushFromRectWithAngleI(ref rect,
        //                                                                   color1.ToArgb(),
        //                                                                   color2.ToArgb(),
        //                                                                   angle,
        //                                                                   isAngleScalable,
        //                                                                   WrapMode.WrapModeTile,
        //                                                                   out brush);

        //    SetNativeBrush(brush);
        //}
        #endregion

        GpStatus SetLinearColors(Color color1,
                               Color color2)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetLineColors((GpLineGradient)nativeBrush,
                                                                     color1.ToArgb(),
                                                                     color2.ToArgb()));
        }

        GpStatus GetLinearColors(Color[] colors)
        {
            int[] argb = new int[2];


            GpStatus status = SetStatus(NativeMethods.GdiPlus.GdipGetLineColors((GpLineGradient)nativeBrush, argb));

            if (status == GpStatus.Ok)
            {
                colors[0] = Color.FromArgb(argb[0]);
                colors[1] = Color.FromArgb(argb[1]);
            }

            return status;
        }

        GpStatus GetRectangle(out RectangleF rect)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipGetLineRect((GpLineGradient)nativeBrush, out rect));
        }

        GpStatus GetRectangle(out Rectangle rect)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipGetLineRectI((GpLineGradient)nativeBrush, out rect));
        }

        GpStatus SetGammaCorrection(bool useGammaCorrection)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetLineGammaCorrection((GpLineGradient)nativeBrush,
                                                                              useGammaCorrection));
        }

        bool GetGammaCorrection()
        {
            bool useGammaCorrection;

            SetStatus(NativeMethods.GdiPlus.GdipGetLineGammaCorrection((GpLineGradient)nativeBrush,
                                                                       out useGammaCorrection));

            return useGammaCorrection;
        }

        int GetBlendCount()
        {
            int count = 0;

            SetStatus(NativeMethods.GdiPlus.GdipGetLineBlendCount((GpLineGradient)
                                                                  nativeBrush,
                                                                  out count));

            return count;
        }

        GpStatus SetBlend(float[] blendFactors,
                        float[] blendPositions)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetLineBlend((GpLineGradient)
                                                                    nativeBrush,
                                                                    blendFactors,
                                                                    blendPositions,
                                                                    blendFactors.Length));
        }

        GpStatus GetBlend(float[] blendFactors,
                        float[] blendPositions)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipGetLineBlend((GpLineGradient)nativeBrush,
                                                                    blendFactors,
                                                                    blendPositions,
                                                                    blendFactors.Length));
        }

        int GetInterpolationColorCount()
        {
            int count = 0;

            SetStatus(NativeMethods.GdiPlus.GdipGetLinePresetBlendCount((GpLineGradient)
                                                                        nativeBrush,
                                                                        out count));

            return count;
        }

        GpStatus SetInterpolationColors(Color[] presetColors,
                                      float[] blendPositions)
        {
            int count = presetColors.Length;
            int[] argbs = new int[count];

            for (int i = 0; i < count; i++)
            {
                argbs[i] = presetColors[i].ToArgb();
            }

            GpStatus status = SetStatus(NativeMethods.GdiPlus.GdipSetLinePresetBlend(
                                                                                     (GpLineGradient)nativeBrush,
                                                                                     argbs,
                                                                                     blendPositions,
                                                                                     argbs.Length));
            return status;
        }

        GpStatus GetInterpolationColors(Color[] presetColors,
                                      float[] blendPositions)
        {
            int count = presetColors.Length;

            int[] argbs = new int[count];

            GpStatus status = SetStatus(NativeMethods.GdiPlus.GdipGetLinePresetBlend((GpLineGradient)nativeBrush,
                                                                                     argbs,
                                                                                     blendPositions,
                                                                                     argbs.Length));
            if (status == GpStatus.Ok)
            {
                for (int i = 0; i < count; i++)
                {
                    presetColors[i] = Color.FromArgb(argbs[i]);
                }
            }


            return status;
        }

        GpStatus SetBlendBellShape(float focus,
                                 float scale)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetLineSigmaBlend((GpLineGradient)nativeBrush, focus, scale));
        }

        GpStatus SetBlendTriangularShape(
            float focus,
            float scale)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetLineLinearBlend((GpLineGradient)nativeBrush, focus, scale));
        }
        GpStatus SetWrapMode(WrapMode wrapMode)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetLineWrapMode((GpLineGradient)nativeBrush,
                                                                       wrapMode));
        }

        WrapMode GetWrapMode()
        {
            WrapMode wrapMode;

            SetStatus(NativeMethods.GdiPlus.GdipGetLineWrapMode((GpLineGradient)
                                                                nativeBrush,
                                                                out wrapMode));

            return wrapMode;
        }
    }

    public class HatchBrush : BrushPlus
    {
        public HatchBrush(HatchStyle hatchStyle,
                   Color foreColor,
                   Color backColor)
        {

            GpHatch brush = new GpHatch();

            lastResult = NativeMethods.GdiPlus.GdipCreateHatchBrush(hatchStyle,
                                                                    foreColor.ToArgb(),
                                                                    backColor.ToArgb(),
                                                                    out brush);
            SetNativeBrush(brush);
        }

        HatchStyle GetHatchStyle()
        {
            HatchStyle hatchStyle;

            SetStatus(NativeMethods.GdiPlus.GdipGetHatchStyle((GpHatch)nativeBrush,
                                                              out hatchStyle));

            return hatchStyle;
        }

        GpStatus GetForegroundColor(out Color color)
        {
            int argb;

            GpStatus status = SetStatus(NativeMethods.GdiPlus.GdipGetHatchForegroundColor(
                                                                                          (GpHatch)nativeBrush,
                                                                                          out argb));

            color = Color.FromArgb(argb);

            return status;
        }

        GpStatus GetBackgroundColor(out Color color)
        {
            int argb;

            GpStatus status = SetStatus(NativeMethods.GdiPlus.GdipGetHatchBackgroundColor(
                                                                                          (GpHatch)nativeBrush,
                                                                                          out argb));

            color = Color.FromArgb(argb);

            return status;
        }

    }

    public class PathGradientBrush : BrushPlus
    {
        public PathGradientBrush(
            PointF[] points,
            WrapMode wrapMode)
        {
            GpPathGradient brush = new GpPathGradient();

            lastResult = NativeMethods.GdiPlus.GdipCreatePathGradient(
                                                                      points, points.Length,
                                                                      wrapMode, out brush);
            SetNativeBrush(brush);
        }

        public PathGradientBrush(
        Point[] points,
        WrapMode wrapMode)
        {
            GpPathGradient brush = new GpPathGradient();

            lastResult = NativeMethods.GdiPlus.GdipCreatePathGradientI(
                                                                       points, points.Length,
                                                                       wrapMode, out brush);

            SetNativeBrush(brush);
        }

        public PathGradientBrush(
        GraphicsPath path
        )
        {
            GpPathGradient brush = new GpPathGradient();

            lastResult = NativeMethods.GdiPlus.GdipCreatePathGradientFromPath(
                                                                              path.nativePath, out brush);
            SetNativeBrush(brush);
        }

        public GpStatus GetCenterColor(out Color color)
        {
            int argb;

            SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientCenterColor(
                                                                           (GpPathGradient)nativeBrush, out argb));

            color = Color.FromArgb(argb);

            return lastResult;
        }

        public GpStatus SetCenterColor(Color color)
        {
            SetStatus(NativeMethods.GdiPlus.GdipSetPathGradientCenterColor(
                                                                           (GpPathGradient)nativeBrush,
                                                                           color.ToArgb()));

            return lastResult;
        }

        public int GetPointCount()
        {
            int count;

            SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientPointCount(
                                                                          (GpPathGradient)nativeBrush, out count));

            return count;
        }

        public int GetSurroundColorCount()
        {
            int count;

            SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientSurroundColorCount(
                                                                                  (GpPathGradient)nativeBrush, out count));

            return count;
        }

        public GpStatus GetSurroundColors(Color[] colors, ref int count)
        {

            int count1;

            SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientSurroundColorCount(
                                                                                  (GpPathGradient)nativeBrush, out count1));

            if (lastResult != GpStatus.Ok)
                return lastResult;

            if ((count < count1) || (count1 <= 0))
                return SetStatus(GpStatus.InsufficientBuffer);

            int[] argbs = new int[count1];

            SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientSurroundColorsWithCount(
                                                                                       (GpPathGradient)nativeBrush, argbs, out count1));

            if (lastResult == GpStatus.Ok)
            {
                for (int i = 0; i < count1; i++)
                {
                    colors[i] = Color.FromArgb(argbs[i]);
                }
                count = count1;
            }

            return lastResult;
        }

        public GpStatus SetSurroundColors(Color[] colors,
                             ref int count)
        {
            int count1 = GetPointCount();

            if ((count > count1) || (count1 <= 0))
                return SetStatus(GpStatus.InvalidParameter);

            count1 = count;

            int[] argbs = new int[count1];

            for (int i = 0; i < count1; i++)
                argbs[i] = colors[i].ToArgb();

            SetStatus(NativeMethods.GdiPlus.GdipSetPathGradientSurroundColorsWithCount(
                                                                                       (GpPathGradient)nativeBrush, argbs, ref count1));

            if (lastResult == GpStatus.Ok)
                count = count1;


            return lastResult;
        }

        public GpStatus GetGraphicsPath(out GraphicsPath path)
        {
            path = new GraphicsPath();
            return SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientPath(
                                                                           (GpPathGradient)nativeBrush, out path.nativePath));
        }

        public GpStatus SetGraphicsPath(GraphicsPath path)
        {
            if (path == null)
                return SetStatus(GpStatus.InvalidParameter);

            return SetStatus(NativeMethods.GdiPlus.GdipSetPathGradientPath(
                                                                           (GpPathGradient)nativeBrush, path.nativePath));
        }

        public GpStatus GetCenterPoint(out PointF point)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientCenterPoint(
                                                                                  (GpPathGradient)nativeBrush,
                                                                                  out point));
        }

        public GpStatus GetCenterPoint(out Point point)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientCenterPointI(
                                                                                   (GpPathGradient)nativeBrush,
                                                                                   out point));
        }

        public GpStatus SetCenterPoint(PointF point)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetPathGradientCenterPoint(
                                                                                  (GpPathGradient)nativeBrush,
                                                                                  ref point));
        }

        public GpStatus SetCenterPoint(Point point)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetPathGradientCenterPointI(
                                                                                   (GpPathGradient)nativeBrush,
                                                                                   ref point));
        }

        public GpStatus GetRectangle(out RectangleF rect)
        {
            rect = new RectangleF();
            return SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientRect(
                                                                           (GpPathGradient)nativeBrush, out rect));
        }

        public GpStatus GetRectangle(out Rectangle rect)
        {
            rect = new Rectangle();
            return SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientRectI(
                                                                            (GpPathGradient)nativeBrush, out rect));
        }

        public GpStatus SetGammaCorrection(bool useGammaCorrection)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetPathGradientGammaCorrection(
                                                                                      (GpPathGradient)nativeBrush, useGammaCorrection));
        }

        public bool GetGammaCorrection()
        {
            bool useGammaCorrection;

            SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientGammaCorrection(
                                                                               (GpPathGradient)nativeBrush, out useGammaCorrection));

            return useGammaCorrection;
        }

        public int GetBlendCount()
        {
            int count = 0;

            SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientBlendCount(
                                                                          (GpPathGradient)nativeBrush, out count));

            return count;
        }

        public GpStatus GetBlend(float[] blendFactors,
                    float[] blendPositions)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientBlend(
                                                                            (GpPathGradient)nativeBrush,
                                                                            blendFactors, blendPositions, blendFactors.Length));
        }

        public GpStatus SetBlend(float[] blendFactors,
                    float[] blendPositions)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetPathGradientBlend(
                                                                            (GpPathGradient)nativeBrush,
                                                                            blendFactors, blendPositions, blendFactors.Length));
        }

        public int GetInterpolationColorCount()
        {
            int count = 0;

            SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientPresetBlendCount(
                                                                                (GpPathGradient)nativeBrush, out count));

            return count;
        }

        public GpStatus SetInterpolationColors(Color[] presetColors,
                                  float[] blendPositions)
        {

            int[] argbs = new int[presetColors.Length];
            for (int i = 0; i < argbs.Length; i++)
            {
                argbs[i] = presetColors[i].ToArgb();
            }

            GpStatus status = SetStatus(NativeMethods.GdiPlus.
                                                      GdipSetPathGradientPresetBlend(
                                                                                     (GpPathGradient)nativeBrush,
                                                                                     argbs,
                                                                                     blendPositions,
                                                                                     argbs.Length));
            return status;
        }

        public GpStatus GetInterpolationColors(Color[] presetColors,
                                  float[] blendPositions)
        {
            int[] argbs = new int[presetColors.Length];

            GpStatus status = SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientPresetBlend(
                                                                                             (GpPathGradient)nativeBrush,
                                                                                             argbs,
                                                                                             blendPositions,
                                                                                             argbs.Length));

            for (int i = 0; i < presetColors.Length; i++)
            {
                presetColors[i] = Color.FromArgb(argbs[i]);
            }

            return status;
        }

        public GpStatus SetBlendBellShape(float focus,
                             float scale)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetPathGradientSigmaBlend(
                                                                                 (GpPathGradient)nativeBrush, focus, scale));
        }

        public GpStatus SetBlendTriangularShape(
        float focus,
        float scale
    )
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetPathGradientLinearBlend(
                                                                                  (GpPathGradient)nativeBrush, focus, scale));
        }

        //GpStatus GetFocusScales(OUT float* xScale, 
        //                      OUT float* yScale) 
        //{
        //    return SetStatus(NativeMethods.GdipGetPathGradientFocusScales(
        //                        (GpPathGradient ) nativeBrush, xScale, yScale));
        //}

        //GpStatus SetFocusScales(float xScale,
        //                      float yScale)
        //{
        //    return SetStatus(NativeMethods.GdipSetPathGradientFocusScales(
        //                        (GpPathGradient ) nativeBrush, xScale, yScale));
        //}

        public WrapMode GetWrapMode()
        {
            WrapMode wrapMode;

            SetStatus(NativeMethods.GdiPlus.GdipGetPathGradientWrapMode(
                                                                        (GpPathGradient)nativeBrush, out wrapMode));

            return wrapMode;
        }

        public GpStatus SetWrapMode(WrapMode wrapMode)
        {
            return SetStatus(NativeMethods.GdiPlus.GdipSetPathGradientWrapMode(
                                                                               (GpPathGradient)nativeBrush, wrapMode));
        }




        public PathGradientBrush()
        {
        }
    }
}
