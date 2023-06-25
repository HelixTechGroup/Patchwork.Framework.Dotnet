using System;
using System.Drawing;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.Collections.Concurrent;

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderContext<TNative> : NRenderTarget<TNative>, INRenderContext
    {
        protected static INRenderContext m_instance;
        protected ConcurrentList<INWindow> m_windows;
        protected bool m_isBound;

        protected NRenderContext(INRenderDevice device) : base(device) { }

        /// <inheritdoc />
        public INRenderContext this[INWindow window]
        {
            get { return this; }
        }

        /// <inheritdoc />
        public static INRenderContext CurrentContext
        {
            get { return m_instance; }
        }

        /// <inheritdoc />
        public bool Bind(INWindow window, bool force = false)
        {
            //{
            //    Create();
            //    //Initialize();
            //}

            lock (m_lock)
            {
                if (m_isBound && !force)
                    return false;

                if (!m_isCreated || !m_isInitialized)
                    return m_isBound = false;
                
                m_isBound = PlatformBind(window, force);
                if(m_isBound)
                    m_windows.Add(window);

                return true;
            }
        }

        /// <inheritdoc />
        public bool Unbind(INWindow window, bool force = false)
        {
            lock (m_lock)
            {
                if (!m_isBound && !force)
                    return false;

                if (!m_isCreated || !m_isInitialized)
                    return m_isBound = false;
                m_isBound = PlatformUnbind(window, force);
                if (!m_isBound)
                    m_windows.Remove(window);

                return true;
            }
        }

        /// <inheritdoc />
        public void Resize(int width, int height)
        {
            Resize(m_windows[0], new Size(width, height));
        }

        /// <inheritdoc />
        public void Resize(Size size)
        {
            Resize(m_windows[0], size);
        }

        /// <inheritdoc />
        public void Resize(INWindow window, Size size)
        {
            lock (m_lock)
            {
                PlatformResize(window, size);
            }
        }

        protected abstract void PlatformResize(INWindow window, Size size);

        protected abstract bool PlatformBind(INWindow window, bool force = false);

        protected abstract bool PlatformUnbind(INWindow window, bool force = false);

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_windows = new ConcurrentList<INWindow>();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            m_windows.Clear();
        }
    }

    /// <inheritdoc />
    public abstract class NRenderContext : NRenderTarget, INRenderContext
    {
        protected static INRenderContext m_instance;
        protected ConcurrentList<INWindow> m_windows;
        protected bool m_isBound;

        protected NRenderContext(INRenderDevice device) : base(device) { }

        /// <inheritdoc />
        public INRenderContext this[INWindow window]
        {
            get { return this; }
        }

        /// <inheritdoc />
        public static INRenderContext CurrentContext
        {
            get { return m_instance; }
        }

        /// <inheritdoc />
        public bool Bind(INWindow window, bool force = false)
        {
            //{
            //    Create();
            //    //Initialize();
            //}

            lock (m_lock)
            {
                if (m_isBound && !force)
                    return false;

                if (!m_isCreated || !m_isInitialized)
                    return m_isBound = false;

                m_isBound = PlatformBind(window, force);
                if (m_isBound)
                    m_windows.Add(window);

                return true;
            }
        }

        /// <inheritdoc />
        public bool Unbind(INWindow window, bool force = false)
        {
            lock (m_lock)
            {
                if (!m_isBound && !force)
                    return false;

                if (!m_isCreated || !m_isInitialized)
                    return m_isBound = false;
                m_isBound = PlatformUnbind(window, force);
                if (!m_isBound)
                    m_windows.Remove(window);

                return true;
            }
        }

        /// <inheritdoc />
        public void Resize(int width, int height)
        {
            Resize(m_windows[0], new Size(width, height));
        }

        /// <inheritdoc />
        public void Resize(Size size)
        {
            Resize(m_windows[0], size);
        }

        /// <inheritdoc />
        public void Resize(INWindow window, Size size)
        {
            lock (m_lock)
            {
                PlatformResize(window, size);
            }
        }

        protected abstract void PlatformResize(INWindow window, Size size);

        protected abstract bool PlatformBind(INWindow window, bool force = false);

        protected abstract bool PlatformUnbind(INWindow window, bool force = false);

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_windows = new ConcurrentList<INWindow>();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            m_windows.Clear();
        }
    }
}