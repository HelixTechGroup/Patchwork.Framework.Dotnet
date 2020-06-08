using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct SizeS : IEquatable<SizeS>
    {

        public SizeS(short width, short height)
        {
            this.Width = width;
            this.Height = height;
        }

        public bool Equals(SizeS other)
        {
            return (this.Width == other.Width) && (this.Height == other.Height);
        }

        public override bool Equals(object obj)
        {
            return obj is SizeS && this.Equals((SizeS)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)this.Width * 397) ^ (int)this.Height;
            }
        }

        public short Width;
        public short Height;

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

        public bool IsEmpty => this.Width == 0 && this.Height == 0;
    }
}
