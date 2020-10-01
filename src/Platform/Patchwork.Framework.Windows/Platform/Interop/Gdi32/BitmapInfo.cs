namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    public struct BitmapInfo
    {
        public BitmapInfoHeader Header;
        public RgbQuad[] Colors;

        public static NativeBitmapInfoHandle NativeAlloc(ref BitmapInfo bitmapInfo)
        {
            return new NativeBitmapInfoHandle(ref bitmapInfo);
        }
    }
}