namespace Patchwork.Framework.Platform.Windowing
{
    public enum NWindowTransparency
    {
        /** Value indicating that a window does not support transparency */
        None,

        /** Value indicating that a window supports transparency at the window level (one opacity applies to the entire window) */
        PerWindow,

        /** Value indicating that a window supports per-pixel alpha blended transparency */
        PerPixel,
    }
}