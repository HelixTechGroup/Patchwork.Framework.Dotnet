using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Platform.Windowing
{
    public partial class NWindow
    {
        /// <inheritdoc />
        public bool IsRenderable
        {
            get { return m_cache.IsRenderable; }
        }
    }
}
