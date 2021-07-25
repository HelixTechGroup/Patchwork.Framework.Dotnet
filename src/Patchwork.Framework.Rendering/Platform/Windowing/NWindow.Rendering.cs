using System;
using System.Collections.Generic;
using System.Linq;
using Patchwork.Framework.Platform.Rendering;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform.Windowing
{
    public partial class NWindow
    {
        protected IList<INRenderer> m_renders;

        #region Properties
        /// <inheritdoc />
        public bool IsRenderable
        {
            get { return m_cache.IsRenderable; }
        }

        /// <inheritdoc />
        public void AddRenderer(params INRenderer[] renderer)
        {
            m_renders.AddRange(renderer);
            if (!m_isInitialized) 
                return;

            //foreach (var r in renderer)
            //    r.Initialize();
        }
        #endregion

        partial void InitializeResourcesShared2()
        {
            m_renders = new ConcurrentList<INRenderer>();
        }

        public virtual void Render()
        {
            if (!m_isInitialized)
                return;

            foreach (var r in m_renders/*.Where(r => !r.ContainsInterface(typeof(IFrameBufferRenderer)))*/)
                r.Render();
        }
    }
}