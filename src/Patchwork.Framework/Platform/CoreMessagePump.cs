using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
using Shin.Framework.Messaging;

namespace Patchwork.Framework
{
    public class CoreMessagePump : PlatformMessagePump
    {
        private readonly INativeApplication m_application;

        public CoreMessagePump(ILogger logger, INativeApplication application) : base(logger)
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
