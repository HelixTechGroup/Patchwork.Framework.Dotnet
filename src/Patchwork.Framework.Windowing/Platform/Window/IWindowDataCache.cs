using System.Drawing;

namespace Patchwork.Framework.Platform.Window
{
    public partial interface IWindowDataCache : IDataCache
    {
        Rectangle ClientArea { get; set; }
        Size ClientSize { get; set; }
        Size PreviousClientSize { get; }
        Size MaxClientSize { get; set; }
        Size Size { get; set; }
        Size PreviousSize { get; }
        Point Position { get; set; }
        Point PreviousPosition { get; }
        string Title { get; set; }
        string PreviousTitle { get; }
        bool IsResizable { get; set; }
        bool IsActive { get; set; }
        bool PreviouslyActive { get; }
        bool IsVisible { get; set; }
        bool PreviouslyVisible { get; }
        bool IsEnabled { get; set; }
        bool PreviouslyEnabled { get; }
        bool IsFocused { get; set; }
        bool PreviouslyFocused { get; }
        NWindowDefinition Definition { get; set; }
    }
}