using System;
using System.Threading;
using System.Threading.Tasks;
using Android.OS;
using Shield.Framework.Threading;

namespace Patchwork.Framework.Platform.Threading
{
    public class AndroidThreadDispatcher : INThreadDispatcher
    {
        private readonly AndroidApplication m_application;
        private Handler m_handler;

        /// <inheritdoc />
        public event Action<NThreadDispatcherPriority?> Signaled;

        #region Properties
        public bool CurrentThreadIsLoopThread
        {
            get { return Looper.MyLooper() == m_application?.Context.MainLooper; }
        }

        public Thread Thread
        {
            get { return m_application.Thread; }
        }
        #endregion

        public AndroidThreadDispatcher()
        {
            m_application = Core.Application as AndroidApplication;
            m_handler = new Handler(m_application?.Context.MainLooper);
        }

        /// <inheritdoc />
        public IDisposable StartTimer(NThreadDispatcherPriority priority, TimeSpan interval, Action tick)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Signal(NThreadDispatcherPriority priority)
        {
            m_handler.Post(() => Signaled?.Invoke(null));
        }

        /// <inheritdoc />
        public Task InvokeAsync(Action action, NThreadDispatcherPriority priority = NThreadDispatcherPriority.Normal)
        {
            var con = SynchronizationContext.Current;
            NThreadDispatcherSynchronizationContext.InstallIfNeeded();
            var ui = SynchronizationContext.Current;
            var task = Task.Factory.StartNew(
                                             action,
                                             new CancellationToken(),
                                             TaskCreationOptions.None,
                                             TaskScheduler.FromCurrentSynchronizationContext());
            //new Thread(() => renderer.Render()).Start(ui);
            SynchronizationContext.SetSynchronizationContext(con);

            return task;
        }

        /// <inheritdoc />
        public Task<TResult> InvokeAsync<TResult>(Func<TResult> function, NThreadDispatcherPriority priority = NThreadDispatcherPriority.Normal)
        {
            var con = SynchronizationContext.Current;
            NThreadDispatcherSynchronizationContext.InstallIfNeeded();
            var ui = SynchronizationContext.Current;
            var task = Task.Factory.StartNew(
                                             function,
                                             new CancellationToken(),
                                             TaskCreationOptions.None,
                                             TaskScheduler.FromCurrentSynchronizationContext());
            //new Thread(() => renderer.Render()).Start(ui);
            SynchronizationContext.SetSynchronizationContext(con);
            return task;
        }
    }
}
