#region Usings
using System;
using Patchwork.Framework.Platform.Interop.User32;
#endregion

namespace Patchwork.Framework.Platform
{
    public class NInput : WindowsProcessHook, INInput
    {
        /// <inheritdoc />
        public NInput(IWindowsProcess process) : base(process, WindowHookType.WH_GETMESSAGE) { }

        #region Methods
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
        #endregion
    }
}