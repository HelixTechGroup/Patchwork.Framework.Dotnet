using System;
using System.Collections.Generic;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;

namespace Patchwork.Framework.Platform
{
    public interface IWindowManager : IPlatformManager<AssemblyWindowingAttribute, IPlatformMessage<IWindowMessageData>>
    {
        /// <inheritdoc />
        INativeWindow MainWindow { get; }

        /// <inheritdoc />
        INativeWindow CurrentWindow { get; }

        /// <inheritdoc />
        IEnumerable<INativeWindow> Windows { get; }

        /// <inheritdoc />
        event EventHandler<INativeWindow> WindowCreated;

        /// <inheritdoc />
        event EventHandler<INativeWindow> WindowDestroyed;

        /// <inheritdoc />
        INativeWindow CreateWindow();

        INativeWindow CreateWindow(NativeWindowDefinition definition);
    }
}