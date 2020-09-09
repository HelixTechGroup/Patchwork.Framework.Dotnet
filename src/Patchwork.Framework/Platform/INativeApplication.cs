using System;
using System.Collections.Generic;
using System.Threading;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public partial interface INativeApplication : INativeObject, IInitialize, IDispose
    {
        event EventHandler<INativeWindow> WindowCreated;
        event EventHandler<INativeWindow> WindowDestroyed;

        #region Properties
        Thread Thread { get; }
        INativeWindow MainWindow { get; }
        INativeWindow CurrentWindow { get; }
        IEnumerable<INativeWindow> Windows { get; }
        #endregion

        void PumpMessages(CancellationToken cancellationToken);

        INativeWindow CreateWindow();
    }
}