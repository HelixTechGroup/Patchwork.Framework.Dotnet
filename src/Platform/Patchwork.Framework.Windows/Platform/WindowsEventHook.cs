using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Interop.User32;
using Shin.Framework;
using Shin.Framework.Extensions;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using static Patchwork.Framework.Platform.Interop.Utilities;

namespace Patchwork.Framework.Platform
{
    public delegate void ProcessWindowsEventHandler(WindowsEvent winEvent);

    public class WindowsEventHook : Initializable
    {
        public event ProcessWindowsEventHandler ProcessEvent;

        #region Members
        protected SWEH_Events m_event;
        protected SWEH_Events m_eventEnd;
        protected WinEventProc m_hookProc;
        protected INHandle m_hookHandle;
        protected IWindowsProcess m_process;
        #endregion

        #region Properties
        public IWindowsProcess Process
        {
            get { return m_process; }
        }
        #endregion

        public WindowsEventHook(IWindowsProcess process, SWEH_Events eventType)
        {
            m_process = process;
            m_event = m_eventEnd = eventType;
        }

        public WindowsEventHook(IWindowsProcess process, SWEH_Events eventStart, SWEH_Events eventEnd)
        {
            m_process = process;
            m_event = eventStart;
            m_eventEnd = eventEnd;
        }

        public WindowsEventHook() { }

        protected void WinEventProc(IntPtr hWinEventHook,
                                    SWEH_Events eventType,
                                    IntPtr hwnd,
                                    SWEH_ObjectId idObject,
                                    uint idChild,
                                    uint dwEventThread,
                                    uint dwmsEventTime)
        {
            if (hwnd == m_process.Handle.Pointer)
                ProcessEvent?.Invoke(new WindowsEvent(hwnd, eventType, idObject, idChild, dwEventThread, dwmsEventTime));
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_hookProc = WinEventProc;
            var hhnd = IntPtr.Zero;
            var mhnd = IntPtr.Zero;
            using var module = System.Diagnostics.Process.GetCurrentProcess().MainModule;
            var threadId = GetWindowThreadProcessId(m_process.Handle.Pointer, out var pid);
            CheckLastError();
            mhnd = GetModuleHandle(module?.ModuleName);
            CheckLastError();
            hhnd = SetWinEventHook(m_event,
                           m_eventEnd,
                           mhnd,
                           m_hookProc,
                           pid.ToUint(),
                           threadId,
                           (uint)SWEH_dwFlags.WINEVENT_INCONTEXT);
            CheckLastError();

            m_hookHandle = new NHandle(hhnd, "");
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            UnhookWinEvent(m_hookHandle.Pointer);
            base.DisposeUnmanagedResources();
        }
    }
}
