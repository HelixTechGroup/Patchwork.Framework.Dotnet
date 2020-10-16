//
// System.Drawing.TextureBrush.cs
//
// Authors:
//   Dennis Hayes (dennish@Raytek.com)
//   Ravindra (rkumar@novell.com)
//   Sebastien Pouliot  <sebastien@ximian.com>
//
// (C) 2002 Ximian, Inc
// Copyright (C) 2004,2006-2007 Novell, Inc (http://www.novell.com)
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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
#endregion

namespace System.Drawing
{
    public abstract class TextureBrush<TNative> : Brush<TNative>
    {
        #region Members
        protected ImageAttributes m_attributes;
        protected Image m_image;
        protected WrapMode m_mode;
        protected Rectangle m_rectangle;
        protected RectangleF m_rectangleF;
        #endregion

        #region Properties
        // properties

        public Image Image
        {
            get { return PlatformGetImage(); }
        }

        public Matrix Transform
        {
            get { return PlatformGetTransform(); }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Transform");

                PlatformSetTransform(value);
            }
        }

        public WrapMode WrapMode
        {
            get { return PlatformGetWrapMode(); }
            set
            {
                if (value < WrapMode.Tile || value > WrapMode.Clamp)
                    throw new InvalidEnumArgumentException("WrapMode");

                PlatformSetWrapMode(value);
            }
        }
        #endregion

        public TextureBrush(Image bitmap) :
            this(bitmap, WrapMode.Tile) { }

        public TextureBrush(Image image, Rectangle dstRect) :
            this(image, WrapMode.Tile, dstRect) { }

        public TextureBrush(Image image, RectangleF dstRect) :
            this(image, WrapMode.Tile, dstRect) { }

        public TextureBrush(Image image, WrapMode wrapMode = WrapMode.None)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (wrapMode < WrapMode.Tile || wrapMode > WrapMode.Clamp)
                throw new InvalidEnumArgumentException("WrapMode");

            m_image = image;
            m_mode = wrapMode;
            Initialize();
        }

        public TextureBrush(Image image, Rectangle dstRect = new Rectangle(), ImageAttributes imageAttr = null)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            m_image = image;
            m_rectangle = dstRect;
            m_attributes = imageAttr;
            Initialize();
        }

        public TextureBrush(Image image, RectangleF dstRect = new RectangleF(), ImageAttributes imageAttr = null)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            m_image = image;
            m_rectangleF = dstRect;
            m_attributes = imageAttr;
            Initialize();
        }

        public TextureBrush(Image image, WrapMode wrapMode = WrapMode.None, Rectangle dstRect = new Rectangle())
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (wrapMode < WrapMode.Tile || wrapMode > WrapMode.Clamp)
                throw new InvalidEnumArgumentException("WrapMode");

            m_image = image;
            m_mode = wrapMode;
            m_rectangle = dstRect;
            Initialize();
        }

        public TextureBrush(Image image, WrapMode wrapMode = WrapMode.None, RectangleF dstRect = new RectangleF())
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (wrapMode < WrapMode.Tile || wrapMode > WrapMode.Clamp)
                throw new InvalidEnumArgumentException("WrapMode");

            m_image = image;
            m_mode = wrapMode;
            m_rectangleF = dstRect;
            Initialize();
        }

        internal TextureBrush(TNative ptr) :
            base(ptr) { }

        #region Methods
        // public methods

        public void MultiplyTransform(Matrix matrix, MatrixOrder order = MatrixOrder.Prepend)
        {
            PlatformMultiplyTransform(matrix, order);
        }

        public void ResetTransform()
        {
            PlatformResetTransform();
        }

        public void RotateTransform(float angle, MatrixOrder order = MatrixOrder.Prepend)
        {
            PlatformRotateTransform(angle, order);
        }

        public void ScaleTransform(float sx, float sy)
        {
            ScaleTransform(sx, sy, MatrixOrder.Prepend);
        }

        public void ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            PlatformScaleTransform(sx, sy, order);
        }

        public void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Prepend)
        {
            PlatformTranslateTransform(dx, dy, order);
        }

        protected abstract WrapMode PlatformGetWrapMode();

        protected abstract WrapMode PlatformSetWrapMode(WrapMode mode);

        protected abstract void PlatformMultiplyTransform(Matrix matrix, MatrixOrder order);

        protected abstract void PlatformResetTransform();

        protected abstract void PlatformRotateTransform(float angle, MatrixOrder order);

        protected abstract void PlatformScaleTransform(float sx, float sy, MatrixOrder order);

        protected abstract void PlatformTranslateTransform(float dx, float dy, MatrixOrder order);

        protected abstract Image PlatformGetImage();

        protected abstract Matrix PlatformGetTransform();

        protected abstract Matrix PlatformSetTransform(Matrix transform);
        #endregion
    }
}