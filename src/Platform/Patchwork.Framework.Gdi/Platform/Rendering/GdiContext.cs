using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;

namespace Patchwork.Framework.Platform.Rendering
{
    public sealed class GdiContext : Initializable, INRenderContext
    {
        private GdiHdcManager m_hdcManager;

        public INHandle CurrentContext
        {
            get { return m_hdcManager.CurrentWindowHdc; }
        }

        public INHandle this[INWindow window]
        {
            get { return m_hdcManager[window]; }
        }

        /// <inheritdoc />
        public INHandle Create(INWindow window)
        {
            return m_hdcManager.CreateHdc(window);
        }

        /// <inheritdoc />
        public void Destroy(INWindow window)
        {
            m_hdcManager.DestroyHdc(window);
        }

        /// <inheritdoc />
        public INHandle Clone(INWindow window)
        {
            return m_hdcManager.CloneHdc(window);
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_hdcManager = new GdiHdcManager();
            m_hdcManager.Initialize();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_hdcManager.Dispose();

            base.DisposeManagedResources();
        }
    }
}
