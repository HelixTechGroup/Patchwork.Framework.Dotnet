using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform;

namespace Patchwork.Framework.Messaging
{
    public sealed class DesktopWindowMessage : WindowMessage<INativeDesktopWindow>
    {
        /// <inheritdoc />
        public DesktopWindowMessage(WindowMessageIds messageId, INativeDesktopWindow window) : base(messageId, window) { }
    }
}
