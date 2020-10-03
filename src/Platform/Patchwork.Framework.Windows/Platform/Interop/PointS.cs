#region Usings
using System;
using System.Globalization;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PointS : IEquatable<PointS>
    {
        #region Members
        public short X, Y;
        #endregion

        #region Properties
        public bool IsEmpty
        {
            get { return X == 0 && Y == 0; }
        }
        #endregion

        public PointS(short x, short y)
        {
            X = x;
            Y = y;
        }

        #region Methods
        public bool Equals(PointS other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is PointS && Equals((PointS)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

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
        #endregion
    }
}