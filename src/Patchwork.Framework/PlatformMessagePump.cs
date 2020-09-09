using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
using Shin.Framework.Messaging;

namespace Patchwork.Framework
{
    public class PlatformMessagePump : MessagePump<IPlatformMessage>
    {
        private readonly INativeApplication m_application;
        private bool m_isRunning;

        public PlatformMessagePump(ILogger logger, INativeApplication application) : base(logger)
        {
            m_application = application;
        }

        public bool IsRunning { get => m_isRunning; set => m_isRunning = value; }

        /// <inheritdoc />
        public override void Pump(CancellationToken ctx)
        {
            AddCancellationToken(ctx);

            if (m_tokenSource.IsCancellationRequested)
                return;

            m_application.PumpMessages(m_tokenSource.Token);
        }

        /// <inheritdoc />
        public override void Pump() 
        {
            Pump(CancellationToken.None);
        }
    }
}
