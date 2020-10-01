namespace Patchwork.Framework.Platform.Windowing
{
    public enum NWindowType
    {
        /** Value indicating that this is a standard, general-purpose window */
        Normal,

        /** Value indicating that this is a window used for a popup menu */
        Menu,

        /** Value indicating that this is a window used for a tooltip */
        ToolTip,

        /** Value indicating that this is a window used for a notification toast */
        Notification,

        /** Value indicating that this is a window used for a cursor decorator */
        CursorDecorator,

        /** Value indicating that this is a game window */
        GameWindow
    }
}