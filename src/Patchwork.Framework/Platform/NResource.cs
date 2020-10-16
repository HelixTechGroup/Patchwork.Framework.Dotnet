#region Usings
using Patchwork.Framework.Platform;
using Shin.Framework;
#endregion

namespace System.Drawing
{
    public abstract class NResource<TNative> : Initializable, ICloneable, INResource<TNative>
    {
        #region Members
        protected string m_name;
        protected TNative m_resource;
        #endregion

        #region Properties
        /// <inheritdoc />
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <inheritdoc />
        public TNative Resource
        {
            get { return m_resource; }
        }

        /// <inheritdoc />
        object INResource.Resource
        {
            get { return Resource; }
        }
        #endregion

        protected NResource() { }

        internal NResource(TNative native)
        {
            m_resource = native;
        }

        #region Methods
        public object Clone()
        {
            return PlatformClone();
        }

        protected abstract object PlatformClone();

        protected internal void SetNativeResource(TNative resource)
        {
            m_resource = resource;
        }
        #endregion
    }
}