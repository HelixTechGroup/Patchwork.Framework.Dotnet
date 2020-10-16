//
// System.Drawing.Pen.cs
//
// Authors:
//   Miguel de Icaza (miguel@ximian.com)
//   Alexandre Pigolkine (pigolkine@gmx.de)
//   Duncan Mak (duncan@ximian.com)
//   Ravindra (rkumar@novell.com)
//
// Copyright (C) Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004,2006 Novell, Inc (http://www.novell.com)
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
#endregion

namespace System.Drawing
{
    public abstract class Pen : Pen<IntPtr>
    {
        /// <inheritdoc />
        public Pen(Brush brush) : base(brush) { }

        /// <inheritdoc />
        public Pen(Color color) : base(color) { }

        /// <inheritdoc />
        public Pen(Brush brush, float width) : base(brush, width) { }

        /// <inheritdoc />
        public Pen(Color color, float width) : base(color, width) { }
    }

    public abstract class Pen<TNative> : NResource<TNative>
    {
        #region Members
        protected Brush m_brush;
        protected Color m_color;
        protected CustomLineCap m_endCap;
        protected CustomLineCap m_startCap;
        protected float m_width;
        internal bool isModifiable = true;
        #endregion

        #region Properties
        //
        // Properties
        //
        public PenAlignment Alignment
        {
            get { return PlatformGetPenAlignment(); }

            set
            {
                if (value < PenAlignment.Center || value > PenAlignment.Right)
                    throw new InvalidEnumArgumentException("Alignment", (int)value, typeof(PenAlignment));

                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetPenAlignment(value);
            }
        }

        public Brush Brush
        {
            get { return PlatformGetBrush(); }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("Brush");
                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetBrush(value);
            }
        }

        public Color Color
        {
            get { return PlatformGetColor(); }

            set
            {
                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetColor(value);
            }
        }

        public float[] CompoundArray
        {
            get { return PlatformGetCompoundArray(); }

            set
            {
                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                var length = value.Length;
                if (length < 2)
                    throw new ArgumentException("Invalid parameter.");

                foreach (var val in value)
                {
                    if (val < 0 || val > 1)
                        throw new ArgumentException("Invalid parameter.");
                }

                PlatformSetCompoundArray(value);
            }
        }

        public CustomLineCap CustomEndCap
        {
            get { return PlatformGetCustomEndCap(); }

            set
            {
                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetCustomEndCap(value);
            }
        }

        public CustomLineCap CustomStartCap
        {
            get { return PlatformGetCustomStartCap(); }

            set
            {
                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetCustomStartCap(value);
            }
        }

        public DashCap DashCap
        {
            get { return PlatformGetDashCap(); }

            set
            {
                if (value < DashCap.Flat || value > DashCap.Triangle)
                    throw new InvalidEnumArgumentException("DashCap", (int)value, typeof(DashCap));

                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetDashCap(value);
            }
        }

        public float DashOffset
        {
            get { return PlatformGetDashOffset(); }

            set
            {
                if (isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetDashOffset(value);
            }
        }

        public float[] DashPattern
        {
            get { return PlatformGetDashPattern(); }

            set
            {
                if (isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetDashPattern(value);
            }
        }

        public DashStyle DashStyle
        {
            get { return PlatformGetDashStyle(); }

            set
            {
                if (value < DashStyle.Solid || value > DashStyle.Custom)
                    throw new InvalidEnumArgumentException("DashStyle", (int)value, typeof(DashStyle));

                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetDashStyle(value);
            }
        }

        public LineCap EndCap
        {
            get { return PlatformGetEndCap(); }

            set
            {
                if (value < LineCap.Flat || value > LineCap.Custom)
                    throw new InvalidEnumArgumentException("EndCap", (int)value, typeof(LineCap));

                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetEndCap(value);
            }
        }

        public LineJoin LineJoin
        {
            get { return PlatformGetLineJoin(); }

            set
            {
                if (value < LineJoin.Miter || value > LineJoin.MiterClipped)
                    throw new InvalidEnumArgumentException("LineJoin", (int)value, typeof(LineJoin));

                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetLineJoin(value);
            }
        }

        public float MiterLimit
        {
            get { return PlatformGetMiterLimit(); }

            set
            {
                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetMiterLimit(value);
            }
        }

        public PenType PenType
        {
            get { return PlatformGetPenType(); }
        }

        public LineCap StartCap
        {
            get { return PlatformGetStartCap(); }

            set
            {
                if (value < LineCap.Flat || value > LineCap.Custom)
                    throw new InvalidEnumArgumentException("StartCap", (int)value, typeof(LineCap));

                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetStartCap(value);
            }
        }

        public Matrix Transform
        {
            get { return PlatformGetTransform(); }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("Transform");

                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetTransform(value);
            }
        }

        public float Width
        {
            get { return PlatformGetWidth(); }
            set
            {
                if (!isModifiable)
                    throw new ArgumentException("This Pen object can't be modified.");

                PlatformSetWidth(value);
            }
        }
        #endregion

        public Pen(Brush brush) : this(brush, 1.0F) { }

        public Pen(Color color) : this(color, 1.0F) { }

        public Pen(Brush brush, float width)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            m_brush = brush;
            m_color = Color.Empty;
            m_width = width;

            Initialize();
        }

        public Pen(Color color, float width)
        {
            m_color = color;
            m_brush = null;
            m_width = width;

            Initialize();
        }

        #region Methods
        public void MultiplyTransform(Matrix matrix, MatrixOrder order = MatrixOrder.Prepend)
        {
            PlatformMultipleTransdorm(matrix, order);
        }

        public void ResetTransform()
        {
            PlatformResetTransform();
        }

        public void RotateTransform(float angle, MatrixOrder order = MatrixOrder.Prepend)
        {
            PlatformRotateTransform(angle, order);
        }

        public void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Prepend)
        {
            PlatformScaleTransform(sx, sy, order);
        }

        public void SetLineCap(LineCap startCap, LineCap endCap, DashCap dashCap)
        {
            if (!isModifiable)
                throw new ArgumentException("This Pen object can't be modified.");

            PlatformSetLineCap(startCap, endCap, dashCap);
        }

        public void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Prepend)
        {
            PlatformTranslateTransform(dx, dy, order);
        }

        protected abstract void PlatformMultipleTransdorm(Matrix matrix, MatrixOrder order);

        protected abstract void PlatformResetTransform();

        protected abstract void PlatformRotateTransform(float angle, MatrixOrder order);

        protected abstract void PlatformScaleTransform(float sx, float sy, MatrixOrder order);

        protected abstract void PlatformSetLineCap(LineCap startCap, LineCap endCap, DashCap dashCap);

        protected abstract void PlatformTranslateTransform(float dx, float dy, MatrixOrder order);

        protected abstract PenAlignment PlatformGetPenAlignment();

        protected abstract void PlatformSetPenAlignment(PenAlignment alignment);

        protected abstract Brush PlatformGetBrush();

        protected abstract void PlatformSetBrush(Brush value);

        protected abstract Color PlatformGetColor();

        protected abstract void PlatformSetColor(Color value);

        protected abstract float[] PlatformGetCompoundArray();

        protected abstract void PlatformSetCompoundArray(float[] value);

        protected abstract CustomLineCap PlatformGetCustomEndCap();

        protected abstract void PlatformSetCustomEndCap(CustomLineCap value);

        protected abstract CustomLineCap PlatformGetCustomStartCap();

        protected abstract void PlatformSetCustomStartCap(CustomLineCap value);

        protected abstract DashCap PlatformGetDashCap();

        protected abstract void PlatformSetDashCap(DashCap value);

        protected abstract float PlatformGetDashOffset();

        protected abstract void PlatformSetDashOffset(float value);

        protected abstract float[] PlatformGetDashPattern();

        protected abstract void PlatformSetDashPattern(float[] value);

        protected abstract DashStyle PlatformGetDashStyle();

        protected abstract void PlatformSetDashStyle(DashStyle value);

        protected abstract LineCap PlatformGetEndCap();

        protected abstract void PlatformSetEndCap(LineCap value);

        protected abstract LineJoin PlatformGetLineJoin();

        protected abstract void PlatformSetLineJoin(LineJoin value);

        protected abstract float PlatformGetMiterLimit();

        protected abstract void PlatformSetMiterLimit(float value);

        protected abstract PenType PlatformGetPenType();

        protected abstract LineCap PlatformGetStartCap();

        protected abstract void PlatformSetStartCap(LineCap value);

        protected abstract Matrix PlatformGetTransform();

        protected abstract void PlatformSetTransform(Matrix value);

        protected abstract float PlatformGetWidth();

        protected abstract void PlatformSetWidth(float value);
        #endregion
    }
}