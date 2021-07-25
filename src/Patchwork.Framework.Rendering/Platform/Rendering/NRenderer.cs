#region Usings
using System;
using System.Drawing;
using System.Threading;
using Shin.Framework;
using Shin.Framework.Extensions;
using Shin.Framework.Threading;
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
        protected bool m_isEnabled;
        protected bool m_checkEnabled;
        protected bool m_handleRenderLoop = true;
        protected readonly ReaderWriterLockSlim m_lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        protected RenderPriority m_priority = RenderPriority.Normal;
        protected RenderStage m_level = RenderStage.Application;
        protected static readonly object m_lock = new object();
        protected bool m_hasLock;
        protected readonly int m_lockTimeout = 50;
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
        public RenderPriority Priority
        {
            get { return m_priority; }
        }

        /// <inheritdoc />
        public RenderStage Stage
        {
            get { return m_level; }
        }

        public bool IsRendering
        {
            get { return m_isRendering; }
        }

        public bool IsEnabled
        {
            get { return m_isEnabled; }
            set
            {
                m_isEnabled = value;
            }
        }

        public bool HandleRenderLoop
        {
            get { return m_handleRenderLoop; }
        }
        #endregion

        protected NRenderer(INRenderDevice device)
        {
            m_device = device;
            m_checkEnabled = true;
        }

        #region Methods
        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_isEnabled = true;
            m_isValid = m_isRendering = false;
        }

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
            //^ m_isValid

            if (!m_isEnabled ^ m_isRendering ^ !m_isInitialized ^ m_isDisposed)
                return;

            if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
                return;

            m_hasLock = true;
            try
            {
                //lock(m_lock)
                {
                    CheckEnabled();

                    m_isRendering = true;
                    PlatformRendering();
                    Rendering.Raise(this, null);
                    PlatformRender();
                    PlatformRendered();
                    Rendered.Raise(this, null);
                    m_isRendering = false;
                }
            }
            finally
            {
                if (m_hasLock)
                {
                    m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
            ///CheckEnabled();

        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_isEnabled = false;
            base.DisposeManagedResources();
        }

        protected virtual bool CheckEnabled()
        {
            if (!m_checkEnabled)
                return true;

            return false;
        }

        protected abstract bool PlatformValidate();

        protected abstract bool PlatformInvalidate();

        protected abstract void PlatformRender();

        protected abstract void PlatformRendering();

        protected abstract void PlatformRendered();
        #endregion
    }
}