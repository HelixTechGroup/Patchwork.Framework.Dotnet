//
// System.Drawing.SolidBrush.cs
//
// Author:
//   Dennis Hayes (dennish@Raytek.com)
//   Alexandre Pigolkine(pigolkine@gmx.de)
//   Ravindra (rkumar@novell.com)
//
// (C) 2002 Ximian, Inc.
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

using Patchwork.Framework.Platform.Interop.GdiPlus;

namespace System.Drawing
{
    public abstract class SolidBrush : SolidBrush<IntPtr>
    {
        /// <inheritdoc />
        protected SolidBrush(Color color) : base(color) { }

        /// <inheritdoc />
        protected SolidBrush(IntPtr ptr) : base(ptr) { }
    }

    public abstract class SolidBrush<TNative> : Brush<TNative>
    {
        #region Members
        internal bool isModifiable = true;

        // we keep this cached because calling GdipGetSolidFillColor 
        // wouldn't return a "named" color like SD is expected to do
        protected Color m_color;
        #endregion

        #region Properties
        public Color Color
        {
            get
            {
                return PlatformGetColor();
            }
            set
            {
                if (!isModifiable)
                    throw new ArgumentException("This SolidBrush object can't be modified.");
                
                PlatformSetColor(value);
            }
        }

        protected abstract Color PlatformGetColor();

        protected abstract void PlatformSetColor(Color value);
        #endregion

        public SolidBrush(Color color)
        {
            m_color = color;
        }

        internal SolidBrush(TNative ptr)
            : base(ptr)
        {
        }
    }
}