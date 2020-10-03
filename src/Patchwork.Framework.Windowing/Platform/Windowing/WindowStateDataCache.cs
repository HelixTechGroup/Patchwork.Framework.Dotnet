#region Usings
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    [StructLayout(LayoutKind.Sequential)]
    internal partial struct WindowStateDataCache : IWindowDataCache
    {
        #region Members
        private Size m_clientSize;
        private NWindowDefinition m_definition;
        private bool m_isActive;
        private bool m_isEnabled;
        private bool m_isFocused;
        private bool m_isResizable;
        private bool m_isValid;
        private bool m_isVisible;
        private Point m_position;
        private readonly bool m_previousActive;
        private Size m_previousClientSize;
        private bool m_previousEnabled;
        private bool m_previousFocus;
        private Point m_previousPosition;
        private Size m_previousSize;
        private string m_previousTitle;
        private bool m_previousVisible;
        private Size m_size;
        private string m_title;
        #endregion

        #region Properties
        public Rectangle ClientArea { get; set; }

        public Size ClientSize
        {
            get { return m_clientSize; }
            set
            {
                m_previousClientSize = m_clientSize;
                m_clientSize = value;
            }
        }

        public NWindowDefinition Definition
        {
            get { return m_definition; }
            set
            {
                m_definition = value;
                SetDefinitionData();
            }
        }

        public bool IsActive
        {
            get { return m_isActive; }
            set
            {
                m_previousFocus = m_isActive;
                m_isActive = value;
            }
        }

        public bool IsEnabled
        {
            get { return m_isEnabled; }
            set
            {
                m_previousEnabled = m_isEnabled;
                m_isEnabled = value;
            }
        }

        public bool IsFocused
        {
            get { return m_isFocused; }
            set
            {
                m_previousFocus = m_isFocused;
                m_isFocused = value;
            }
        }

        public bool IsResizable
        {
            get { return m_isResizable; }
            set { m_isResizable = value; }
        }

        public bool IsValid
        {
            get { return m_isValid; }
        }

        public bool IsVisible
        {
            get { return m_isVisible; }
            set
            {
                m_previousVisible = m_isVisible;
                m_isVisible = value;
            }
        }

        public Size MaxClientSize { get; set; }

        public Point Position
        {
            get { return m_position; }
            set
            {
                m_previousPosition = m_position;
                m_position = value;
            }
        }

        public Size PreviousClientSize
        {
            get { return m_previousClientSize; }
        }

        public bool PreviouslyActive
        {
            get { return m_previousActive; }
        }

        public bool PreviouslyEnabled
        {
            get { return m_previousEnabled; }
        }

        public bool PreviouslyFocused
        {
            get { return m_previousFocus; }
        }

        public bool PreviouslyVisible
        {
            get { return m_previousVisible; }
        }

        public Point PreviousPosition
        {
            get { return m_previousPosition; }
        }

        public Size PreviousSize
        {
            get { return m_previousSize; }
        }

        public string PreviousTitle
        {
            get { return m_previousTitle; }
        }

        public Size Size
        {
            get { return m_size; }
            set
            {
                m_previousSize = m_size;
                m_size = value;
            }
        }

        public string Title
        {
            get { return m_title; }
            set
            {
                m_previousTitle = m_title;
                m_title = value;
            }
        }
        #endregion

        #region Methods
        public void Invalidate()
        {
            m_isValid = false;
        }

        public void Validate()
        {
            m_isValid = true;
        }

        public void Reset()
        {
            SetDefinitionData();
        }

        private void SetDefinitionData()
        {
            Title = Definition.Title;
            Size = m_definition.DesiredSize;
            Position = m_definition.DesiredPosition;
            SetDefinitionDataShared();
        }

        partial void SetDefinitionDataShared();
        #endregion
    }
}