using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform;

namespace Patchwork.Framework
{
    public static partial class Core
    {
        public static IWindowManager Window { get { return m_managers[typeof(IWindowManager)].Value as IWindowManager; } }
    }
}
