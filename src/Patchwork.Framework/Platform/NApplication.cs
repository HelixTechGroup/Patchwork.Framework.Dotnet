#region Usings
using System.Threading;
using Patchwork.Framework.Messaging;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public abstract partial class NApplication : Initializable, INApplication
    {
        #region Members
        protected readonly Thread m_thread;
        protected INHandle m_handle;
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
            WireUpApplicationEvents();
            ConstructionShared();
        }

        #region Methods
        public virtual void PumpMessages(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            PlatformPumpMessages(cancellationToken);
            PumpMessagesShared(cancellationToken);
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
            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            Core.ProcessMessage += OnProcessMessage;
        }

        partial void ConstructionShared();

        partial void InitializeResourcesShared();

        partial void DisposeManagedResourcesShared();

        partial void PumpMessagesShared(CancellationToken cancellationToken);

        protected abstract void PlatformPumpMessages(CancellationToken cancellationToken);

        private void WireUpApplicationEvents() { }
        #endregion
    }
}