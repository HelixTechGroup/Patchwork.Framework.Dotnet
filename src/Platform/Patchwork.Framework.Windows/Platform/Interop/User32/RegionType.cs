namespace Patchwork.Framework.Platform.Interop.User32
{
    public enum RegionType
    {
        /// <summary>
        ///     The specified window does not have a region, or an error occurred while attempting to return the region.
        /// </summary>
        ERROR = 0,

        /// <summary>
        ///     The region is empty.
        /// </summary>
        NULLREGION,

        /// <summary>
        ///     The region is a single rectangle.
        /// </summary>
        SIMPLEREGION,

        /// <summary>
        ///     The region is more than one rectangle.
        /// </summary>
        COMPLEXREGION
    }
}