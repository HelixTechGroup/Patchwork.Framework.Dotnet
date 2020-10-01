namespace Patchwork.Framework.Platform.Windowing
{
    internal partial struct WindowStateDataCache
        {
            private NWindowState m_state;
            private NWindowState m_previousState;
            private bool m_isVisibleInTaskbar;
            private bool m_isTopmostWindow;
            private NWindowMode m_mode;
            private NWindowMode m_previousMode;

            public NWindowState State
            {
                get { return m_state; }
                set { m_previousState = m_state; m_state = value; }
            }

            public NWindowState PreviousState
            {
                get { return m_previousState; }
            }

            public bool IsVisibleInTaskbar
            {
                get { return m_isVisibleInTaskbar; }
                set { m_isVisibleInTaskbar = value; }
            }

            public bool IsTopmostWindow
            {
                get { return m_isTopmostWindow; }
                set { m_isTopmostWindow = value; }
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

            partial void SetDefinitionDataShared()
            {
                m_state = m_definition.InitialState;
                m_mode = m_definition.InitialMode;
                m_isVisibleInTaskbar = m_definition.IsVisibleInTaskbar;
                m_isTopmostWindow = m_definition.IsTopmostWindow;
            }
    }
}
