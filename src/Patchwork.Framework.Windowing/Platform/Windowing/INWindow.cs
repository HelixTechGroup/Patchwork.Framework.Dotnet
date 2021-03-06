﻿#region Usings
using System;
using System.Drawing;
using Shin.Framework;
using Shin.Framework.ComponentModel;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public partial interface INWindow : INObject, IInitialize, IDispose, IEquatable<INWindow>
    {
        #region Events
        event EventHandler Activated;
        event EventHandler Activating;
        event EventHandler Closed;
        event EventHandler Closing;
        event EventHandler Created;
        event EventHandler Deactivated;
        event EventHandler Deactivating;
        event EventHandler Destroyed;
        event EventHandler Disabled;
        event EventHandler Enabled;
        event EventHandler<PropertyChangedEventArgs<bool>> FocusChanged;
        event EventHandler<PropertyChangingEventArgs<bool>> FocusChanging;
        event EventHandler FocusGained;
        event EventHandler FocusLost;
        event EventHandler<PropertyChangedEventArgs<Point>> PositionChanged;
        event EventHandler<PropertyChangingEventArgs<Point>> PositionChanging;
        event EventHandler<PropertyChangedEventArgs<Size>> SizeChanged;
        event EventHandler<PropertyChangingEventArgs<Size>> SizeChanging;
        event EventHandler<PropertyChangedEventArgs<string>> TitleChanged;
        #endregion

        #region Properties
        Rectangle ClientArea { get; }
        Size ClientSize { get; }
        bool IsActive { get; }
        bool IsEnabled { get; }
        bool IsFocused { get; }
        bool IsMainApplicationWindow { get; }
        bool IsResizable { get; set; }
        bool IsVisible { get; }
        Size MaxClientSize { get; }
        INObject Parent { get; }
        Point Position { get; }
        Size Size { get; }
        string Title { get; set; }
        #endregion

        #region Methods
        void Initialize(NWindowDefinition definition);

        void Create(bool initialize = true);

        void Destroy();

        Point PointToClient(Point point);

        Point PointToScreen(Point point);

        void SyncDataCache(bool force = false);

        void InvalidateDataCache();

        IWindowDataCache GetDataCache();
        #endregion
    }
}