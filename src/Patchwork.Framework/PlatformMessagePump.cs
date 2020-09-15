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
