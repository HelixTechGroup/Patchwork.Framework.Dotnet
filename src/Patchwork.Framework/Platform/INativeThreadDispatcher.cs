#region Usings
using System;
using System.Threading;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface INativeThreadDispatcher
    {
        #region Events
        event Action<NativeThreadDispatcherPriority?> Signaled;
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
        IDisposable StartTimer(NativeThreadDispatcherPriority priority, TimeSpan interval, Action tick);

        void Signal(NativeThreadDispatcherPriority priority);
        #endregion
    }
}