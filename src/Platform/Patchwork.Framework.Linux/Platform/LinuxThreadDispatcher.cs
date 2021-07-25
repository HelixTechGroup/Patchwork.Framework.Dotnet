using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Platform.Threading;

namespace Patchwork.Framework.Platform
{
    public class LinuxThreadDispatcher : INThreadDispatcher
    {
        private bool m_currentThreadIsLoopThread;
        private Thread m_thread;

        /// <inheritdoc />
        public event Action<NThreadDispatcherPriority?> Signaled;

        /// <inheritdoc />
        public bool CurrentThreadIsLoopThread
        {
            get { return m_currentThreadIsLoopThread; }
        }

        /// <inheritdoc />
        public Thread Thread
        {
            get { return m_thread; }
        }

        /// <inheritdoc />
        public IDisposable StartTimer(NThreadDispatcherPriority priority, TimeSpan interval, Action tick)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Signal(NThreadDispatcherPriority priority)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task InvokeAsync(Action action, NThreadDispatcherPriority priority = NThreadDispatcherPriority.Normal)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<TResult> InvokeAsync<TResult>(Func<TResult> function, NThreadDispatcherPriority priority = NThreadDispatcherPriority.Normal)
        {
            throw new NotImplementedException();
        }
    }
}
