using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform
{
    public abstract partial class NativeApplication : Initializable, INativeApplication
    {
        protected INativeHandle m_handle;
        protected readonly Thread m_thread;

        public INativeHandle Handle
        {
            get { return m_handle; }
        }

        public Thread Thread
        {
            get { return m_thread; }
        }

        protected NativeApplication()
        {
            m_thread = Thread.CurrentThread;
            WireUpApplicationEvents();
            ConstructionShared();
        }

        partial void ConstructionShared();

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            Core.ProcessMessage += OnProcessMessage;            
        }

        partial void InitializeResourcesShared();

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            Core.ProcessMessage -= OnProcessMessage;
            base.DisposeManagedResources();
        }

        partial void DisposeManagedResourcesShared();

        public virtual void PumpMessages(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            PlatformPumpMessages(cancellationToken);
            PumpMessagesShared(cancellationToken);
        }

        partial void PumpMessagesShared(CancellationToken cancellationToken);

        protected abstract void PlatformPumpMessages(CancellationToken cancellationToken);

        private void WireUpApplicationEvents()
        {
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
    }
}
