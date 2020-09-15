using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Messaging;
using Shin.Framework.Messaging;

namespace Patchwork.Framework.Messaging 
{
    public interface IPlatformMessage : IMessage<MessageIds>
    {
        DateTime TimeStamp { get; }

        IMessageData RawData { get; }
    }

    public interface IPlatformMessage<TMessageData> : IPlatformMessage where TMessageData : IMessageData
    {
        new TMessageData Data { get; }
    }
}
