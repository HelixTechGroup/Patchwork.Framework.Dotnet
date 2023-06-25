using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Patchwork.Framework.Extensions
{
    public static class FloatExtensions
    {
        public static int ToInt32(this float value)
        {
            return Convert.ToInt32(value);
        }
    }
}
