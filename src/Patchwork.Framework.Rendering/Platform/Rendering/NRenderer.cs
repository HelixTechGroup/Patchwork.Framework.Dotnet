﻿#region Usings
using System;
using System.Drawing;
using Shin.Framework;
using Shin.Framework.Extensions;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderer : Initializable, INRenderer
    {
        #region Events
        /// <inheritdoc />
        public event EventHandler Rendered;

        /// <inheritdoc />
        public event EventHandler Rendering;
        #endregion

        #region Members
        protected INRenderDevice m_device;
        protected INScreen m_screen;
        protected Size m_size;
        protected Size m_virutalSize;
        protected bool m_isValid;
        protected bool m_isRendering;

        /// <inheritdoc />
        protected int m_priority = 1;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INScreen Screen
        {
            get { return m_screen; }
        }

        /// <inheritdoc />
        public Size Size
        {
            get { return m_size; }
        }

        /// <inheritdoc />
        public Size VirutalSize
        {
            get { return m_virutalSize; }
        }

        /// <inheritdoc />
        public int Priority
        {
            get { return m_priority; }
        }

        public bool IsRendering
        {
            get { return m_isRendering; }
        }
        #endregion

        protected NRenderer(INRenderDevice device)
        {
            m_device = device;
        }

        #region Methods
        public bool Invalidate()
        {
            m_isValid = false;
            return PlatformInvalidate();
        }

        public bool Validate()
        {
            m_isValid = true;
            return PlatformValidate();
        }

        /// <inheritdoc />
        public void Render()
        {
            if (m_isValid ^ m_isRendering)
                return;

            m_isRendering = true;
            PlatformRendering();
            Rendering.Raise(this, null);
            PlatformRender();
            PlatformRendered();
            Rendered.Raise(this, null);
            m_isRendering = false;
        }

        protected virtual void OnPaint() { }

        protected abstract bool PlatformValidate();

        protected abstract bool PlatformInvalidate();

        protected abstract void PlatformRender();

        protected abstract void PlatformRendering();

        protected abstract void PlatformRendered();
        #endregion
    }
}