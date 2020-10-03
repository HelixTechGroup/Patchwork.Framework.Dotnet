#region Usings
using System;
using System.Collections.Generic;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Windowing;
#endregion

namespace Patchwork.Framework
{
    public interface IPlatformWindowManager : IPlatformManager<AssemblyWindowingAttribute, IPlatformMessage<IWindowMessageData>>
    {
        #region Events
        /// <inheritdoc />
        event EventHandler<INWindow> WindowCreated;

        /// <inheritdoc />
        event EventHandler<INWindow> WindowDestroyed;
        #endregion

        #region Properties
        /// <inheritdoc />
        INWindow CurrentWindow { get; }

        /// <inheritdoc />
        INWindow MainWindow { get; }

        /// <inheritdoc />
        IEnumerable<INWindow> Windows { get; }
        #endregion

        #region Methods
        /// <inheritdoc />
        INWindow CreateWindow();

        INWindow CreateWindow(NWindowDefinition definition);
        #endregion
    }
}