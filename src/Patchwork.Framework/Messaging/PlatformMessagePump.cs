#region Usings
using System;
using System.Collections.Generic;
using System.Threading;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Messaging;
#endregion

namespace Patchwork.Framework.Messaging
{
    public class PlatformMessagePump<TMessage> : MessagePump, IPlatformMessagePump where TMessage : IPlatformMessage
    {
        #region Members
        protected bool m_isRunning;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return m_isRunning; }
            set { m_isRunning = value; }
        }
        #endregion

        public PlatformMessagePump(ILogger logger) : base(logger) { }

        #region Methods
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

        /// <inheritdoc />
        public bool Push(IPlatformMessage message)
        {
            /*Core.Dispatcher.InvokeAsync(() => */Push(message as IPumpMessage)/*).ConfigureAwait(false)*/;
            return true;
        }
        #endregion
    }

    public class PlatformMessagePump : MessagePump, IPlatformMessagePump
    {
        #region Members
        protected bool m_isRunning;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return m_isRunning; }
            set { m_isRunning = value; }
        }
        #endregion

        public PlatformMessagePump(ILogger logger) : base(logger)
        {
            MessagePushed += OnPushed;
            MessagePopped += OnPopped;
        }

        #region Methods
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

        /// <inheritdoc />
        public bool Push(IPlatformMessage message)
        {
            Core.Dispatcher.InvokeAsync(() => Push(message as IPumpMessage)).ConfigureAwait(false);
            return true;
        }

        protected void OnPushed(object sender, IPumpMessage message)
        {
            if (message is null)
                Throw.Exception().InvalidOperationException();
        }

        protected void OnPopped(object sender, IPumpMessage message)
        {
            if (message is null)
                Throw.Exception().InvalidOperationException();

            //var m = message as IPlatformMessage;
            //(m?.RawData as IDispose)?.Dispose();
        }
        #endregion
    }
}