namespace Patchwork.Framework.Platform.Windowing
{
    internal partial struct WindowStateDataCache
    {
        #region Members
        private bool m_isTopmostWindow;
        private bool m_isVisibleInTaskbar;
        private NWindowMode m_mode;
        private NWindowMode m_previousMode;
        private NWindowState m_previousState;
        private NWindowState m_state;
        #endregion

        #region Properties
        public bool IsTopmostWindow
        {
            get { return m_isTopmostWindow; }
            set { m_isTopmostWindow = value; }
        }

        public bool IsVisibleInTaskbar
        {
            get { return m_isVisibleInTaskbar; }
            set { m_isVisibleInTaskbar = value; }
        }

        public NWindowMode Mode
        {
            get { return m_mode; }
            set
            {
                m_previousMode = m_mode;
                m_mode = value;
            }
        }

        public NWindowMode PreviousMode
        {
            get { return m_previousMode; }
        }

        public NWindowState PreviousState
        {
            get { return m_previousState; }
        }

        public NWindowState State
        {
            get { return m_state; }
            set
            {
                m_previousState = m_state;
                m_state = value;
            }
        }
        #endregion

        #region Methods
        partial void SetDefinitionDataShared()
        {
            m_state = m_definition.InitialState;
            m_mode = m_definition.InitialMode;
            m_isVisibleInTaskbar = m_definition.IsVisibleInTaskbar;
            m_isTopmostWindow = m_definition.IsTopmostWindow;
            m_isRenderable = m_definition.IsRenderable;
        }
        #endregion
    }
}