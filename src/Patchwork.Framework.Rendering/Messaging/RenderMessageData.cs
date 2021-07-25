#region Usings
using System.Drawing;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Rendering;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Messaging
{
    public class RenderMessageData : Disposable, IRenderMessageData
    {
        #region Members
        protected readonly INRenderer m_render;
        protected RenderMessageIds m_messageId;
        protected NFrameBuffer m_frameBuffer;
        #endregion

        #region Properties
        public PropertyChangedData<bool> FocusChangedData { get; set; }

        public PropertyChangingData<bool> FocusChangingData { get; set; }

        public RenderMessageIds MessageId
        {
            get { return m_messageId; }
        }

        public PropertyChangedData<Point> PositionChangedData { get; set; }

        public PropertyChangingData<Point> PositionChangingData { get; set; }

        public INRenderer Render
        {
            get { return m_render; }
        }

        /// <inheritdoc />
        public NFrameBuffer FrameBuffer
        {
            get { return m_frameBuffer.Copy(); }
        }

        public PropertyChangedData<Size> SizeChangedData { get; set; }

        public PropertyChangingData<Size> SizeChangingData { get; set; }
        #endregion

        public RenderMessageData(INRenderer renderer)
        {
            Throw.If(renderer == null).InvalidOperationException();
            m_render = renderer;
        }

        public RenderMessageData(RenderMessageIds messageId, INRenderer renderer) : this(renderer)
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
        public IRenderMessageData SetStateData<T>(MessageIds messageId, T data)
        {
            return this;
        }

        public IRenderMessageData SetFrameBuffer(NFrameBuffer frameBuffer)
        {
            m_messageId = RenderMessageIds.SetFrameBuffer;
            m_frameBuffer = frameBuffer.Copy();
            //PositionChangingData = new PropertyChangingData<Point>(m_render.Position, requestedPosition);
            return this;
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_frameBuffer?.Dispose();

            base.DisposeManagedResources();
        }
        #endregion
    }
}