using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public class LinuxApplication : Initializable, INativeApplication
    {
        private INativeHandle m_handle;
        private Thread m_thread;
        private IEnumerable<INativeWindow> m_windows;

        /// <inheritdoc />
        public INativeHandle Handle
        {
            get { return m_handle; }
        }

        /// <inheritdoc />
        public event EventHandler<INativeWindow> WindowCreated;

        /// <inheritdoc />
        public event EventHandler<INativeWindow> WindowDestroyed;

        /// <inheritdoc />
        public Thread Thread
        {
            get { return m_thread; }
        }

        /// <inheritdoc />
        public IEnumerable<INativeWindow> Windows
        {
            get { return m_windows; }
        }

        /// <inheritdoc />
        public INativeWindow CreateWindow()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void PumpMessages(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool CreateConsole()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void CloseConsole()
        {
            throw new NotImplementedException();
        }
    }
}
