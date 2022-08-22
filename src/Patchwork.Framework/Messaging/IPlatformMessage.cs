#region Usings
using System;
using System.Collections.Generic;
using Shin.Framework.Messaging;
#endregion

namespace Patchwork.Framework.Messaging
{
    public interface IPlatformMessage : IPumpMessage<MessageIds>
    {
        #region Properties
        IMessageData RawData { get; }

        DateTime TimeStamp { get; }

        //IEnumerable<string> ProcessedBy { get;  }  
        #endregion
    }

    public interface IPlatformMessage<TMessageData> : IPlatformMessage where TMessageData : IMessageData
    {
        #region Properties
         TMessageData Data { get; }
        #endregion
    }
}