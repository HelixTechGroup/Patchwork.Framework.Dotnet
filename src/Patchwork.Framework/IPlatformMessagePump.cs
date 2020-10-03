#region Usings
using Patchwork.Framework.Messaging;
using Shin.Framework.Messaging;
#endregion

namespace Patchwork.Framework
{
    //public interface IPlatformMessagePump : IMessagePump<IPlatformMessage> { }

    public interface IPlatformMessagePump : IMessagePump
    {
        new bool Push(IPlatformMessage message);
    }
}