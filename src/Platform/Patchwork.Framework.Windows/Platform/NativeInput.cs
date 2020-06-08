using System;
using Patchwork.Framework.Platform.Interop.User32;

namespace Patchwork.Framework.Platform
{
    public class NativeInput : WindowsProcessHook, INativeInput
    {
        /// <inheritdoc />
        public NativeInput(IWindowsProcess process) : base(process, WindowHookType.WH_GETMESSAGE) { }

        /// <inheritdoc />
        protected override IntPtr OnGetMsg(WindowsMessage message)
        {
            switch (message.Id)
            {
                case WindowsMessageIds.INPUT:
                    break;
            }

            return base.OnGetMsg(message);
        }
    }
}
