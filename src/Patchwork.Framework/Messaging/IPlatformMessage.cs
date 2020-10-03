#region Usings
using System;
using Shin.Framework.Messaging;
#endregion

namespace Patchwork.Framework.Messaging
{
    public interface IPlatformMessage : IMessage<MessageIds>
    {
        #region Properties
        IMessageData RawData { get; }
        DateTime TimeStamp { get; }
        #endregion
    }

    public interface IPlatformMessage<TMessageData> : IPlatformMessage where TMessageData : IMessageData
    {
        #region Properties
         TMessageData Data { get; }
        #endregion
    }
}