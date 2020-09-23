using System.Drawing;
using Patchwork.Framework.Platform.Window;

namespace Patchwork.Framework.Messaging
{
    public interface IWindowMessageData : IMessageData
    {
        IWindowMessageData SetStateData<T>(MessageIds messageId, T data);

        IWindowMessageData PositionChanging(Point requestedPosition);

        IWindowMessageData PositionChanged(Point requestedPosition);

        IWindowMessageData SizeChanging(Size requestedSize);

        IWindowMessageData SizeChanged(Size requestedSize);

        IWindowMessageData FocusChanging(bool requestedFocus);

        IWindowMessageData FocusChanged(bool requestedFocus);

        WindowMessageIds MessageId { get; }
        INWindow Window { get; }
        PropertyChangingData<Point> PositionChangingData { get; set; }
        PropertyChangedData<Point> PositionChangedData { get; set; }
        PropertyChangingData<Size> SizeChangingData { get; set; }
        PropertyChangedData<Size> SizeChangedData { get; set; }
        PropertyChangingData<bool> FocusChangingData { get; set; }
        PropertyChangedData<bool> FocusChangedData { get; set; }
    }
}