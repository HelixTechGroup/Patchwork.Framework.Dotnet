using System.Threading;
using Shin.Framework;
using Shin.Framework.Messaging;

namespace Patchwork.Framework.Messaging
{
    public class PlatformMessagePump<TMessage> : MessagePump, IPlatformMessagePump where TMessage : IPlatformMessage
    {
        protected bool m_isRunning;

        public PlatformMessagePump(ILogger logger) : base(logger) { }

        public bool IsRunning
        {
            get => m_isRunning;
            set => m_isRunning = value;
        }

        /// <inheritdoc />
        public override void Pump(CancellationToken ctx)
        {
            AddCancellationToken(ctx);
        }

        /// <inheritdoc />
        public override void Pump()
        {
            Pump(CancellationToken.None);
        }
    }

    public class PlatformMessagePump : MessagePump, IPlatformMessagePump
    {
        protected bool m_isRunning;

        public PlatformMessagePump(ILogger logger) : base(logger) { }

        public bool IsRunning { get => m_isRunning; set => m_isRunning = value; }

        /// <inheritdoc />
        public override void Pump(CancellationToken ctx)
        {
            AddCancellationToken(ctx);
        }

        /// <inheritdoc />
        public override void Pump() 
        {
            Pump(CancellationToken.None);
        }
    }
}
