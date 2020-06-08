using System.Runtime.InteropServices;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInputState
    {
        public uint Value;

        public KeyboardInputState(uint value)
        {
            this.Value = value;
        }

        /// <summary>
        ///     The repeat count for the current message. The value is the number of times the keystroke is autorepeated as a
        ///     result of the user holding down the key. If the keystroke is held long enough, multiple messages are sent. However,
        ///     the repeat count is not cumulative.
        /// </summary>
        public uint RepeatCount
        {
            get { return this.Value & 0x0000ffff; }
            set { this.Value = this.Value.WithLow(value.Low()); }
        }

        public uint ScanCode
        {
            get { return (this.Value >> 16) & 0x000000ff; }
            set
            {
                var mask = 0x00ff0000U;
                var newValue = (value << 16) & mask;
                this.Value = this.Value & ~mask | newValue;
            }
        }

        /// <summary>
        ///     Indicates whether the key is an extended key, such as the right-hand ALT and CTRL keys that appear on an enhanced
        ///     101- or 102-key keyboard. The value is 1 if it is an extended key; otherwise, it is 0.
        /// </summary>
        public bool IsExtendedKey
        {
            get { return unchecked(((int)this.Value >> 24) & 0x1) == 1; }
            set
            {
                var mask = 0b0000_0001_0000_0000_0000_0000_0000_0000U;
                this.Value = this.Value & ~mask | (value ? mask : 0U);
            }
        }

        /// <summary>
        ///     The value is 1 if the ALT key is down while the key is pressed; it is 0 if the WM_SYSKEYDOWN message is posted to
        ///     the active window because no window has the keyboard focus.
        /// </summary>
        public bool IsContextual
        {
            get { return unchecked(((int)this.Value >> 29) & 0x1) == 1; }
            set
            {
                var mask = 0b0010_0000_0000_0000_0000_0000_0000_0000U;
                this.Value = this.Value & ~mask | (value ? mask : 0U);
            }
        }

        /// <summary>
        ///     The value is 1 if the key is down before the message is sent, or it is 0 if the key is up.
        /// </summary>
        public bool IsPreviousKeyStatePressed
        {
            get { return unchecked(((int)this.Value >> 30) & 0x1) == 1; }
            set
            {
                var mask = 0b0100_0000_0000_0000_0000_0000_0000_0000U;
                this.Value = this.Value & ~mask | (value ? mask : 0U);
            }
        }

        /// <summary>
        ///     The value is 1 if the key is being released, or it is 0 if the key is being pressed.
        /// </summary>
        public bool IsKeyUpTransition
        {
            get { return unchecked(((int)this.Value >> 31) & 0x1) == 1; }
            set
            {
                var mask = 0b1000_0000_0000_0000_0000_0000_0000_0000U;
                this.Value = this.Value & ~mask | (value ? mask : 0U);
            }
        }
    }
}