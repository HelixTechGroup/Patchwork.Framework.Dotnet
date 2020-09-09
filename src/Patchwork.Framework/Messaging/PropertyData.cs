using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Messaging
{
    public abstract class PropertyData<T>
    {
        #region Members
        protected readonly T m_value;
        #endregion

        #region Properties
        public T Value
        {
            get { return m_value; }
        }

        protected PropertyData(T value)
        {
            m_value = value;
        }
        #endregion
    }
}
