using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Platform.Runtime
{
    public abstract class NResourceAllocator : Allocator<INResource>
    {
    }

    public abstract class NResourceAllocator<T> : Allocator<T> where T : class, INResource
    {
    }

    public abstract class NResourceAllocator<TNative, TResource>: NResourceAllocator<TResource>, INResource<TNative> where TResource : class, INResource
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

        #region Methods
        //protected abstract object PlatformClone();

        protected internal void SetNativeResource(TNative resource)
        {
            m_resource = resource;
        }
        #endregion

        /// <inheritdoc />
        INResource<TNative> ICloneable<INResource<TNative>>.Clone()
        {
            return this;
        }

        object ICloneable.Clone() { return this; }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_handle.Dispose();
            base.DisposeManagedResources();
        }
    }
}
