#region Usings
using System;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Platform.Interop;
using Patchwork.Framework.Platform.Interop.User32;
using Shield.Framework.Threading;
using Shin.Framework.Collections.Concurrent;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
#endregion

namespace Patchwork.Framework.Platform.Threading
{
    public sealed class WinThreadDispatcher : INThreadDispatcher
    {
        #region Events
        public event Action<NThreadDispatcherPriority?> Signaled;
        #endregion

        #region Members
        private readonly INApplication m_application;
        private readonly ConcurrentList<Delegate> m_delegates = new ConcurrentList<Delegate>();
        private bool m_quitSend;
        #endregion

        #region Properties
        public bool CurrentThreadIsLoopThread
        {
            get { return m_application.Thread == Thread.CurrentThread; }
        }

        public Thread Thread
        {
            get { return m_application.Thread; }
        }
        #endregion

        public WinThreadDispatcher(INApplication application)
        {
            m_application = application;
        }

        #region Methods
        public IDisposable StartTimer(NThreadDispatcherPriority priority, TimeSpan interval, Action callback)
        {
            //UnmanagedMethods.TimerProc timerDelegate =
            //    (hWnd, uMsg, nIDEvent, dwTime) => callback();

            //IntPtr handle = UnmanagedMethods.SetTimer(
            //    IntPtr.Zero,
            //    IntPtr.Zero,
            //    (uint)interval.TotalMilliseconds,
            //    timerDelegate);

            //// Prevent timerDelegate being garbage collected.
            //_delegates.Add(timerDelegate);

            //return Disposable.Create(() =>
            //{
            //    _delegates.Remove(timerDelegate);
            //    UnmanagedMethods.KillTimer(IntPtr.Zero, handle);
            //});
            throw new NotImplementedException();
        }

        public void Signal(NThreadDispatcherPriority prio)
        {
            PostMessage(Core.Application.Handle.Pointer,
                        WindowsMessageIds.DISPATCH_WORK_ITEM,
                        new IntPtr(Constants.SignalW),
                        new IntPtr(Constants.SignalL));
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

        internal void Dispatch()
        {
            Signaled?.Invoke(null);
        }
        #endregion
    }
}