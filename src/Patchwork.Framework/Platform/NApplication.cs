#region Usings
using System.Collections.Concurrent;
using System.Threading;
using Patchwork.Framework.Messaging;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Messaging;
#endregion

namespace Patchwork.Framework.Platform
{
    public abstract partial class NApplication : Initializable, INApplication
    {
        #region Members
        protected readonly Thread m_thread;
        protected INHandle m_handle;
        protected readonly ConcurrentList<int> m_tokens;
        protected ILogger m_logger;

        protected ConcurrentQueue<IMessage> m_queue;
        protected CancellationToken m_token;
        protected CancellationTokenSource m_tokenSource;
        #endregion

        #region Properties
        public INHandle Handle
        {
            get { return m_handle; }
        }

        public Thread Thread
        {
            get { return m_thread; }
        }
        #endregion

        protected NApplication()
        {
            m_thread = Thread.CurrentThread;
            m_token = new CancellationToken();
            m_tokens = new ConcurrentList<int>();
            WireUpApplicationEvents();
            ConstructionShared();
        }

        #region Methods
        protected void AddCancellationToken(CancellationToken ctx)
        {
            if (ctx == CancellationToken.None || m_token.IsCancellationRequested)
                return;

            if (m_tokens.Contains(ctx.GetHashCode()))
                return;

            m_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(m_token, ctx);
            m_tokens.Add(ctx.GetHashCode());
        }

        public virtual void PumpMessages(CancellationToken cancellationToken)
        {
            AddCancellationToken(cancellationToken);
            if (m_token.IsCancellationRequested)
                return;

            PlatformPumpMessages();
            PumpMessagesShared();
        }

        protected virtual void OnProcessMessage(IPlatformMessage message)
        {
            if (!m_isInitialized)
                return;

            switch (message.Id)
            {
                case MessageIds.Quit:
                    break;
                case MessageIds.Window:
                    break;
            }
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            Core.ProcessMessage -= OnProcessMessage;
            if (!m_tokenSource.IsCancellationRequested)
                m_tokenSource.Cancel();

            base.DisposeManagedResources();
            m_tokenSource.Dispose();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            Core.ProcessMessage += OnProcessMessage;
            m_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(m_token);
        }

        partial void ConstructionShared();

        partial void InitializeResourcesShared();

        partial void DisposeManagedResourcesShared();

        partial void PumpMessagesShared();

        protected abstract void PlatformPumpMessages();

        private void WireUpApplicationEvents() { }
        #endregion
    }
}