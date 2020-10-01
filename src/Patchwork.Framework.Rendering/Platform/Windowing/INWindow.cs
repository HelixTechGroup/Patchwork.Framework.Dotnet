using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Platform.Windowing
{
    public partial interface INWindow
    {
        bool IsRenderable { get; }
    }
}
