using System;
using System.Drawing;
using System.Drawing.Imaging;
using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;

namespace Patchwork.Framework.Platform.Interop.GdiPlus
{
    public class ImagePlus : IDisposable
    {

        public ImagePlus(string filename, bool useEmbeddedColorManagement)
        {
        }

        public ImagePlus(IStream stream, bool useEmbeddedColorManagement )
        {
            NativeMethods.GdiPlus.GdipLoadImageFromStream(stream, out nativeImage);
        }


        static public ImagePlus FromStream( IStream stream, bool useEmbeddedColorManagement )
        {
            return new ImagePlus(stream, useEmbeddedColorManagement);
        }


        public void GetPhysicalDimension(out SizeF size)
        {

            float width, height;

            SetStatus(NativeMethods.GdiPlus.GdipGetImageDimension(nativeImage, out width, out height));

            size = new SizeF(width, height);
        }

        public RectangleF GetBounds(out GraphicsUnit pageUnit)
        {
            RectangleF srcRect;
            SetStatus(NativeMethods.GdiPlus.GdipGetImageBounds(nativeImage, out srcRect, out pageUnit));
            return srcRect;
        }

        public uint GetWidth()
        {
            RectangleF rc = GetBounds(out var unit);
            
            return (uint)rc.Width;
        }

        public uint GetHeight()
        {
            RectangleF rc = GetBounds(out var unit);
            return (uint)rc.Height;
        }

        public void GetRawFormat(out Guid format)
        {
            SetStatus(NativeMethods.GdiPlus.GdipGetImageRawFormat(nativeImage, out format));
        }

        public PixelFormat GetPixelFormat()
        {

            PixelFormat format;

            SetStatus(NativeMethods.GdiPlus.GdipGetImagePixelFormat(nativeImage, out format));

            return format;

        }

        internal ImagePlus() { }

        internal ImagePlus(GpImage nativeImage, GpStatus status)
        {
            SetNativeImage(nativeImage);
        }

        internal void SetNativeImage(GpImage nativeImage)
        {
            this.nativeImage = nativeImage;
        }

        protected void SetStatus(GpStatus gpStatus)
        {
            if (gpStatus != GpStatus.Ok) throw GdiPlusStatusException.Exception(gpStatus);
        }

        internal GpImage nativeImage;
        protected GpStatus lastResult;
        protected GpStatus loadStatus;


        private ImagePlus(ImagePlus C)
        {
            NativeMethods.GdiPlus.GdipCloneImage(C.nativeImage, out this.nativeImage);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (nativeImage != IntPtr.Zero)
            {
                NativeMethods.GdiPlus.GdipDisposeImage(nativeImage);
                nativeImage = null;
            }
        }

        #endregion
    }
}
