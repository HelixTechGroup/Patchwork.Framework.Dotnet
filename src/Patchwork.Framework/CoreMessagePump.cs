using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Shin.Framework;

namespace Patchwork.Framework
{
    public class CoreMessagePump : PlatformMessagePump
    {
        private readonly INApplication m_application;

        public CoreMessagePump(ILogger logger, INApplication application) : base(logger)
        {
            m_application = application;
        }

        /// <inheritdoc />
        public override void Pump(CancellationToken ctx)
        {
            if (m_tokenSource.IsCancellationRequested)
                return;

            base.Pump(ctx);
            m_application.PumpMessages(m_tokenSource.Token);
        }

        /// <inheritdoc />
        public override void Pump() 
        {
            Pump(CancellationToken.None);
        }
    }
}
