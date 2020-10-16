namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    public struct BitmapInfo
    {
        #region Members
        public RgbQuad[] Colors;
        public BitmapInfoHeader Header;
        #endregion

        #region Methods
        public static NativeBitmapInfoHandle NativeAlloc(ref BitmapInfo bitmapInfo)
        {
            return new NativeBitmapInfoHandle(ref bitmapInfo);
        }
        #endregion
    }
}