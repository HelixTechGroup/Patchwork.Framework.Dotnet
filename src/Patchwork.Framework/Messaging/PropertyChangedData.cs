using System;
using System.Collections.Generic;
using System.Text;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Messaging
{
    public sealed class PropertyChangedData<T> : PropertyData<T>
    {
        #region Members
        private readonly T m_requestedValue;
        private readonly T m_previousValue;
        #endregion

        #region Properties
        public T RequestedValue
        {
            get { return m_requestedValue; }
        }

        public T PreviousValue
        {
            get { return m_previousValue; }
        }
        #endregion

        public PropertyChangedData(T currentValue, T requestedValue, T previousRequestedValue) : base(currentValue)
        {
            m_requestedValue = requestedValue;
            m_previousValue = previousRequestedValue;
        }

        public PropertyChangedEventArgs<T> ToEventArgs()
        {
            return new PropertyChangedEventArgs<T>(m_value, m_requestedValue, m_previousValue);
        }
    }
}
