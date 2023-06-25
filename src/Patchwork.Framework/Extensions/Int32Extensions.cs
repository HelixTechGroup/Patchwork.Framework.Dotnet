using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Extensions
{
    public static class Int32Extensions
    {
        public static float ToFloat(this int value)
        {
            return (float)Convert.ToDecimal(value);
        }
    }
}
