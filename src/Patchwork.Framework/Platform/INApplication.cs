using System;
using System.Collections.Generic;
using System.Threading;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public partial interface INApplication : INObject, IInitialize, IDispose
    {
        #region Properties
        Thread Thread { get; }
        #endregion

        void PumpMessages(CancellationToken cancellationToken);
    }
}