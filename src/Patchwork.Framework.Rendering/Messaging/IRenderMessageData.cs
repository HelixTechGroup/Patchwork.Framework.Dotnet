using System.Drawing;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Window;

namespace Patchwork.Framework.Messaging
{
    public interface IRenderMessageData : IMessageData
    {
        IRenderMessageData SetStateData<T>(MessageIds messageId, T data);

        IRenderMessageData PositionChanging(Point requestedPosition);

        IRenderMessageData PositionChanged(Point requestedPosition);

        IRenderMessageData SizeChanging(Size requestedSize);

        IRenderMessageData SizeChanged(Size requestedSize);

        IRenderMessageData FocusChanging(bool requestedFocus);

        IRenderMessageData FocusChanged(bool requestedFocus);

        RenderMessageIds MessageId { get; }
        INativeWindowRenderer Render { get; }
        PropertyChangingData<Point> PositionChangingData { get; set; }
        PropertyChangedData<Point> PositionChangedData { get; set; }
        PropertyChangingData<Size> SizeChangingData { get; set; }
        PropertyChangedData<Size> SizeChangedData { get; set; }
        PropertyChangingData<bool> FocusChangingData { get; set; }
        PropertyChangedData<bool> FocusChangedData { get; set; }
    }
}