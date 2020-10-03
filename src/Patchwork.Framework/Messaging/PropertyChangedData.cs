#region Usings
using Shin.Framework.ComponentModel;
#endregion

namespace Patchwork.Framework.Messaging
{
    public sealed class PropertyChangedData<T> : PropertyData<T>
    {
        #region Members
        private readonly T m_previousValue;
        private readonly T m_requestedValue;
        #endregion

        #region Properties
        public T PreviousValue
        {
            get { return m_previousValue; }
        }

        public T RequestedValue
        {
            get { return m_requestedValue; }
        }
        #endregion

        public PropertyChangedData(T currentValue, T requestedValue, T previousRequestedValue) : base(currentValue)
        {
            m_requestedValue = requestedValue;
            m_previousValue = previousRequestedValue;
        }

        #region Methods
        public PropertyChangedEventArgs<T> ToEventArgs()
        {
            return new PropertyChangedEventArgs<T>(m_value, m_requestedValue, m_previousValue);
        }
        #endregion
    }
}