using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;
using SkiaSharp;

namespace Patchwork.Framework.Extensions
{
    public static class SKColorExtenstions
    {
        public static Color ToSystemColor(this SKColor skColor)
        {
            return Color.FromArgb(skColor.Alpha, skColor.Red, skColor.Green, skColor.Blue);
        }
    }
}
