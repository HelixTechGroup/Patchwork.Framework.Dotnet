using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Patchwork.Framework.Platform.Rendering;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform.Windowing
{
    public partial class NWindow
    {
        protected IList<INWindowRenderer> m_renders;
        private Size m_renderSize;
        private bool m_isRendering;

        #region Properties
        /// <inheritdoc />
        public bool IsRenderable
        {
            get { return m_cache.IsRenderable; }
        }

        /// <inheritdoc />
        public void AddRenderer(params INWindowRenderer[] renderer)
        {
            if (!m_isInitialized)
                return;

            m_renders.AddRange(renderer);

            //foreach (var r in renderer)
            //    r.Initialize();
        }
        #endregion

        partial void InitializeResourcesShared2()
        {
            m_renders = new ConcurrentList<INWindowRenderer>();
        }

        /// <inheritdoc />
        public event EventHandler Rendered;

        /// <inheritdoc />
        public event EventHandler Rendering;

        /// <inheritdoc />
        public bool IsRendering
        {
            get { return m_isRendering; }
        }

        /// <inheritdoc />
        public Size RenderSize
        {
            get { return m_renderSize; }
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