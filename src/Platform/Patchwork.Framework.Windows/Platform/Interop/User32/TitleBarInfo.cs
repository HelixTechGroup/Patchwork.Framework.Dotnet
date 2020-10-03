#region Usings
using System.Drawing;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TitleBarInfo
    {
        #region Members
        public ElementSystemStates CloseButtonStates;
        public ElementSystemStates HelpButtonStates;
        public ElementSystemStates MaximizeButtonStates;
        public ElementSystemStates MinimizeButtonStates;
        public uint Size;
        public Rectangle TitleBarRect;
        public ElementSystemStates TitleBarStates;
        private readonly ElementSystemStates Reserved;
        #endregion
    }
}