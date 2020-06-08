using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Window;

namespace Patchwork.Framework.Messaging
{
    public class WindowMessage : PlatformMessage
    {
        protected readonly WindowMessageIds m_messageId;
        protected readonly INativeWindow m_window;

        public WindowMessage(WindowMessageIds messageId, INativeWindow window) : base(MessageIds.Window)
        {
            m_messageId = messageId;
            m_window = window;
        }

        public WindowMessageIds MessageId
        {
            get { return m_messageId; }
        }

        public INativeWindow Window
        {
            get { return m_window; }
        }
    }

    public abstract class WindowMessage<T> : WindowMessage where T : INativeWindow
    {
        protected new readonly T m_window;        

        /// <inheritdoc />
        protected WindowMessage(WindowMessageIds messageId, T window) : base(messageId, window)
        {
            m_window = window;
        }

        public new T Window
        {
            get { return m_window; }
        }
    }
}
