using System;

namespace Patchwork.Framework.Platform.Interop.User32 {
    [Flags]
    public enum PeekMessageFlags
    {
        /// <summary>
        ///     Messages are not removed from the queue after processing by PeekMessage.
        /// </summary>
        PM_NOREMOVE = 0x0000,

        /// <summary>
        ///     Messages are removed from the queue after processing by PeekMessage.
        /// </summary>
        PM_REMOVE = 0x0001,

        /// <summary>
        ///     Prevents the system from releasing any thread that is waiting for the caller to go idle (see WaitForInputIdle).
        ///     Combine this value with either PM_NOREMOVE or PM_REMOVE.
        /// </summary>
        PM_NOYIELD = 0x0002,

        /// <summary>
        ///     Process mouse and keyboard messages.
        /// </summary>
        PM_QS_INPUT = QueueStatusFlags.QS_INPUT << 16,

        /// <summary>
        ///     Process paint messages.
        /// </summary>
        PM_QS_PAINT = QueueStatusFlags.QS_PAINT << 16,

        /// <summary>
        ///     Process all posted messages, including timers and hotkeys.
        /// </summary>
        PM_QS_POSTMESSAGE =
            (QueueStatusFlags.QS_POSTMESSAGE | QueueStatusFlags.QS_HOTKEY | QueueStatusFlags.QS_TIMER) << 16,

        /// <summary>
        ///     Process all sent messages.
        /// </summary>
        PM_QS_SENDMESSAGE = QueueStatusFlags.QS_SENDMESSAGE << 16
    }
}