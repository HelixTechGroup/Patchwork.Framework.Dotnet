#region Usings
using System.Drawing;
#endregion

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public abstract class NRenderTarget : NRenderResource, INRenderTarget
    {
        #region Members
        protected Color m_backgroundColor;
        protected bool m_hasBegun;
        protected Size m_size;
        protected INRenderResource m_target;
        #endregion

        #region Properties
        /// <inheritdoc />
        public Size RenderSize
        {
            get { return m_size; }
        }

        /// <inheritdoc />
        public INRenderResource Target
        {
            get { return m_target; }
            set { m_target = value; }
        }
        #endregion

        /// <inheritdoc />
        protected NRenderTarget(INRenderDevice device) : base(device) { }

        #region Methods
        /// <inheritdoc />
        public void Clear()
        {
            Clear(m_backgroundColor);
        }

        /// <inheritdoc />
        public void Begin()
        {
            if ((!m_isCreated || !m_isInitialized) || m_hasBegun)
                return;

            lock(m_lock)
            {
                m_hasBegun = PlatformBegin();
            }
        }

        /// <inheritdoc />
        public void End()
        {
            if ((!m_isCreated || !m_isInitialized) || !m_hasBegun)
                return;

            lock(m_lock)
            {
                m_hasBegun = !PlatformEnd();
            }
        }

        /// <inheritdoc />
        public void Flush()
        {
            if ((!m_isCreated || !m_isInitialized) || m_hasBegun)
                return;

            lock(m_lock)
            {
                PlatformFlush();
            }
        }

        /// <inheritdoc />
        public void Clear(Color color)
        {
            if ((!m_isCreated || !m_isInitialized) || !m_hasBegun)
                return;

            lock(m_lock)
            {
                PlatformClear(color);
            }
        }

        /// <inheritdoc />
        public void Resize(Size size)
        {
            if ((!m_isCreated || !m_isInitialized) || m_hasBegun)
                return;

            lock(m_lock)
            {
                PlatformResize(size);
            }
        }

        /// <inheritdoc />
        public INRenderResource Bind(INRenderResource resource)
        {
            lock(m_lock)
            {
                return PlatformBind(resource);
            }
        }

        /// <inheritdoc />
        public INRenderResource Unbind(INRenderResource resource)
        {
            lock(m_lock)
            {
                return PlatformUnbind(resource);
            }
        }

        protected virtual void PlatformResize(Size size)
        {
            m_size = size;
        }

        protected abstract bool PlatformBegin();

        protected abstract bool PlatformEnd();

        protected abstract void PlatformFlush();

        protected abstract void PlatformClear(Color color);

        protected abstract INRenderResource PlatformBind(INRenderResource resource);

        protected abstract INRenderResource PlatformUnbind(INRenderResource resource);
        #endregion
    }

    public abstract class NRenderTarget<TNative> : NRenderResource<TNative>, INRenderTarget<TNative>
    {
        #region Members
        protected Color m_backgroundColor;
        protected bool m_hasBegun;
        protected Size m_size;
        protected INRenderResource m_target;
        #endregion

        #region Properties
        /// <inheritdoc />
        public Size RenderSize
        {
            get { return m_size; }
        }

        /// <inheritdoc />
        public INRenderResource Target
        {
            get { return m_target; }
            set { m_target = value; }
        }
        #endregion

        /// <inheritdoc />
        protected NRenderTarget(INRenderDevice device) : base(device) { }

        #region Methods
        /// <inheritdoc />
        public void Clear()
        {
            Clear(m_backgroundColor);
        }

        /// <inheritdoc />
        public void Begin()
        {
            if ((!m_isCreated || !m_isInitialized) || m_hasBegun)
                return;

            lock(m_lock)
            {
                m_hasBegun = PlatformBegin();
            }
        }

        /// <inheritdoc />
        public void End()
        {
            if ((!m_isCreated || !m_isInitialized) || !m_hasBegun)
                return;

            lock(m_lock)
            {
                m_hasBegun = !PlatformEnd();
            }
        }

        /// <inheritdoc />
        public void Flush()
        {
            if ((!m_isCreated || !m_isInitialized) || m_hasBegun)
                return;

            lock(m_lock)
            {
                PlatformFlush();
            }
        }

        /// <inheritdoc />
        public void Clear(Color color)
        {
            if ((!m_isCreated || !m_isInitialized) || !m_hasBegun)
                return;

            lock(m_lock)
            {
                PlatformClear(color);
            }
        }

        /// <inheritdoc />
        public void Resize(Size size)
        {
            if ((!m_isCreated || !m_isInitialized) || m_hasBegun)
                return;

            lock(m_lock)
            {
                PlatformResize(size);
            }
        }

        /// <inheritdoc />
        public INRenderResource Bind(INRenderResource resource)
        {
            lock(m_lock)
            {
                return PlatformBind(resource);
            }
        }

        /// <inheritdoc />
        public INRenderResource Unbind(INRenderResource resource)
        {
            lock(m_lock)
            {
                return PlatformUnbind(resource);
            }
        }

        protected virtual void PlatformResize(Size size)
        {
            m_size = size;
        }

        protected abstract bool PlatformBegin();

        protected abstract bool PlatformEnd();

        protected abstract void PlatformFlush();

        protected abstract void PlatformClear(Color color);

        protected abstract INRenderResource PlatformBind(INRenderResource resource);

        protected abstract INRenderResource PlatformUnbind(INRenderResource resource);
        #endregion
    }
}