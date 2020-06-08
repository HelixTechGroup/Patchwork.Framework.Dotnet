using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Messaging
{
    public class PlatformMessage : IPlatformMessage
    {
        protected readonly MessageIds m_id;

        /// <inheritdoc />
        protected readonly DateTime m_timeStamp;

        /// <inheritdoc />
        public MessageIds Id
        {
            get { return m_id; }
        }

        public PlatformMessage(MessageIds id)
        {
            m_id = id;
            m_timeStamp = DateTime.Now.ToUniversalTime();
        }

        /// <inheritdoc />
        public DateTime TimeStamp
        {
            get { return m_timeStamp; }
        }
    }
}
