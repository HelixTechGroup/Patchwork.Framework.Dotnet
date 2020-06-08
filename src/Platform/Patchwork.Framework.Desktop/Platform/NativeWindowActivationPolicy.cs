namespace Patchwork.Framework.Platform
{
    public enum NativeWindowActivationPolicy
    {
        /** Value indicating that a window never activates when it is shown */
        Never,

        /** Value indicating that a window always activates when it is shown */
        Always,

        /** Value indicating that a window only activates when it is first shown */
        FirstShown
    }
}