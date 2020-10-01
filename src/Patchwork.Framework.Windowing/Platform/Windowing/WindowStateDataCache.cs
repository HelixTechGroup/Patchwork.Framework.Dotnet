namespace Patchwork.Framework.Platform.Windowing
{
    internal partial struct WindowStateDataCache : IWindowDataCache
    {

        private bool m_isRenderable;
        private bool m_previousRenderable;

        public bool IsRenderable
        {
            get { return m_isRenderable; }
            set { m_previousRenderable = m_isRenderable; m_isRenderable = value; }
        }

        public bool PreviouslyRenderable
        {
            get { return m_previousRenderable; }
        }
    }
}
