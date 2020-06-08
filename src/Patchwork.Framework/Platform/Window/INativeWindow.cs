using System;
using System.Drawing;
using Shin.Framework;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Platform.Window
{
    public interface INativeWindow : INativeObject, IInitialize, IDispose
    {
        event EventHandler Closing;
        event EventHandler Closed;
        event EventHandler Created;
        event EventHandler Destroyed;
        event EventHandler Activating;
        event EventHandler Activated;
        event EventHandler Deactivating;
        event EventHandler Deactivated;
        event EventHandler<PropertyChangedEventArgs<string>> TitleChanged;

        #region Properties
        NativeWindowType Type { get; }
        string Title { get; set; }
        bool IsMainApplicationWindow { get; }
        bool IsActive { get; }
        bool IsVisible { get; }
        Size ClientSize { get; }
        Rectangle ClientArea { get; }
        Size MaxClientSize { get; }
        INativeObject Parent { get; }
        INativeWindowRenderer Renderer { get; }
        INativeInput Input { get; }
        #endregion

        void Show();
        void Hide();
        void Create();
        void Destroy();
        void Activate();
        void Deactivate();
        bool IsPointInWindow(Point point);
        Point PointToClient(Point point);
        Point PointToScreen(Point point);
    }
}