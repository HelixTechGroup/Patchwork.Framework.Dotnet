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
        #endregion

        #region Properties
        public bool IsCreated
        {
            get { return m_isCreated; }
        }
        #endregion

        protected Creatable()
        {
            WireUpCreateEvents();
        }

        #region Methods
        public void Create()
        {
            if (m_isCreated)
                return;

            Creating.Raise(this, EventArgs.Empty);
            CreateResources();
            m_isCreated = true;
            Created.Raise(this, EventArgs.Empty);
        }

        protected virtual void CreateResources() { }

        protected virtual void OnCreated(object sender, EventArgs e) { }

        protected virtual void OnCreating(object sender, EventArgs e) { }

        protected override void DisposeManagedResources()
        {
            Creating.Dispose();
            Created.Dispose();
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