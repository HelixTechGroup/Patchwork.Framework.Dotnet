#region Usings
using System;
#endregion

namespace Patchwork.Framework.Messaging
{
    public struct PlatformMessage : IPlatformMessage<IMessageData>
    {
        #region Members
        private readonly IMessageData m_data;
        private readonly MessageIds m_id;
        private readonly DateTime m_timeStamp;
        #endregion

        #region Properties
        /// <inheritdoc />
        public IMessageData Data
        {
            get { return m_data; }
        }

        /// <inheritdoc />
        public MessageIds Id
        {
            get { return m_id; }
        }

        /// <inheritdoc />
        public IMessageData RawData
        {
            get { return m_data; }
        }

        /// <inheritdoc />
        public DateTime TimeStamp
        {
            get { return m_timeStamp; }
        }
        #endregion

        public PlatformMessage(MessageIds id, IMessageData data = null)
        {
            m_id = id;
            m_data = data;
            m_timeStamp = DateTime.Now.ToUniversalTime();
        }
    }

    public struct PlatformMessage<TMessageData> : IPlatformMessage<TMessageData> where TMessageData : IMessageData
    {
        #region Members
        private readonly TMessageData m_data;
        private readonly MessageIds m_id;
        private readonly DateTime m_timeStamp;
        #endregion

        #region Properties
        /// <inheritdoc />
        public TMessageData Data
        {
            get { return m_data; }
        }

        /// <inheritdoc />
        public MessageIds Id
        {
            get { return m_id; }
        }

        /// <inheritdoc />
        public IMessageData RawData
        {
            get { return m_data; }
        }

        /// <inheritdoc />
        public DateTime TimeStamp
        {
            get { return m_timeStamp; }
        }
        #endregion

        public PlatformMessage(MessageIds id, TMessageData data = default)
        {
            m_id = id;
            m_data = data;
            m_timeStamp = DateTime.Now.ToUniversalTime();
        }
    }
}