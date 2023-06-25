using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Mathematics;
using Color = System.Drawing.Color;

namespace Patchwork.Framework.Extensions
{
    public static class ColorExtensions
    {
        public static Color4 ToColor4(this Color color)
        {
            return new Color4(color.R, color.G, color.B, color.A);
        }
    }
}
