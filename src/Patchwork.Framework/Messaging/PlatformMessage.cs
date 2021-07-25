#region Usings
using System;
using Shin.Framework.Messaging;
#endregion

namespace Patchwork.Framework.Messaging
{
    public struct PlatformMessage : IPlatformMessage<IMessageData>, IEquatable<PlatformMessage>
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

        /// <inheritdoc />
        public bool Equals(PlatformMessage other)
        {
            //if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            return TimeStamp == other.TimeStamp;
        }

        /// <inheritdoc />
        public bool Equals(IPlatformMessage other)
        {
            if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            return TimeStamp == other.TimeStamp;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            //ReferenceEquals(this, obj) ||
            return obj is PlatformMessage other && Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(IMessage other)
        {
            if (ReferenceEquals(null, other)) return false;
            return true;
        }

        /// <inheritdoc />
        public bool Equals(IMessage<MessageIds> other)
        {
            if (ReferenceEquals(null, other)) return false;
            return m_id == other.Id;
        }

        public static bool operator ==(PlatformMessage left, PlatformMessage right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PlatformMessage left, PlatformMessage right)
        {
            return !Equals(left, right);
        }
    }

    public struct PlatformMessage<TMessageData> : IPlatformMessage<TMessageData>, IEquatable<PlatformMessage<TMessageData>> where TMessageData : IMessageData
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

        /// <inheritdoc />
        public bool Equals(PlatformMessage<TMessageData> other)
        {
            //if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            return TimeStamp == other.TimeStamp;
        }

        /// <inheritdoc />
        public bool Equals(IPlatformMessage<TMessageData> other)
        {
            if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            return TimeStamp == other.TimeStamp;
        }

        /// <inheritdoc />
        public bool Equals(IMessage other)
        {
            if (ReferenceEquals(null, other)) return false;
            return true;
        }

        /// <inheritdoc />
        public bool Equals(IMessage<MessageIds> other)
        {
            if (ReferenceEquals(null, other)) return false;
            return m_id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            //ReferenceEquals(this, obj) ||
            return obj is PlatformMessage<TMessageData> other && Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(IPlatformMessage other)
        {
            if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            return TimeStamp == other.TimeStamp;
        }

        public static bool operator ==(PlatformMessage<TMessageData> left, PlatformMessage right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PlatformMessage<TMessageData> left, PlatformMessage right)
        {
            return !Equals(left, right);
        }
    }
}