#region Usings
using System;
using Patchwork.Framework.Platform.Interop.User32;
#endregion

namespace Patchwork.Framework.Platform
{
    public struct WindowsEvent
    {
        #region Members
        public IntPtr Hwnd;
        public SWEH_Events EventId;
        public SWEH_ObjectId ObjectId;
        uint ChildId;
        uint ThreadId;
        DateTime EventTime;
        #endregion

        public WindowsEvent(IntPtr hwnd, 
                              SWEH_Events eventId, 
                              SWEH_ObjectId objectId,
                              uint idChild,
                              uint dwEventThread,
                              uint dwmsEventTime)
        {
            Hwnd = hwnd;
            EventId = eventId;
            ObjectId = objectId;
            ChildId = idChild;
            ThreadId = dwEventThread;
            EventTime = DateTime.Now.AddMilliseconds((int)dwmsEventTime - System.Environment.TickCount);
        }
    }
}