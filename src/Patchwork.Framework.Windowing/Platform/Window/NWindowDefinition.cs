﻿#region Usings
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Window
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct NWindowDefinition
    {
        #region Members
        private Size m_maxClientSize;
        private bool m_acceptsInput;
        private bool m_isVisibleInTaskbar;
        private int m_cornerRadius;
        private Point m_desiredPosition;
        private Size m_desiredSize;
        private Size m_expectedMaxSize;
        private bool m_isModalWindow;
        private bool m_isRegularWindow;
        private bool m_isTopmostWindow;
        private bool m_isMainApplicationWindow;
        private float m_opacity;
        private bool m_preserveAspectRatio;
        private string m_title;
        private NWindowType m_type;
        private bool m_isResizable;
        #endregion

        #region Properties
        public Size MaxClientSize
        {
            get { return m_maxClientSize; }
            set { m_maxClientSize = value; }
        }

        public bool IsResizable
        {
            get { return m_isResizable; }
            set { m_isResizable = value; }
        }

        public bool AcceptsInput
        {
            get { return m_acceptsInput; }
            set { m_acceptsInput = value; }
        }

        public bool IsVisibleInTaskbar
        {
            get { return m_isVisibleInTaskbar; }
            set { m_isVisibleInTaskbar = value; }
        }

        public int CornerRadius
        {
            get { return m_cornerRadius; }
            set { m_cornerRadius = value; }
        }

        public Point DesiredPosition
        {
            get { return m_desiredPosition; }
            set { m_desiredPosition = value; }
        }

        public Size DesiredSize
        {
            get { return m_desiredSize; }
            set { m_desiredSize = value; }
        }

        public Size ExpectedMaxSize
        {
            get { return m_expectedMaxSize; }
            set { m_expectedMaxSize = value; }
        }

        public bool IsModalWindow
        {
            get { return m_isModalWindow; }
            set { m_isModalWindow = value; }
        }

        public bool IsRegularWindow
        {
            get { return m_isRegularWindow; }
            set { m_isRegularWindow = value; }
        }

        public bool IsTopmostWindow
        {
            get { return m_isTopmostWindow; }
            set { m_isTopmostWindow = value; }
        }

        public float Opacity
        {
            get { return m_opacity; }
            set { m_opacity = value; }
        }

        //bool SizeWillChangeOften;
        public bool PreserveAspectRatio
        {
            get { return m_preserveAspectRatio; }
            set { m_preserveAspectRatio = value; }
        }

        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        public NWindowType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        public bool IsMainApplicationWindow
        {
            get { return m_isMainApplicationWindow; }
            set { m_isMainApplicationWindow = value; }
        }
        #endregion

        //bool ManualDPI;
    }
}