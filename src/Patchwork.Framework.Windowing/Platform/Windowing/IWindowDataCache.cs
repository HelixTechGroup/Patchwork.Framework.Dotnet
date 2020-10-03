#region Usings
using System.Drawing;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public partial interface IWindowDataCache : IDataCache
    {
        #region Properties
        Rectangle ClientArea { get; set; }
        Size ClientSize { get; set; }
        NWindowDefinition Definition { get; set; }
        bool IsActive { get; set; }
        bool IsEnabled { get; set; }
        bool IsFocused { get; set; }
        bool IsResizable { get; set; }
        bool IsVisible { get; set; }
        Size MaxClientSize { get; set; }
        Point Position { get; set; }
        Size PreviousClientSize { get; }
        bool PreviouslyActive { get; }
        bool PreviouslyEnabled { get; }
        bool PreviouslyFocused { get; }
        bool PreviouslyVisible { get; }
        Point PreviousPosition { get; }
        Size PreviousSize { get; }
        string PreviousTitle { get; }
        Size Size { get; set; }
        string Title { get; set; }
        #endregion
    }
}