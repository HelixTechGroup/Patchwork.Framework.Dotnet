#region Usings
using System;
using System.Threading;
using Patchwork.Framework;
using Patchwork.Framework.Platform;
using Shin.Framework;
using Shin.Framework.Extensions;
#endregion

namespace Patchwork.Framework.Platform
{
    public abstract class NResource : NResource<object>, INResource
    {
    }

    public abstract class NResource<TNative> : Creatable, INResource<TNative>
    {
        #region Members
        protected string m_name;
        protected TNative m_resource;
        protected INHandle m_handle;
        #endregion

        #region Properties
        /// <inheritdoc />
        public TNative Resource
        {
            get { return m_resource; }
        }

        /// <inheritdoc />
        public INHandle Handle
        {
            get { return m_handle; }
        }

        /// <inheritdoc />
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <inheritdoc />
        object INResource.Resource
        {
            get { return Resource; }
        }
        #endregion

        protected NResource() { }

        protected NResource(TNative native)
        {
            m_resource = native;
        }

        #region Methods
        //protected abstract bool PlatformClone(out object clone);


        protected internal void SetNativeResource(TNative resource)
        {
            m_resource = resource;
        }
        #endregion

        /// <inheritdoc />
        object ICloneable.Clone()
        {
            object result = null;
        //    PlatformClone(out result);
        //    Cloned.Raise(this, result);
            return result;
        }

        //public event EventHandler<object> Cloned;

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            m_handle?.Dispose();
            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        INResource<TNative> ICloneable<INResource<TNative>>.Clone()
        {
            return this;
        }
    }
}