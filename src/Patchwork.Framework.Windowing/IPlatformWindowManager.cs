using System;
using System.Collections.Generic;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;

namespace Patchwork.Framework.Platform
{
    public interface IPlatformWindowManager : IPlatformManager<AssemblyWindowingAttribute, IPlatformMessage<IWindowMessageData>>
    {
        /// <inheritdoc />
        INWindow MainWindow { get; }

        /// <inheritdoc />
        INWindow CurrentWindow { get; }

        /// <inheritdoc />
        IEnumerable<INWindow> Windows { get; }

        /// <inheritdoc />
        event EventHandler<INWindow> WindowCreated;

        /// <inheritdoc />
        event EventHandler<INWindow> WindowDestroyed;

        /// <inheritdoc />
        INWindow CreateWindow();

        INWindow CreateWindow(NWindowDefinition definition);
    }
}