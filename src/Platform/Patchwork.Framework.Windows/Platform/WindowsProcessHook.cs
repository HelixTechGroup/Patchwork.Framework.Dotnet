#region Usings
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Patchwork.Framework.Platform.Interop.User32;
using Shin.Framework;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
#endregion

namespace Patchwork.Framework.Platform
{
    public delegate IntPtr ProcessWindowsMessageHandler(WindowsMessage message);

    public class WindowsProcessHook : Initializable
    {
        #region Events
        public event ProcessWindowsMessageHandler ProcessMessage;
        #endregion

        #region Members
        protected INHandle m_hookHandle;
        protected HookProc m_hookProc;
        protected WindowHookType m_hookType;

        protected IWindowsProcess m_process;
        #endregion

        #region Properties
        public IWindowsProcess Process
        {
            get { return m_process; }
        }
        #endregion

        public WindowsProcessHook(IWindowsProcess process, WindowHookType hookType)
        {
            m_process = process;
            m_hookType = hookType;
        }

        #region Methods
        protected virtual IntPtr OnGetMsg(WindowsMessage message)
        {
            return IntPtr.Zero;
        }

        protected virtual IntPtr OnProcRet(WindowsMessage message)
        {
            return IntPtr.Zero;
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            UnhookWindowsHookEx(m_hookHandle.Pointer);
        }

        /// <inheritdoc />
        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_hookProc = HookProc;
            var hhnd = IntPtr.Zero;
            var mhnd = IntPtr.Zero;
            //using var proc = Process.GetCurrentProcess().MainModule;
            using var module = System.Diagnostics.Process.GetCurrentProcess().MainModule;
            var threadId = GetWindowThreadProcessId(m_process.Handle.Pointer, IntPtr.Zero);
            //foreach (ProcessModule module in proc.Modules)
            //{
            //    mhnd = GetModuleHandle(module.ModuleName);
            //    //hhnd = SetWindowsHookEx(m_hookType, m_hookProc, mhnd, 0);
            //    if (mhnd == IntPtr.Zero || hhnd == IntPtr.Zero)
            //        continue;
            //}

            mhnd = GetModuleHandle(module?.ModuleName);
            hhnd = SetWindowsHookEx(m_hookType, m_hookProc, mhnd, threadId);

            if (hhnd == IntPtr.Zero)
                throw new Win32Exception();

            m_hookHandle = new NHandle(hhnd, "");
        }

        private IntPtr HookProc(WindowHookCode nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return CallNextHookEx(m_hookHandle.Pointer, nCode, wParam, lParam);

            var wMsg = new WindowsMessage();
            switch (m_hookType)
            {
                case WindowHookType.WH_CALLWNDPROCRET:
                    var msgRet = (CwpRetStruct)Marshal.PtrToStructure(lParam, typeof(CwpRetStruct));
                    wMsg = new WindowsMessage
                           {
                               Id = (WindowsMessageIds)msgRet.Message,
                               WParam = msgRet.WParam,
                               LParam = msgRet.LParam,
                               Result = msgRet.LResult,
                               Hwnd = msgRet.Hwnd
                           };
                    //return OnProcRet(wMsg);
                    return ProcessMessage.Invoke(wMsg);
                case WindowHookType.WH_JOURNALRECORD:
                    break;
                case WindowHookType.WH_JOURNALPLAYBACK:
                    break;
                case WindowHookType.WH_KEYBOARD:
                    break;
                case WindowHookType.WH_GETMESSAGE:
                    var msg = (Message)Marshal.PtrToStructure(lParam, typeof(Message));
                    wMsg = new WindowsMessage
                           {
                               Id = msg.Value,
                               WParam = msg.WParam,
                               LParam = msg.LParam,
                               Result = IntPtr.Zero,
                               Hwnd = msg.Hwnd
                           };
                    //return OnGetMsg(wMsg);
                    return ProcessMessage.Invoke(wMsg);
                case WindowHookType.WH_CALLWNDPROC:
                    break;
                case WindowHookType.WH_CBT:
                    break;
                case WindowHookType.WH_SYSMSGFILTER:
                    break;
                case WindowHookType.WH_MOUSE:
                    break;
                case WindowHookType.WH_HARDWARE:
                    break;
                case WindowHookType.WH_DEBUG:
                    break;
                case WindowHookType.WH_SHELL:
                    break;
                case WindowHookType.WH_FOREGROUNDIDLE:
                    break;
                case WindowHookType.WH_KEYBOARD_LL:
                    break;
                case WindowHookType.WH_MOUSE_LL:
                    break;
            }

            return CallNextHookEx(m_hookHandle.Pointer, nCode, wParam, lParam);
        }
        #endregion
    }
}