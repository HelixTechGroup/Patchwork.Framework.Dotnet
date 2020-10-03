#region Usings
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
        #region Members
        public ushort Day;
        public ushort DayOfWeek;
        public ushort Hour;
        public ushort Milliseconds;
        public ushort Minute;
        public ushort Month;
        public ushort Second;
        public ushort Year;
        #endregion
    }
}