using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Patchwork.Framework.Platform.Window;

namespace Patchwork.Framework.Platform
{
    public partial interface INativeApplication
    {
        bool OpenConsole();
        void CloseConsole();
    }
}
