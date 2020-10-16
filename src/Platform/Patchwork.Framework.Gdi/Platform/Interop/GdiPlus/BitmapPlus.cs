using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Patchwork.Framework.Platform.Interop.GdiPlus
{
    public class BitmapPlus : ImagePlus
    {
        public BitmapPlus(string filename)
        {
            Initialize();
            GpBitmap bitmap = new GpBitmap();

            lastResult = NativeMethods.GdiPlus.GdipCreateBitmapFromFileICM(filename, out bitmap);

            SetNativeImage((GpImage)(IntPtr)bitmap);
        }

        private void Initialize()
        {
            //    GdiPlusInitializer.Initialize();
        }


        public BitmapPlus(IStream stream)
        {
            Initialize();
            GpBitmap bitmap = new GpBitmap();

            lastResult = NativeMethods.GdiPlus.GdipCreateBitmapFromStreamICM(stream, out bitmap);

            SetNativeImage((GpImage)(IntPtr)bitmap);
        }


        public BitmapPlus(int width, int height, int stride, PixelFormat format, IntPtr scan0)
        {
            Initialize();
            GpBitmap bitmap = new GpBitmap();

            lastResult = NativeMethods.GdiPlus.GdipCreateBitmapFromScan0(width,
                                                                         height,
                                                                         stride,
                                                                         format,
                                                                         scan0,
                                                                         out bitmap);

            SetNativeImage((GpImage)(IntPtr)bitmap);
        }


        public BitmapPlus(int width, int height, PixelFormat format)
        {
            Initialize();
            GpBitmap bitmap = new GpBitmap();

            lastResult = NativeMethods.GdiPlus.GdipCreateBitmapFromScan0(width,
                                                                         height,
                                                                         0,
                                                                         format,
                                                                         IntPtr.Zero,
                                                                         out bitmap);

            SetNativeImage((GpImage)(IntPtr)bitmap);
        }


        public BitmapPlus(int width, int height, GraphicsPlus target)
        {
            Initialize();
            GpBitmap bitmap = new GpBitmap();

            lastResult = NativeMethods.GdiPlus.GdipCreateBitmapFromGraphics(width,
                                                                            height,
                                                                            target.nativeGraphics,
                                                                            out bitmap);

            SetNativeImage((GpImage)(IntPtr)bitmap);
        }

        public BitmapPlus(HBITMAP hbm, IntPtr hpal)
        {
            Initialize();
            GpBitmap bitmap = new GpBitmap();

            lastResult = NativeMethods.GdiPlus.GdipCreateBitmapFromHBITMAP(hbm, hpal, out bitmap);

            SetNativeImage((GpImage)(IntPtr)bitmap);
        }

        public BitmapPlus(IntPtr hbitmap)
        {
            Initialize();
            IntPtr hpal = IntPtr.Zero;
            GpBitmap bitmap = new GpBitmap();

            lastResult = NativeMethods.GdiPlus.GdipCreateBitmapFromHBITMAP(hbitmap, hpal, out bitmap);

            SetNativeImage((GpImage)(IntPtr)bitmap);
        }




        public BitmapPlus FromFile(string filename)
        {
            return new BitmapPlus(filename);
        }


        public BitmapPlus FromStream(IStream stream)
        {
            return new BitmapPlus(stream);
        }



        public BitmapPlus FromHBITMAP(HBITMAP hbm, IntPtr hpal)
        {
            return new BitmapPlus(hbm, hpal);
        }



        public IntPtr GetHBITMAP(Color colorBackground)
        {

            HBITMAP hbmReturn;
            GpStatus status = NativeMethods.GdiPlus.GdipCreateHBITMAPFromBitmap((GpBitmap)(IntPtr)nativeImage, out hbmReturn, colorBackground.ToArgb());
            SetStatus(status);
            return hbmReturn.val;
        }



        public BitmapPlus(GpBitmap nativeBitmap)
        {
            lastResult = GpStatus.Ok;

            SetNativeImage((IntPtr)nativeBitmap);
        }


        public void LockBits(Rectangle rect, uint flags, PixelFormat format, BitmapData lockedBitmapData)
        {
            SetStatus(NativeMethods.GdiPlus.GdipBitmapLockBits(
                                                               (GpBitmap)(IntPtr)nativeImage,
                                                               rect,
                                                               flags,
                                                               format,
                                                               lockedBitmapData));
        }


        public void UnlockBits(BitmapData lockedBitmapData)
        {
            SetStatus(NativeMethods.GdiPlus.GdipBitmapUnlockBits(
                                                                 (GpBitmap)(IntPtr)nativeImage,
                                                                 lockedBitmapData));
        }


        public Color GetPixel(int x, int y)
        {
            int argb;
            SetStatus(NativeMethods.GdiPlus.GdipBitmapGetPixel((GpBitmap)(IntPtr)nativeImage, x, y, out argb));

            return Color.FromArgb(argb);
        }


        public void SetPixel(int x, int y, Color color)
        {
            SetStatus(NativeMethods.GdiPlus.GdipBitmapSetPixel((GpBitmap)(IntPtr)nativeImage, x, y, color.ToArgb()));
        }




    }
}
