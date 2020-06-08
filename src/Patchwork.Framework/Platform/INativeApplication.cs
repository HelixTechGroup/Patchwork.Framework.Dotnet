using System;
using System.Collections.Generic;
using System.Threading;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public interface INativeApplication : INativeObject, IInitialize, IDispose
    {
        event EventHandler<INativeWindow> WindowCreated;
        event EventHandler<INativeWindow> WindowDestroyed;

        #region Properties
        Thread Thread { get; }
        IEnumerable<INativeWindow> Windows { get; }
        #endregion

        INativeWindow CreateWindow();

        void PumpMessages(CancellationToken cancellationToken);

        bool CreateConsole();

        void CloseConsole();
    }
}