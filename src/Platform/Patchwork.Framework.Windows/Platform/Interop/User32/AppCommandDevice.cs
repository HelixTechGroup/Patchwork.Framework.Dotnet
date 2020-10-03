namespace Patchwork.Framework.Platform.Interop.User32
{
    public enum AppCommandDevice
    {
        /// <summary>
        ///     User pressed a key.
        /// </summary>
        FAPPCOMMAND_KEY = 0,

        /// <summary>
        ///     User clicked a mouse button.
        /// </summary>
        FAPPCOMMAND_MOUSE = 0x8000,

        /// <summary>
        ///     An unidentified hardware source generated the event. It could be a mouse or a keyboard event.
        /// </summary>
        FAPPCOMMAND_OEM = 0x1000,
        FAPPCOMMAND_MASK = 0xF000
    }
}