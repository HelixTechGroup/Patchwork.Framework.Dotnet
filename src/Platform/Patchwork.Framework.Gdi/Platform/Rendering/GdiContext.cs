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

        /// <inheritdoc />
        public void Begin()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void End()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Flush()
        {
            throw new NotImplementedException();
        }

        public INRenderContext CurrentContext
        {
            get { return m_hdcManager.CurrentWindowHdc; }
        }

        public INRenderContext this[INWindow window]
        {
            get { return m_hdcManager[window]; }
        }

        /// <inheritdoc />
        public INRenderContext Bind(INWindow window)
        {
            return m_hdcManager.CreateHdc(window);
        }

        /// <inheritdoc />
        public void Unbind(INWindow window)
        {
            m_hdcManager.DestroyHdc(window);
        }

        /// <inheritdoc />
        public INRenderContext Clone(INWindow window)
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
