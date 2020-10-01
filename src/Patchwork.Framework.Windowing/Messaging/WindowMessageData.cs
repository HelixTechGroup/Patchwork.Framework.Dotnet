using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Messaging
{
    public partial class WindowMessageData : IWindowMessageData
    {
        protected WindowMessageIds m_messageId;
        protected readonly INWindow m_window;

        public WindowMessageData(INWindow window)
        {
            Throw.If(window == null).InvalidOperationException();
            m_window = window;
        }

        public WindowMessageData(WindowMessageIds messageId, INWindow window) : this(window)
        {
            m_messageId = messageId;
            PositionChangedData = new PropertyChangedData<Point>(Point.Empty, Point.Empty, Point.Empty);
            PositionChangingData = new PropertyChangingData<Point>(Point.Empty, Point.Empty);
            SizeChangedData = new PropertyChangedData<Size>(Size.Empty, Size.Empty, Size.Empty);
            SizeChangingData = new PropertyChangingData<Size>(Size.Empty, Size.Empty);
            FocusChangedData = new PropertyChangedData<bool>(true, true, true);
            FocusChangingData = new PropertyChangingData<bool>(true, true);

        }

        /// <inheritdoc />
        public IWindowMessageData SetStateData<T>(MessageIds messageId, T data)
        {

            return this;
        }

        public IWindowMessageData PositionChanging(Point requestedPosition)
        {
            m_messageId = WindowMessageIds.Moving;
            PositionChangingData = new PropertyChangingData<Point>(m_window.Position, requestedPosition);
            return this;
        }

        public IWindowMessageData PositionChanged(Point requestedPosition)
        {
            m_messageId = WindowMessageIds.Moved;
            var data = m_window.GetDataCache();
            PositionChangedData = new PropertyChangedData<Point>(m_window.Position, requestedPosition, data.PreviousPosition);
            return this;
        }

        public IWindowMessageData SizeChanging(Size requestedSize)
        {
            m_messageId = WindowMessageIds.Resizing;
            SizeChangingData = new PropertyChangingData<Size>(m_window.Size, requestedSize);
            return this;
        }

        public IWindowMessageData SizeChanged(Size requestedSize)
        {
            m_messageId = WindowMessageIds.Resized;
            var data = m_window.GetDataCache();
            SizeChangedData = new PropertyChangedData<Size>(m_window.Size, requestedSize, data.PreviousSize);
            return this;
        }

        public IWindowMessageData FocusChanging(bool requestedFocus)
        {
            m_messageId = requestedFocus ? WindowMessageIds.Focusing : WindowMessageIds.Unfocusing;
            FocusChangingData = new PropertyChangingData<bool>(m_window.IsFocused, requestedFocus);
            return this;
        }

        public IWindowMessageData FocusChanged(bool requestedFocus)
        {
            m_messageId = requestedFocus ? WindowMessageIds.Focused : WindowMessageIds.Unfocused;
            var data = m_window.GetDataCache();
            FocusChangedData = new PropertyChangedData<bool>(m_window.IsFocused, requestedFocus, data.PreviouslyFocused);
            return this;
        }

        public WindowMessageIds MessageId
        {
            get { return m_messageId; }
        }

        public INWindow Window
        {
            get { return m_window; }
        }

        public PropertyChangingData<Point> PositionChangingData { get; set; }

        public PropertyChangedData<Point> PositionChangedData { get; set; }

        public PropertyChangingData<Size> SizeChangingData { get; set; }

        public PropertyChangedData<Size> SizeChangedData { get; set; }

        public PropertyChangingData<bool> FocusChangingData { get; set; }

        public PropertyChangedData<bool> FocusChangedData { get; set; }
    }
}
