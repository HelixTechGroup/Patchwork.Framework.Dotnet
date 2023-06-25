namespace Patchwork.Framework.Platform.Rendering
{
    public abstract partial class NRenderDeviceConfiguration : INRenderDeviceConfiguration
    {
        protected int m_bufferCount = 2;

        /// <inheritdoc />
        public int BufferCount
        {
            get { return m_bufferCount; }
            set { m_bufferCount = value; }
        }
    }
}