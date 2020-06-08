using System;
using System.Collections.Generic;
using System.Text;
using Shin.Framework.Messaging;

namespace Patchwork.Framework.Messaging
{
    public interface IPlatformMessage : IMessage<MessageIds>
    {
        DateTime TimeStamp { get; }
    }
}
