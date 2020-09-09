using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Messaging
{
    public sealed class PropertyChangingData<T> : PropertyData<T>
    {
        #region Members
        private readonly T m_requestedValue;
        #endregion

        #region Properties
        public T RequestedValue
        {
            get { return m_requestedValue; }
        }
        #endregion

        public PropertyChangingData(T currentValue, T requestedValue) : base(currentValue)
        {
            m_requestedValue = requestedValue;
        }

        public PropertyChangingEventArgs<T> ToEventArgs()
        {
            return new PropertyChangingEventArgs<T>(m_value, m_requestedValue);
        }
    }
}
