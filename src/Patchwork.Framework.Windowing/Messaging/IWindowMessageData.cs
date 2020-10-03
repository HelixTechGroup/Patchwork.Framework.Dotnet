#region Usings
using System.Drawing;
using Patchwork.Framework.Platform.Windowing;
#endregion

namespace Patchwork.Framework.Messaging
{
    public interface IWindowMessageData : IMessageData
    {
        #region Properties
        PropertyChangedData<bool> FocusChangedData { get; set; }
        PropertyChangingData<bool> FocusChangingData { get; set; }

        WindowMessageIds MessageId { get; }
        PropertyChangedData<Point> PositionChangedData { get; set; }
        PropertyChangingData<Point> PositionChangingData { get; set; }
        PropertyChangedData<Size> SizeChangedData { get; set; }
        PropertyChangingData<Size> SizeChangingData { get; set; }
        INWindow Window { get; }
        #endregion

        #region Methods
        IWindowMessageData SetStateData<T>(MessageIds messageId, T data);

        IWindowMessageData PositionChanging(Point requestedPosition);

        IWindowMessageData PositionChanged(Point requestedPosition);

        IWindowMessageData SizeChanging(Size requestedSize);

        IWindowMessageData SizeChanged(Size requestedSize);

        IWindowMessageData FocusChanging(bool requestedFocus);

        IWindowMessageData FocusChanged(bool requestedFocus);
        #endregion
    }
}