using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform;

namespace Patchwork.Framework.Messaging
{
    public sealed class MobileWindowMessage : WindowMessage<INativeMobileWindow>
    {
        /// <inheritdoc />
        public MobileWindowMessage(WindowMessageIds messageId, INativeMobileWindow window) : base(messageId, window) { }
    }
}
