using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;
using Vortice.Mathematics;
using Color = System.Drawing.Color;

namespace Patchwork.Framework.Extensions
{
    public static class Color4Extensions
    {
        public static Color4 FromSystemColor(this Color4 dxColor, Color color)
        {
            return new Color4(color.R, color.G, color.B);
        }

        public static Color ToSystemColor(this Color4 dxColor)
        {

            dxColor.ToBgra(out var r, out var g, out var b, out var a);
            return Color.FromArgb(a, r, g, b);
        }
    }
}
