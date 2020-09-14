#region Usings
using System;
using System.Collections.Generic;
using Patchwork.Framework.Platform.Window;
#endregion

namespace Patchwork.Framework.Platform
{
    public partial interface INativeApplication
    {
        #region Events
        event EventHandler<INativeWindow> WindowCreated;
        event EventHandler<INativeWindow> WindowDestroyed;
        #endregion

        #region Properties
        INativeWindow CurrentWindow { get; }
        INativeWindow MainWindow { get; }
        IEnumerable<INativeWindow> Windows { get; }
        #endregion

        #region Methods
        INativeWindow CreateWindow();
        #endregion
    }
}