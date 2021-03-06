﻿#region Usings
using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Shin.Framework;
#endregion

namespace Patchwork.Framework
{
    public static partial class Core
    {
        #region Nested Types
        internal class MainMessagePump : PlatformMessagePump
        {
            #region Members
            private readonly INApplication m_application;
            #endregion

            public MainMessagePump(ILogger logger, INApplication application) : base(logger)
            {
                m_application = application;
            }

            #region Methods
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
            #endregion
        }
        #endregion
    }
}