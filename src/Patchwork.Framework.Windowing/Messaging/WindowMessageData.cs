#region Usings
using System;
using System.Drawing;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Messaging
{
    public partial class WindowMessageData : IWindowMessageData, IEquatable<WindowMessageData>
    {
        #region Members
        protected readonly INWindow m_window;
        protected WindowMessageIds m_messageId;
        #endregion

        #region Properties
        public PropertyChangedData<bool> FocusChangedData { get; set; }

        public PropertyChangingData<bool> FocusChangingData { get; set; }

        public WindowMessageIds MessageId
        {
            get { return m_messageId; }
        }

        public PropertyChangedData<Point> PositionChangedData { get; set; }

        public PropertyChangingData<Point> PositionChangingData { get; set; }

        public PropertyChangedData<Size> SizeChangedData { get; set; }

        public PropertyChangingData<Size> SizeChangingData { get; set; }

        public INWindow Window
        {
            get { return m_window; }
        }
        #endregion

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

        #region Methods
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

        /// <inheritdoc />
        public bool Equals(WindowMessageData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (m_window == other.Window) &&
                (m_messageId == other.MessageId) &&
                PositionChangingData.Equals(other.PositionChangingData) &&
                PositionChangedData.Equals(other.PositionChangedData) &&
                SizeChangingData.Equals(other.SizeChangingData) &&
                SizeChangedData.Equals(other.SizeChangedData) &&
                FocusChangingData.Equals(other.FocusChangingData) &&
                FocusChangedData.Equals(other.FocusChangedData);
        }

        /// <inheritdoc />
        public bool Equals(IWindowMessageData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (m_window == other.Window) &&
                   (m_messageId == other.MessageId) && 
                   PositionChangingData.Equals(other.PositionChangingData) &&
                   PositionChangedData.Equals(other.PositionChangedData) &&
                   SizeChangingData.Equals(other.SizeChangingData) &&
                   SizeChangedData.Equals(other.SizeChangedData) &&
                   FocusChangingData.Equals(other.FocusChangingData) &&
                   FocusChangedData.Equals(other.FocusChangedData);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is WindowMessageData other && Equals(other);
        }


        public static bool operator ==(WindowMessageData left, WindowMessageData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WindowMessageData left, WindowMessageData right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}