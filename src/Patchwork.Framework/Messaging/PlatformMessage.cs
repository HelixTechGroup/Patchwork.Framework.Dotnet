using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Messaging
{
    public struct PlatformMessage : IPlatformMessage
    {
        private MessageIds m_id;
        private DateTime m_timeStamp;
        private IMessageData m_data;

        /// <inheritdoc />
        public MessageIds Id
        {
            get { return m_id; }
        }

        /// <inheritdoc />
        public DateTime TimeStamp
        {
            get { return m_timeStamp; }
        }

        /// <inheritdoc />
        public IMessageData Data
        {
            get { return m_data; }
        }

        public PlatformMessage(MessageIds id, IMessageData data = null)
        {
            m_id = id;
            m_data = data;
            m_timeStamp = DateTime.Now.ToUniversalTime();
        }
    }

    //public struct PlatformMessage<TMessageData> : IPlatformMessage<TMessageData> where TMessageData : IMessageData
    //{
    //    private readonly MessageIds m_id;

    //    /// <inheritdoc />
    //    private readonly DateTime m_timeStamp;

    //    private TMessageData m_data;

    //    /// <inheritdoc />
    //    public MessageIds Id
    //    {
    //        get { return m_id; }
    //    }

    //    public PlatformMessage(MessageIds id, TMessageData data)
    //    {
    //        m_id = id;
    //        m_data = data;
    //        m_timeStamp = DateTime.Now.ToUniversalTime();
    //    }

    //    /// <inheritdoc />
    //    public DateTime TimeStamp
    //    {
    //        get { return m_timeStamp; }
    //    }

    //    /// <inheritdoc />
    //    IMessageData IPlatformMessage.Data
    //    {
    //        get { return Data; }
    //    }

    //    /// <inheritdoc />
    //    public TMessageData Data
    //    {
    //        get { return m_data; }
    //    }
    //}
}
