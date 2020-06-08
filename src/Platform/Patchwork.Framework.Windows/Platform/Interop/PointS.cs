using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct PointS : IEquatable<PointS>
    {
        public PointS(short x, short y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(PointS other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is PointS && Equals((PointS)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)X * 397) ^ (int)Y;
            }
        }

        public short X, Y;

        public static bool operator ==(PointS left, PointS right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PointS left, PointS right)
        {
            return !(left == right);
        }

        public void Offset(short x, short y)
        {
            X += x;
            Y += y;
        }

        public void Set(short x, short y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            var culture = CultureInfo.CurrentCulture;
            return $"{{ X = {X.ToString(culture)}, Y = {Y.ToString(culture)} }}";
        }

        public bool IsEmpty => this.X == 0 && this.Y == 0;
    }
}
