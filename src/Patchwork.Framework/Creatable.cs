using System;
using System.Collections.Generic;
using System.Text;
using Shin.Framework;
using Shin.Framework.Extensions;

namespace Patchwork.Framework
{
    public interface ICreate : IInitialize, IDispose
    {
        event EventHandler Created;
        event EventHandler Creating;
        bool IsCreated { get; }

        void Create();
    }

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
