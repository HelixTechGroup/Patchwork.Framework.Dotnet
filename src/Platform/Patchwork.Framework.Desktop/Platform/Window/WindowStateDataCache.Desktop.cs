using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Shin.Framework;

namespace Patchwork.Framework.Platform.Window
{
    internal partial struct WindowStateDataCache
        {
            private NativeWindowState m_state;
            private NativeWindowState m_previousState;
            private bool m_isVisibleInTaskbar;
            private bool m_isTopmostWindow;
            private NativeWindowMode m_mode;
            private NativeWindowMode m_previousMode;

            public NativeWindowState State
            {
                get { return m_state; }
                set { m_previousState = m_state; m_state = value; }
            }

            public NativeWindowState PreviousState
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

            public NativeWindowMode Mode
            {
                get { return m_mode; }
                set
                {
                    m_previousMode = m_mode;
                    m_mode = value;
                }
            }

            public NativeWindowMode PreviousMode
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
