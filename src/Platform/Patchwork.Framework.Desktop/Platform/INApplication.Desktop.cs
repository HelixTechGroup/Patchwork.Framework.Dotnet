using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Patchwork.Framework.Platform.Window;

namespace Patchwork.Framework.Platform
{
    public partial interface INApplication
    {
        bool OpenConsole();
        void CloseConsole();
    }
}
