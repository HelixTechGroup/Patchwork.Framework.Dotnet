using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patchwork.Framework.Platform;
using Shin.Framework.Extensions;

namespace Patchwork.Framework
{
    public static partial class Core
    {
        public static IPlatformRenderManager Renderer { get { return IoCContainer.Resolve<IPlatformRenderManager>(); } }//m_container.ResolveAll<IPlatformManager>().Where(m => m.GetType().ContainsInterface<IWindowManager>()).First() as IWindowManager; } }
    }
}
