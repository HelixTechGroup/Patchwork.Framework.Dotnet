//
// System.Drawing.Image.cs
//
// Authors: 	Christian Meyer (Christian.Meyer@cs.tum.edu)
// 		Alexandre Pigolkine (pigolkine@gmx.de)
//		Jordi Mas i Hernandez (jordi@ximian.com)
//		Sanjay Gupta (gsanjay@novell.com)
//		Ravindra (rkumar@novell.com)
//		Sebastien Pouliot  <sebastien@ximian.com>
//
// Copyright (C) 2002 Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004, 2007 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#region Usings
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Patchwork.Framework;
using Patchwork.Framework.Environment;
using HandleRef = System.Runtime.InteropServices.HandleRef;
#endregion

namespace System.Drawing
{
    [Serializable]
    [ComVisible(true)]
    [Editor("System.Drawing.Design.ImageEditor, System.Drawing.Design", typeof(UITypeEditor))]
    [TypeConverter(typeof(ImageConverter))]
    [ImmutableObject(true)]
    public abstract class Image<TNative> : NResource<TNative>, ISerializable
    {
        #region Delegates
        public delegate bool GetThumbnailImageAbort();
        #endregion

        #region Members
        // when using MS GDI+ and IStream we must ensure the stream stays alive for all the life of the Image
        // http://groups.google.com/group/microsoft.public.win32.programmer.gdi/browse_thread/thread/4967097db1469a27/4d36385b83532126?lnk=st&q=IStream+gdi&rnum=3&hl=en#4d36385b83532126
        internal Stream stream;
        private object tag;
        #endregion

        #region Properties
        // properties	
        [Browsable(false)]
        public int Flags
        {
            get { return PlatformGetFlags(); }
        }

        [Browsable(false)]
        public Guid[] FrameDimensionsList
        {
            get { return PlatformGetFrameDimensionsList(); }
        }

        [DefaultValue(false)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Height
        {
            get { return PlatformGetHeight(); }
        }

        public float HorizontalResolution
        {
            get { return PlatformGetHorizontalResolution(); }
        }

        [Browsable(false)]
        public ColorPalette Palette
        {
            get { return PlatformGetPalette(); }
            set { PlatformSetPalette(value); }
        }


        public SizeF PhysicalDimension
        {
            get { return PlatformGetPhysicalDimension(); }
        }

        public PixelFormat PixelFormat
        {
            get { return PlatformGetPixelFormat(); }
        }

        [Browsable(false)]
        public int[] PropertyIdList
        {
            get { return PlatformGetPropertyIdList(); }
        }

        [Browsable(false)]
        public PropertyItem[] PropertyItems
        {
            get { return PlatformGetPropertyItems(); }
        }

        public ImageFormat RawFormat
        {
            get { return platformGetRawFormat(); }
        }

        public Size Size
        {
            get { return new Size(Width, Height); }
        }

        [DefaultValue(null)]
        [LocalizableAttribute(false)]
        [BindableAttribute(true)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public float VerticalResolution
        {
            get { return PlatformGetVerticalResolution(); }
        }

        [DefaultValue(false)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Width
        {
            get { return PlatformGetWidth(); }
        }
        #endregion

        // constructor
        internal Image() { }

        internal Image(SerializationInfo info, StreamingContext context)
        {
            foreach (var serEnum in info)
            {
                if (string.Compare(serEnum.Name, "Data", true) == 0)
                {
                    var bytes = (byte[])serEnum.Value;

                    if (bytes != null)
                    {
                        var ms = new MemoryStream(bytes);
                        m_resource = InitFromStream(ms);
                        // under Win32 stream is owned by SD/GDI+ code
                        if (Core.Environment.OS.IsType(OsType.Windows))
                            stream = ms;
                    }
                }
            }
        }

        #region Methods
        // public methods
        // static
        public Image FromFile(string filename, bool useEmbeddedColorManagement)
        {
            return PlatformFromFile(filename, useEmbeddedColorManagement);
        }

        public Bitmap FromHbitmap(IntPtr hbitmap, IntPtr hpalette)
        {
            return PlatformFromHbitmap(hbitmap, hpalette);
        }

        public static int GetPixelFormatSize(PixelFormat pixfmt)
        {
            var result = 0;
            switch (pixfmt)
            {
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    result = 16;
                    break;
                case PixelFormat.Format1bppIndexed:
                    result = 1;
                    break;
                case PixelFormat.Format24bppRgb:
                    result = 24;
                    break;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    result = 32;
                    break;
                case PixelFormat.Format48bppRgb:
                    result = 48;
                    break;
                case PixelFormat.Format4bppIndexed:
                    result = 4;
                    break;
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    result = 64;
                    break;
                case PixelFormat.Format8bppIndexed:
                    result = 8;
                    break;
            }

            return result;
        }

        public static bool IsAlphaPixelFormat(PixelFormat pixfmt)
        {
            var result = false;
            switch (pixfmt)
            {
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    result = true;
                    break;
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format48bppRgb:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    result = false;
                    break;
            }

            return result;
        }

        public static bool IsCanonicalPixelFormat(PixelFormat pixfmt)
        {
            return (pixfmt & PixelFormat.Canonical) != 0;
        }

        public static bool IsExtendedPixelFormat(PixelFormat pixfmt)
        {
            return (pixfmt & PixelFormat.Extended) != 0;
        }

        // non-static	
        public RectangleF GetBounds(ref GraphicsUnit pageUnit)
        {
            return PlatformGetBounds(ref pageUnit);
        }

        protected abstract RectangleF PlatformGetBounds(ref GraphicsUnit pageUnit);

        public EncoderParameters GetEncoderParameterList(Guid encoder)
        {
            return PlatformGetEncoderParameterList(encoder);
        }

        protected abstract EncoderParameters PlatformGetEncoderParameterList(Guid encoder);

        public int GetFrameCount(FrameDimension dimension)
        {
            return PlatformGetFrameCount(dimension);
        }

        protected abstract int PlatformGetFrameCount(FrameDimension dimension);

        public PropertyItem GetPropertyItem(int propid)
        {
            return PlatformGetPropertyItem(propid);
        }

        protected abstract PropertyItem PlatformGetPropertyItem(int propid);

        public Image GetThumbnailImage(int thumbWidth, int thumbHeight, Image<IntPtr>.GetThumbnailImageAbort callback, IntPtr callbackData)
        {
            if (thumbWidth <= 0 || thumbHeight <= 0)
                throw new OutOfMemoryException("Invalid thumbnail size");

            return PlatformGetThumbnail(thumbWidth, thumbHeight, callback, callbackData);
        }

        protected abstract Image PlatformGetThumbnail(int thumbWidth,
                                                      int thumbHeight,
                                                      Image<IntPtr>.GetThumbnailImageAbort callback,
                                                      IntPtr callbackData);

        public void RemovePropertyItem(int propid)
        {
            PlatformRemovePropertyItem(propid);
        }

        protected abstract void PlatformRemovePropertyItem(int propid);

        public void RotateFlip(RotateFlipType rotateFlipType)
        {
            PlatformRotateFlip(rotateFlipType);
        }

        protected abstract void PlatformRotateFlip(RotateFlipType rotateFlipType);

        public void Save(string filename, ImageFormat format = null)
        {
            if (format == null)
                format = RawFormat;
            var encoder = findEncoderForFormat(format);
            if (encoder == null)
            {
                // second chance
                encoder = findEncoderForFormat(RawFormat);
                if (encoder == null)
                {
                    var msg = string.Format("No codec available for saving format '{0}'.", format.Guid);
                    throw new ArgumentException(msg, "format");
                }
            }

            Save(filename, encoder, null);
        }

        public void Save(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            PlatformSave(filename, encoder, encoderParams);
        }

        protected abstract void PlatformSave(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams);

        public void Save(Stream stream, ImageFormat format)
        {
            var encoder = findEncoderForFormat(format);

            if (encoder == null)
                throw new ArgumentException("No codec available for format:" + format.Guid);

            Save(stream, encoder, null);
        }

        public void Save(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            PlatformSave(stream, encoder, encoderParams);
        }

        protected abstract void PlatformSave(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams);

        public void SaveAdd(EncoderParameters encoderParams)
        {
            PlatformSaveAdd(encoderParams);
        }

        protected abstract void PlatformSaveAdd(EncoderParameters encoderParams);

        public void SaveAdd(Image image, EncoderParameters encoderParams)
        {
            PlatformSaveAdd(image, encoderParams);
        }

        protected abstract void PlatformSaveAdd(Image image, EncoderParameters encoderParams);

        public int SelectActiveFrame(FrameDimension dimension, int frameIndex)
        {
            return PlatformSelectActiveFrame(dimension, frameIndex);
        }

        protected abstract int PlatformSelectActiveFrame(FrameDimension dimension, int frameIndex);

        public void SetPropertyItem(PropertyItem propitem)
        {
            throw new NotImplementedException();
            /*
                    GdipPropertyItem pi = new GdipPropertyItem ();
                    GdipPropertyItem.MarshalTo (pi, propitem);
                    unsafe {
                        Status status = GDIPlus.GdipSetPropertyItem (nativeObject, &pi);
                        
                        GDIPlus.CheckStatus (status);
                    }
            */
        }

        protected abstract int PlatformGetFlags();

        protected abstract Guid[] PlatformGetFrameDimensionsList();

        protected abstract int PlatformGetHeight();

        protected abstract float PlatformGetHorizontalResolution();

        protected abstract SizeF PlatformGetPhysicalDimension();

        protected abstract PixelFormat PlatformGetPixelFormat();

        protected abstract int[] PlatformGetPropertyIdList();

        protected abstract PropertyItem[] PlatformGetPropertyItems();

        protected abstract ImageFormat platformGetRawFormat();

        protected abstract float PlatformGetVerticalResolution();

        protected abstract int PlatformGetWidth();

        protected abstract Bitmap PlatformFromHbitmap(IntPtr hbitmap, IntPtr hpalette);

        // FIXME - find out how metafiles (another decoder-only codec) are handled
        void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
        {
            using(var ms = new MemoryStream())
            {
                // Icon is a decoder-only codec
                if (RawFormat.Equals(ImageFormat.Icon))
                    Save(ms, ImageFormat.Png);
                else
                    Save(ms, RawFormat);
                si.AddValue("Data", ms.ToArray());
            }
        }

        protected abstract Image PlatformFromFile(string filename, bool useEmbeddedColorManagement);

        internal Image LoadFromStream(Stream stream, bool keepAlive)
        {
            return PlatformLoadFromStream(stream, keepAlive);
        }

        protected abstract Image PlatformLoadFromStream(Stream steam, bool keepAlive);

        internal IntPtr InitFromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentException("stream");

            // Seeking required
            if (!stream.CanSeek)
            {
                var buffer = new byte[256];
                var index = 0;
                int count;

                do
                {
                    if (buffer.Length < index + 256)
                    {
                        var newBuffer = new byte[buffer.Length * 2];
                        Array.Copy(buffer, newBuffer, buffer.Length);
                        buffer = newBuffer;
                    }

                    count = stream.Read(buffer, index, 256);
                    index += count;
                } while (count != 0);

                stream = new MemoryStream(buffer, 0, index);
            }

            return PlatformInitFromStream(stream);
        }

        protected abstract IntPtr PlatformInitFromStream(Stream stream);

        internal ImageCodecInfo findEncoderForFormat(ImageFormat format)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo encoder = null;

            if (format.Guid.Equals(ImageFormat.MemoryBmp.Guid))
                format = ImageFormat.Png;

            /* Look for the right encoder for our format*/
            for (var i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].FormatID.Equals(format.Guid))
                {
                    encoder = encoders[i];
                    break;
                }
            }

            return encoder;
        }

        protected abstract ColorPalette PlatformGetPalette();

        protected abstract ColorPalette PlatformSetPalette(ColorPalette palette);

        // On win32, when cloning images that were originally created from a stream, we need to
        // clone both the image and the stream to make sure the gc doesn't kill it
        // (when using MS GDI+ and IStream we must ensure the stream stays alive for all the life of the Image)
        protected object CloneFromStream()
        {
            var bytes = new byte [stream.Length];
            var ms = new MemoryStream(bytes);
            var count = stream.Length < 4096 ? (int)stream.Length : 4096;
            var buffer = new byte[count];
            stream.Position = 0;
            do
            {
                count = stream.Read(buffer, 0, count);
                ms.Write(buffer, 0, count);
            } while (count == 4096);

            var newimage = IntPtr.Zero;
            newimage = InitFromStream(ms);

            //if (this is Bitmap)
            //    return new Bitmap(newimage, ms);
            //return new Metafile(newimage, ms);
        }
        #endregion
    }

    [Serializable]
    [ComVisible(true)]
    [Editor("System.Drawing.Design.ImageEditor, System.Drawing.Design", typeof(UITypeEditor))]
    [TypeConverter(typeof(ImageConverter))]
    [ImmutableObject(true)]
    public abstract class Image : Image<IntPtr> { }
}