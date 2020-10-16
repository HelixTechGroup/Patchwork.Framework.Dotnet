using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.DrawingCore;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Platform.Interop.GdiPlus;
using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;
using SkiaSharp;
using EncoderParameters = System.Drawing.Imaging.EncoderParameters;

namespace Patchwork.Framework.Platform.Rendering
{
    public class GdiImage : Image<GpImage>
    {
        /// <inheritdoc />
        protected override object PlatformClone()
        {
            if (Core.Environment.OS.IsType(OsType.Windows) && stream != null)
                return CloneFromStream();

            var status = GdiPlus.GdipCloneImage(m_resource, out var newImage);
            GdiPlus.CheckStatus(status);

            if (this is Bitmap)
                return new Bitmap(newImage);
            else
                return new Metafile(newImage);
        }

        /// <inheritdoc />
        protected override RectangleF PlatformGetBounds(ref GraphicsUnit pageUnit)
        {
            var status = GdiPlus.GdipGetImageBounds(m_resource, out var source, ref pageUnit);
            GdiPlus.CheckStatus(status);

            return source;
        }

        /// <inheritdoc />
        protected override EncoderParameters PlatformGetEncoderParameterList(Guid encoder)
        {
            var status = GdiPlus.GdipGetEncoderParameterListSize(nativeObject, ref encoder, out var sz);
            GdiPlus.CheckStatus(status);

            var rawEPList = Marshal.AllocHGlobal((int)sz);
            EncoderParameters eps;

            try
            {
                status = GdiPlus.GdipGetEncoderParameterList(nativeObject, ref encoder, sz, rawEPList);
                eps = EncoderParameters.FromNativePtr(rawEPList);
                GdiPlus.CheckStatus(status);
            }
            finally
            {
                Marshal.FreeHGlobal(rawEPList);
            }

            return eps;
        }

        /// <inheritdoc />
        protected override int PlatformGetFrameCount(FrameDimension dimension)
        {
            var guid = dimension.Guid;

            var status = GdiPlus.GdipImageGetFrameCount(nativeObject, ref guid, out var count);
            GdiPlus.CheckStatus(status);

            return (int)count;
        }

        /// <inheritdoc />
        protected override PropertyItem PlatformGetPropertyItem(int propid)
        {
            var item = new PropertyItem();
            var gdipProperty = new GdipPropertyItem();

            var status = GdiPlus.GdipGetPropertyItemSize(nativeObject,
                                                     propid,
                                                     out var propSize);
            GdiPlus.CheckStatus(status);

            /* Get PropertyItem */
            var property = Marshal.AllocHGlobal((int)propSize);
            try
            {
                status = GdiPlus.GdipGetPropertyItem(nativeObject, propid, propSize, property);
                GdiPlus.CheckStatus(status);
                gdipProperty = (GdipPropertyItem)Marshal.PtrToStructure(property,
                                                                        typeof(GdipPropertyItem));
                GdipPropertyItem.MarshalTo(gdipProperty, item);
            }
            finally
            {
                Marshal.FreeHGlobal(property);
            }

            return item;
        }

        /// <inheritdoc />
        protected override Image PlatformGetThumbnail(int thumbWidth, int thumbHeight, Image<IntPtr>.GetThumbnailImageAbort callback, IntPtr callbackData)
        {
            Image ThumbNail = new Bitmap(thumbWidth, thumbHeight);
            using var g = Graphics.FromImage(ThumbNail);
            var status = GdiPlus.GdipDrawImageRectRectI(g.nativeObject,
                                                        nativeObject,
                                                        0,
                                                        0,
                                                        thumbWidth,
                                                        thumbHeight,
                                                        0,
                                                        0,
                                                        Width,
                                                        Height,
                                                        GraphicsUnit.Pixel,
                                                        IntPtr.Zero,
                                                        null,
                                                        IntPtr.Zero);

            GdiPlus.CheckStatus(status);

            return ThumbNail;
        }

        /// <inheritdoc />
        protected override void PlatformRemovePropertyItem(int propid)
        {
            var status = GdiPlus.GdipRemovePropertyItem(nativeObject, propid);
            GdiPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformRotateFlip(RotateFlipType rotateFlipType)
        {
            var status = GdiPlus.GdipImageRotateFlip(nativeObject, rotateFlipType);
            GdiPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformSave(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            GpStatus st;
            var guid = encoder.Clsid;

            if (encoderParams == null)
                st = GdiPlus.GdipSaveImageToFile(nativeObject, filename, ref guid, IntPtr.Zero);
            else
            {
                var nativeEncoderParams = encoderParams.ToNativePtr();
                st = GdiPlus.GdipSaveImageToFile(nativeObject, filename, ref guid, nativeEncoderParams);
                Marshal.FreeHGlobal(nativeEncoderParams);
            }

            GdiPlus.CheckStatus(st);
        }

        /// <inheritdoc />
        protected override void PlatformSave(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            GpStatus st;
            IntPtr nativeEncoderParams;
            var guid = encoder.Clsid;

            if (encoderParams == null)
                nativeEncoderParams = IntPtr.Zero;
            else
                nativeEncoderParams = encoderParams.ToNativePtr();

            try
            {
                if (Core.Environment.OS.IsUnixBased)
                {
                    GdiPlus.GdiPlusStreamHelper sh = new GdiPlus.GdiPlusStreamHelper(stream, false);
                    st = GdiPlus.GdipSaveImageToDelegate_linux(nativeObject,
                                                               sh.GetBytesDelegate,
                                                               sh.PutBytesDelegate,
                                                               sh.SeekDelegate,
                                                               sh.CloseDelegate,
                                                               sh.SizeDelegate,
                                                               ref guid,
                                                               nativeEncoderParams);
                }
                else
                {
                    st = GdiPlus.GdipSaveImageToStream(new HandleRef(this, nativeObject),
                                                       new ComIStreamWrapper(stream),
                                                       ref guid,
                                                       new HandleRef(encoderParams, nativeEncoderParams));
                }
            }
            finally
            {
                if (nativeEncoderParams != IntPtr.Zero)
                    Marshal.FreeHGlobal(nativeEncoderParams);
            }

            GdiPlus.CheckStatus(st);
        }

        /// <inheritdoc />
        protected override void PlatformSaveAdd(EncoderParameters encoderParams)
        {
            var nativeEncoderParams = encoderParams.ToNativePtr();
            var st = GdiPlus.GdipSaveAdd(nativeObject, nativeEncoderParams);
            Marshal.FreeHGlobal(nativeEncoderParams);
            GdiPlus.CheckStatus(st);
        }

        /// <inheritdoc />
        protected override void PlatformSaveAdd(Image image, EncoderParameters encoderParams)
        {
            var nativeEncoderParams = encoderParams.ToNativePtr();
            var st = GdiPlus.GdipSaveAddImage(nativeObject, image.Resource, nativeEncoderParams);
            Marshal.FreeHGlobal(nativeEncoderParams);
            GdiPlus.CheckStatus(st);
        }

        /// <inheritdoc />
        protected override int PlatformSelectActiveFrame(FrameDimension dimension, int frameIndex)
        {
            var guid = dimension.Guid;
            var st = GdiPlus.GdipImageSelectActiveFrame(nativeObject, ref guid, frameIndex);

            GdiPlus.CheckStatus(st);

            return frameIndex;
        }

        /// <inheritdoc />
        protected override int PlatformGetFlags()
        {
            var status = GdiPlus.GdipGetImageFlags(nativeObject, out var flags);
            GdiPlus.CheckStatus(status);
            return (int)flags;
        }

        /// <inheritdoc />
        protected override Guid[] PlatformGetFrameDimensionsList()
        {
            var status = GdiPlus.GdipImageGetFrameDimensionsCount(nativeObject, out var found);
            GdiPlus.CheckStatus(status);
            var guid = new Guid[found];
            status = GdiPlus.GdipImageGetFrameDimensionsList(nativeObject, guid, found);
            GdiPlus.CheckStatus(status);
            return guid;
        }

        /// <inheritdoc />
        protected override int PlatformGetHeight()
        {
            var status = GdiPlus.GdipGetImageHeight(nativeObject, out var height);
            GdiPlus.CheckStatus(status);

            return (int)height;
        }

        /// <inheritdoc />
        protected override float PlatformGetHorizontalResolution()
        {
            var status = GdiPlus.GdipGetImageHorizontalResolution(nativeObject, out var resolution);
            GdiPlus.CheckStatus(status);

            return resolution;
        }

        /// <inheritdoc />
        protected override SizeF PlatformGetPhysicalDimension()
        {
            float width, height;
            var status = GdiPlus.GdipGetImageDimension(nativeObject, out width, out height);
            GdiPlus.CheckStatus(status);

            return new SizeF(width, height);
        }

        /// <inheritdoc />
        protected override PixelFormat PlatformGetPixelFormat()
        {
            var status = GdiPlus.GdipGetImagePixelFormat(nativeObject, out  var pixFormat);
            GdiPlus.CheckStatus(status);

            return pixFormat;
        }

        /// <inheritdoc />
        protected override int[] PlatformGetPropertyIdList()
        {
            var status = GdiPlus.GdipGetPropertyCount(nativeObject,
                                                      out var propNumbers);
            GdiPlus.CheckStatus(status);

            var idList = new int[propNumbers];
            status = GdiPlus.GdipGetPropertyIdList(nativeObject,
                                                   propNumbers,
                                                   idList);
            GdiPlus.CheckStatus(status);

            return idList;
        }

        /// <inheritdoc />
        protected override PropertyItem[] PlatformGetPropertyItems()
        {
            PropertyItem[] items;
            var gdipProperty = new GdipPropertyItem();

            var status = GdiPlus.GdipGetPropertySize(nativeObject, out var propsSize, out var propNums);
            GdiPlus.CheckStatus(status);

            items = new PropertyItem[propNums];

            if (propNums == 0)
                return items;

            /* Get PropertyItem list*/
            var properties = Marshal.AllocHGlobal(propsSize * propNums);
            try
            {
                status = GdiPlus.GdipGetAllPropertyItems(nativeObject,
                                                         propsSize,
                                                         propNums,
                                                         properties);
                GdiPlus.CheckStatus(status);

                var propSize = Marshal.SizeOf(gdipProperty);
                var propPtr = properties;

                for (var i = 0; i < propNums; i++, propPtr = new IntPtr(propPtr.ToInt64() + propSize))
                {
                    gdipProperty = (GdipPropertyItem)Marshal.PtrToStructure(propPtr, typeof(GdipPropertyItem));
                    items[i] = new PropertyItem();
                    GdipPropertyItem.MarshalTo(gdipProperty, items[i]);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(properties);
            }

            return items;
        }

        /// <inheritdoc />
        protected override ImageFormat platformGetRawFormat()
        {
            Guid guid;
            var st = GdiPlus.GdipGetImageRawFormat(nativeObject, out guid);

            GdiPlus.CheckStatus(st);
            return new ImageFormat(guid);
        }

        /// <inheritdoc />
        protected override float PlatformGetVerticalResolution()
        {
            float resolution;

            var status = GdiPlus.GdipGetImageVerticalResolution(nativeObject, out resolution);
            GdiPlus.CheckStatus(status);

            return resolution;
        }

        /// <inheritdoc />
        protected override int PlatformGetWidth()
        {
            uint width;
            var status = GdiPlus.GdipGetImageWidth(nativeObject, out width);
            GdiPlus.CheckStatus(status);

            return (int)width;
        }

        /// <inheritdoc />
        protected override IntPtr PlatformInitFromStream(Stream steam)
        {
            var st = GpStatus.Ok;
            var imagePtr = IntPtr.Zero; ;

            if (Core.Environment.OS.IsUnixBased)
            {
                // Unix, with libgdiplus
                // We use a custom API for this, because there's no easy way
                // to get the Stream down to libgdiplus.  So, we wrap the stream
                // with a set of delegates.
                GdiPlus.GdiPlusStreamHelper sh = new GdiPlus.GdiPlusStreamHelper(stream, true);

                st = GdiPlus.GdipLoadImageFromDelegate_linux(sh.GetHeaderDelegate,
                                                             sh.GetBytesDelegate,
                                                             sh.PutBytesDelegate,
                                                             sh.SeekDelegate,
                                                             sh.CloseDelegate,
                                                             sh.SizeDelegate,
                                                             out imagePtr);
            }
            else
                st = GdiPlus.GdipLoadImageFromStream(new ComIStreamWrapper(stream), out imagePtr);

            return st == GpStatus.Ok ? imagePtr : IntPtr.Zero;
        }

        /// <inheritdoc />
        protected override Bitmap PlatformFromHbitmap(IntPtr hbitmap, IntPtr hpalette)
        {
            var st = GdiPlus.GdipCreateBitmapFromHBITMAP(hbitmap, hpalette, out var imagePtr);

            GdiPlus.CheckStatus(st);
            return new Bitmap(imagePtr);
        }

        /// <inheritdoc />
        protected override Image PlatformFromFile(string filename, bool useEmbeddedColorManagement)
        {
            IntPtr imagePtr;
            GpStatus st;

            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            if (useEmbeddedColorManagement)
                st = GdiPlus.GdipLoadImageFromFileICM(filename, out imagePtr);
            else
                st = GdiPlus.GdipLoadImageFromFile(filename, out imagePtr);
            GdiPlus.CheckStatus(st);

            return CreateFromHandle(imagePtr);
        }

        /// <inheritdoc />
        protected override Image PlatformLoadFromStream(Stream steam, bool keepAlive)
        {
            if(stream == null)
            throw new ArgumentNullException("stream");

            var img = CreateFromHandle(InitFromStream(stream));

            // Under Windows, we may need to keep a reference on the stream as long as the image is alive
            // (GDI+ seems to use a lazy loader)
            if (keepAlive && Core.Environment.OS.IsType(OsType.Windows))
                img.stream = stream;

            return img;
        }

        protected override ColorPalette PlatformGetPalette()
        {
            var ret = new ColorPalette();
            var st = GdiPlus.GdipGetImagePaletteSize(nativeObject, out var bytes);
            GdiPlus.CheckStatus(st);
            var palette_data = Marshal.AllocHGlobal(bytes);
            try
            {
                st = GdiPlus.GdipGetImagePalette(nativeObject, palette_data, bytes);
                GdiPlus.CheckStatus(st);
                ret.setFromGDIPalette(palette_data);
                return ret;
            }

            finally
            {
                Marshal.FreeHGlobal(palette_data);
            }
        }

        protected override ColorPalette PlatformSetPalette(ColorPalette palette)
        {
            if (palette == null) throw new ArgumentNullException("palette");
            var palette_data = palette.getGDIPalette();
            if (palette_data == IntPtr.Zero) return palette;

            try
            {
                var st = GdiPlus.GdipSetImagePalette(nativeObject, palette_data);
                GdiPlus.CheckStatus(st);
            }

            finally
            {
                Marshal.FreeHGlobal(palette_data);
            }
        }
        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            if (GdiPlus.GdiPlusToken != 0 && nativeObject != IntPtr.Zero)
            {
                var status = GdiPlus.GdipDisposeImage(nativeObject);
                // dispose the stream (set under Win32 only if SD owns the stream) and ...
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }

                // ... set nativeObject to null before (possibly) throwing an exception
                nativeObject = IntPtr.Zero;
                GdiPlus.CheckStatus(status);
            }

            base.DisposeUnmanagedResources();
        }

        protected Image CreateFromHandle(IntPtr handle)
        {
            GdiPlus.ImageType type;
            GdiPlus.CheckStatus(GdiPlus.GdipGetImageType(handle, out type));
            switch (type)
            {
                case GdiPlus.ImageType.Bitmap:
                    return new Bitmap(handle);
                case GdiPlus.ImageType.Metafile:
                    return new Metafile(handle);
                default:
                    throw new NotSupportedException("Unknown image type.");
            }
        }
    }
}
