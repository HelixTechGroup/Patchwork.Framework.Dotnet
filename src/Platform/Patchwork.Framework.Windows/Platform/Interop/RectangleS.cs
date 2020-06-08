using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct RectangleS : IEquatable<RectangleS>
    {
        public RectangleS(short left = 0, short top = 0, short right = 0, short bottom = 0)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RectangleS(short width = 0, short height = 0) : this(0, 0, width, height) { }
        public RectangleS(short all = 0) : this(all, all, all, all) { }

        public short Left, Top, Right, Bottom;

        public bool Equals(RectangleS other)
        {
            return (Left == other.Left) && (Right == other.Right) && (Top == other.Top) && (Bottom == other.Bottom);
        }

        public override bool Equals(object obj)
        {
            return obj is RectangleS && Equals((RectangleS)obj);
        }

        public static bool operator ==(RectangleS left, RectangleS right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RectangleS left, RectangleS right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Left;
                hashCode = (hashCode * 397) ^ (int)Top;
                hashCode = (hashCode * 397) ^ (int)Right;
                hashCode = (hashCode * 397) ^ (int)Bottom;
                return hashCode;
            }
        }

        public SizeS Size
        {
            get { return new SizeS(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public bool IsEmpty => this.Left == 0 && this.Top == 0 && this.Right == 0 && this.Bottom == 0;

        public short Width { get { return unchecked((short)(Right - Left)); } set { Right = unchecked((short)(Left + value)); } }
        public short Height { get { return unchecked((short)(Bottom - Top)); } set { Bottom = unchecked((short)(Top + value)); } }

        public static RectangleS Create(short x, short y, short width, short height)
        {
            unchecked
            {
                return new RectangleS(x, y, (short)(width + x), (short)(height + y));
            }
        }

        public override string ToString()
        {
            var culture = CultureInfo.CurrentCulture;
            return $"{{ Left = {Left.ToString(culture)}, Top = {Top.ToString(culture)} , Right = {Right.ToString(culture)}, Bottom = {Bottom.ToString(culture)} }}, {{ Width: {Width.ToString(culture)}, Height: {Height.ToString(culture)} }}";
        }

        public static RectangleS From(ref RectangleS lvalue, ref RectangleS rvalue,
            Func<short, short, short> leftTopOperation,
            Func<short, short, short> rightBottomOperation = null)
        {
            if (rightBottomOperation == null) rightBottomOperation = leftTopOperation;
            return new RectangleS(
                leftTopOperation(lvalue.Left, rvalue.Left),
                leftTopOperation(lvalue.Top, rvalue.Top),
                rightBottomOperation(lvalue.Right, rvalue.Right),
                rightBottomOperation(lvalue.Bottom, rvalue.Bottom)
            );
        }

        public void Add(RectangleS value)
        {
            Add(ref this, ref value);
        }

        public void Subtract(RectangleS value)
        {
            Subtract(ref this, ref value);
        }

        public void Multiply(RectangleS value)
        {
            Multiply(ref this, ref value);
        }

        public void Divide(RectangleS value)
        {
            Divide(ref this, ref value);
        }

        public void Deflate(RectangleS value)
        {
            Deflate(ref this, ref value);
        }

        public void Inflate(RectangleS value)
        {
            Inflate(ref this, ref value);
        }

        public void Offset(short x, short y)
        {
            Offset(ref this, x, y);
        }

        public void OffsetTo(short x, short y)
        {
            OffsetTo(ref this, x, y);
        }

        public void Scale(short x, short y)
        {
            Scale(ref this, x, y);
        }

        public void ScaleTo(short x, short y)
        {
            ScaleTo(ref this, x, y);
        }

        public static void Add(ref RectangleS lvalue, ref RectangleS rvalue)
        {
            lvalue.Left += rvalue.Left;
            lvalue.Top += rvalue.Top;
            lvalue.Right += rvalue.Right;
            lvalue.Bottom += rvalue.Bottom;
        }

        public static void Subtract(ref RectangleS lvalue, ref RectangleS rvalue)
        {
            lvalue.Left -= rvalue.Left;
            lvalue.Top -= rvalue.Top;
            lvalue.Right -= rvalue.Right;
            lvalue.Bottom -= rvalue.Bottom;
        }

        public static void Multiply(ref RectangleS lvalue, ref RectangleS rvalue)
        {
            lvalue.Left *= rvalue.Left;
            lvalue.Top *= rvalue.Top;
            lvalue.Right *= rvalue.Right;
            lvalue.Bottom *= rvalue.Bottom;
        }

        public static void Divide(ref RectangleS lvalue, ref RectangleS rvalue)
        {
            lvalue.Left /= rvalue.Left;
            lvalue.Top /= rvalue.Top;
            lvalue.Right /= rvalue.Right;
            lvalue.Bottom /= rvalue.Bottom;
        }

        public static void Deflate(ref RectangleS target, ref RectangleS deflation)
        {
            target.Top += deflation.Top;
            target.Left += deflation.Left;
            target.Bottom -= deflation.Bottom;
            target.Right -= deflation.Right;
        }

        public static void Inflate(ref RectangleS target, ref RectangleS inflation)
        {
            target.Top -= inflation.Top;
            target.Left -= inflation.Left;
            target.Bottom += inflation.Bottom;
            target.Right += inflation.Right;
        }

        public static void Offset(ref RectangleS target, short x, short y)
        {
            target.Top += y;
            target.Left += x;
            target.Bottom += y;
            target.Right += x;
        }

        public static void OffsetTo(ref RectangleS target, short x, short y)
        {
            var width = target.Width;
            var height = target.Height;
            target.Left = x;
            target.Top = y;
            target.Right = width;
            target.Bottom = height;
        }

        public static void Scale(ref RectangleS target, short x, short y)
        {
            target.Top *= y;
            target.Left *= x;
            target.Bottom *= y;
            target.Right *= x;
        }

        public static void ScaleTo(ref RectangleS target, short x, short y)
        {
            unchecked
            {
                x = (short)(target.Left / x);
                y = (short)(target.Top / y);
            }
            Scale(ref target, x, y);
        }

    }
}
