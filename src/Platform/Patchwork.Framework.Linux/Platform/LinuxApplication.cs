using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public class LinuxApplication : Initializable, INApplication
    {
        private INHandle m_handle;
        private Thread m_thread;
        private IEnumerable<INWindow> m_windows;

        /// <inheritdoc />
        public INHandle Handle
        {
            get { return m_handle; }
        }

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowCreated;

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowDestroyed;

        /// <inheritdoc />
        public Thread Thread
        {
            get { return m_thread; }
        }

        /// <inheritdoc />
        public IEnumerable<INWindow> Windows
        {
            get { return m_windows; }
        }

        /// <inheritdoc />
        public INWindow CreateWindow()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void PumpMessages(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool OpenConsole()
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
