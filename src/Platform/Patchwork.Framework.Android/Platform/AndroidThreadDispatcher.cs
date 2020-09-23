using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Android.OS;

namespace Patchwork.Framework.Platform
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
    }
}
