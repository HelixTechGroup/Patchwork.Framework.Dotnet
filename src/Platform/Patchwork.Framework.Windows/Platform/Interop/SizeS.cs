#region Usings
using System;
using System.Globalization;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SizeS : IEquatable<SizeS>
    {
        #region Members
        public short Height;

        public short Width;
        #endregion

        #region Properties
        public bool IsEmpty
        {
            get { return Width == 0 && Height == 0; }
        }
        #endregion

        public SizeS(short width, short height)
        {
            Width = width;
            Height = height;
        }

        #region Methods
        public bool Equals(SizeS other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            return obj is SizeS && Equals((SizeS)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }

        public static bool operator ==(SizeS left, SizeS right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SizeS left, SizeS right)
        {
            return !(left == right);
        }

        public void Offset(short width, short height)
        {
            Width += width;
            Height += height;
        }

        public void Set(short width, short height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            var culture = CultureInfo.CurrentCulture;
            return $"{{ Width = {Width.ToString(culture)}, Height = {Height.ToString(culture)} }}";
        }
        #endregion
    }
}