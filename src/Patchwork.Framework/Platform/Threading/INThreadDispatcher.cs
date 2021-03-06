﻿#region Usings
using System;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace Patchwork.Framework.Platform.Threading
{
    public interface INThreadDispatcher
    {
        #region Events
        event Action<NThreadDispatcherPriority?> Signaled;
        #endregion

        #region Properties
        bool CurrentThreadIsLoopThread { get; }
        Thread Thread { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Starts a timer.
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="interval">The interval.</param>
        /// <param name="tick">The action to call on each tick.</param>
        /// <returns>An <see cref="IDisposable"/> used to stop the timer.</returns>
        IDisposable StartTimer(NThreadDispatcherPriority priority, TimeSpan interval, Action tick);

        void Signal(NThreadDispatcherPriority priority);

        Task InvokeAsync(Action action, NThreadDispatcherPriority priority = NThreadDispatcherPriority.Normal);

        Task<TResult> InvokeAsync<TResult>(Func<TResult> function, NThreadDispatcherPriority priority = NThreadDispatcherPriority.Normal);
        #endregion
    }
}