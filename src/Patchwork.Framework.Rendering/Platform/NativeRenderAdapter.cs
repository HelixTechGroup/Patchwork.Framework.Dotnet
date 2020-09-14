using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public partial class NativeRenderAdapter : Initializable, INativeRenderAdapter
    {
        private INativeRenderAdapterConfiguration m_configuration;

        /// <inheritdoc />
        public INativeRenderAdapterConfiguration Configuration
        {
            get { return m_configuration; }
        }
    }
}
