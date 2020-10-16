#region Usings
using System.Drawing;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Rendering;
#endregion

namespace Patchwork.Framework.Messaging
{
    public interface IRenderMessageData : IMessageData
    {
        #region Properties
        RenderMessageIds MessageId { get; }
        INRenderer Render { get; }
        NFrameBuffer FrameBuffer { get; }
        #endregion

        #region Methods
        IRenderMessageData SetStateData<T>(MessageIds messageId, T data);

        IRenderMessageData SetFrameBuffer(NFrameBuffer frameBuffer);
        #endregion
    }
}