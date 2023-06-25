#region Usings
using System;
using Shin.Framework;
using Shin.Framework.Extensions;
#endregion

namespace Patchwork.Framework
{
    public abstract class Creatable : Initializable, ICreate
    {
        #region Events
        public event EventHandler Created;
        public event EventHandler Creating;
        #endregion

        #region Members
        protected bool m_isCreated;
        protected bool m_preInitialize;
        protected bool m_postInitialize;
        #endregion

        #region Properties
        public bool IsCreated
        {
            get { return m_isCreated; }
        }
        #endregion

        protected Creatable()
        {
            m_preInitialize = true;
            WireUpCreateEvents();
        }

        #region Methods
        public void Create(bool force = false)
        {
            if (force && m_isCreated)
            {
                Dispose();
                m_isCreated = m_isInitialized = m_isDisposed = false;
            }

            if (m_isCreated /*^ m_isInitialized*/)
                return;

            if (m_preInitialize)
                Initialize();

            lock(m_lock)
            {
                Creating.Raise(this, EventArgs.Empty);
                m_isCreated = CreateResources(force);
                Throw.If(!m_isCreated).InvalidOperationException();
                Created.Raise(this, EventArgs.Empty);
            }

            if (!m_postInitialize) 
                return;

            m_isInitialized = false;
            Initialize();
        }

        protected abstract bool CreateResources(bool force);

        protected virtual void OnCreated(object sender, EventArgs e) { }

        protected virtual void OnCreating(object sender, EventArgs e) { }

        protected override void DisposeManagedResources()
        {
            Creating.Dispose();
            Created.Dispose();
            m_isCreated = m_isInitialized = false;
            base.DisposeManagedResources();
        }

        private void WireUpCreateEvents()
        {
            Creating += OnCreating;
            Created += OnCreated;
        }
        #endregion
    }
}