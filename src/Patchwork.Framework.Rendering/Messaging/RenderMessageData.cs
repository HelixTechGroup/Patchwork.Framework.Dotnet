using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Messaging
{
    public partial class RenderMessageData : IRenderMessageData
    {
        protected RenderMessageIds m_messageId;
        protected readonly INativeWindowRenderer m_render;

        public RenderMessageData(INativeWindowRenderer renderer)
        {
            Throw.If(renderer == null).InvalidOperationException();
            m_render = renderer;
        }

        public RenderMessageData(RenderMessageIds messageId, INativeWindowRenderer renderer) : this(renderer)
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
        public IRenderMessageData SetStateData<T>(MessageIds messageId, T data)
        {

            return this;
        }

        public IRenderMessageData PositionChanging(Point requestedPosition)
        {
            m_messageId = RenderMessageIds.Moving;
            //PositionChangingData = new PropertyChangingData<Point>(m_render.Position, requestedPosition);
            return this;
        }

        public IRenderMessageData PositionChanged(Point requestedPosition)
        {
            m_messageId = RenderMessageIds.Moved;
            //var data = m_render.GetDataCache();
            //PositionChangedData = new PropertyChangedData<Point>(m_render.Position, requestedPosition, data.PreviousPosition);
            return this;
        }

        public IRenderMessageData SizeChanging(Size requestedSize)
        {
            m_messageId = RenderMessageIds.Resizing;
            //SizeChangingData = new PropertyChangingData<Size>(m_render.Size, requestedSize);
            return this;
        }

        public IRenderMessageData SizeChanged(Size requestedSize)
        {
            m_messageId = RenderMessageIds.Resized;
            //var data = m_render.GetDataCache();
            //SizeChangedData = new PropertyChangedData<Size>(m_render.Size, requestedSize, data.PreviousSize);
            return this;
        }

        public IRenderMessageData FocusChanging(bool requestedFocus)
        {
            m_messageId = requestedFocus ? RenderMessageIds.Focusing : RenderMessageIds.Unfocusing;
            //FocusChangingData = new PropertyChangingData<bool>(m_render.IsFocused, requestedFocus);
            return this;
        }

        public IRenderMessageData FocusChanged(bool requestedFocus)
        {
            m_messageId = requestedFocus ? RenderMessageIds.Focused : RenderMessageIds.Unfocused;
            //var data = m_render.GetDataCache();
            //FocusChangedData = new PropertyChangedData<bool>(m_render.IsFocused, requestedFocus, data.PreviouslyFocused);
            return this;
        }

        public RenderMessageIds MessageId
        {
            get { return m_messageId; }
        }

        public INativeWindowRenderer Render
        {
            get { return m_render; }
        }

        public PropertyChangingData<Point> PositionChangingData { get; set; }

        public PropertyChangedData<Point> PositionChangedData { get; set; }

        public PropertyChangingData<Size> SizeChangingData { get; set; }

        public PropertyChangedData<Size> SizeChangedData { get; set; }

        public PropertyChangingData<bool> FocusChangingData { get; set; }

        public PropertyChangedData<bool> FocusChangedData { get; set; }
    }
}
