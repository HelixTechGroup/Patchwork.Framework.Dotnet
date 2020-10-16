#region Usings
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop;
using Patchwork.Framework.Platform.Interop.Kernel32;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Threading;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using FileAttributes = Patchwork.Framework.Platform.Interop.Kernel32.FileAttributes;
#endregion

namespace Patchwork.Framework.Platform
{
    public sealed class WinApplication : NApplication, IWindowsProcess
    {
        #region Members
        private static SafeFileHandle m_stdIn;
        private static SafeFileHandle m_stdOut;
        private readonly string m_windowClass;
        private WindowProc m_wndProc;
        #endregion

        #region Properties
        /// <inheritdoc />
        public WindowProc Process
        {
            get { return m_wndProc; }
        }
        #endregion

        public WinApplication()
        {
            m_windowClass = "PatchworkMessageWindow-" + Guid.NewGuid();
        }

        #region Methods
        /// <inheritdoc />
        public override bool OpenConsole()
        {
            var handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                ShowWindow(handle, ShowWindowCommands.SW_SHOW);
                return true;
            }

            if (AttachConsole(unchecked((uint)-1)))
                return true;

            if (!AllocConsole())
                return false;

            m_stdOut = new SafeFileHandle(CreateFile("CONOUT$",
                                                     (uint)(DesiredAccess.GenericRead | DesiredAccess.GenericWrite),
                                                     FileShareMode.FILE_SHARE_READ | FileShareMode.FILE_SHARE_WRITE,
                                                     IntPtr.Zero,
                                                     FileCreationDisposition.OPEN_ALWAYS,
                                                     FileAttributes.FILE_ATTRIBUTE_NORMAL,
                                                     IntPtr.Zero),
                                          true);

            if (m_stdOut.IsInvalid)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            m_stdIn = new SafeFileHandle(CreateFile("CONIN$",
                                                    (uint)(DesiredAccess.GenericRead | DesiredAccess.GenericWrite),
                                                    FileShareMode.FILE_SHARE_READ | FileShareMode.FILE_SHARE_WRITE,
                                                    IntPtr.Zero,
                                                    FileCreationDisposition.OPEN_ALWAYS,
                                                    FileAttributes.FILE_ATTRIBUTE_NORMAL,
                                                    IntPtr.Zero),
                                         true);

            if (m_stdIn.IsInvalid)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!SetStdHandle((uint)StdHandle.STD_OUTPUT_HANDLE, m_stdOut.DangerousGetHandle()))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!SetStdHandle((uint)StdHandle.STD_ERROR_HANDLE, m_stdOut.DangerousGetHandle()))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!SetStdHandle((uint)StdHandle.STD_INPUT_HANDLE, m_stdIn.DangerousGetHandle()))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true});
            Console.SetError(new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true});
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            return true;
        }

        /// <inheritdoc />
        public override void CloseConsole()
        {
            m_stdOut.Close();
            m_stdIn.Close();
            if (!FreeConsole())
                throw new Win32Exception(Marshal.GetLastWin32Error());

            m_stdOut.Dispose();
            m_stdIn.Dispose();
        }

        protected override void DisposeManagedResources() { }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            base.DisposeUnmanagedResources();
            UnregisterClass(m_windowClass, GetModuleHandle(null));
        }

        protected override void InitializeResources()
        {
            CreateMessageWindow();
            base.InitializeResources();
        }

        //protected override INWindow PlatformCreateWindow()
        //{
        //    var def = new NWindowDefinition
        //              {
        //                  AcceptsInput = true,
        //                  ActivationPolicy = NWindowActivationPolicy.FirstShown,
        //                  DesiredSize = new Size(800, 600),
        //                  IsRegularWindow = true,
        //                  Title = "test",
        //                  SupportedDecorations = NWindowDecorations.All,
        //                  Type = NWindowType.Normal,
        //                  IsMainApplicationWindow = true
        //              };
        //    var win = new WindowsWindow(this, def);
        //    win.Create();
        //    return win;
        //}

        protected override void PlatformPumpMessages(CancellationToken cancellationToken)
        {
            do
            {
                if (!HasMessages(out var msg))
                    break;

                if (msg.Value == WindowsMessageIds.QUIT)
                {
                    Core.MessagePump.Push(new PlatformMessage(MessageIds.Quit));
                    break;
                }

                ProcessMessage(ref msg);
            } while (!cancellationToken.IsCancellationRequested);
        }

        private IntPtr WindowProc(IntPtr hWnd, WindowsMessageIds msg, IntPtr wParam, IntPtr lParam)
        {
            var platform = Core.Dispatcher as WinThreadDispatcher;
            switch (msg)
            {
                case WindowsMessageIds.DISPATCH_WORK_ITEM:
                    if (wParam.ToInt64() == Constants.SignalW &&
                        lParam.ToInt64() == Constants.SignalL)
                        platform?.Dispatch();
                    break;
                case WindowsMessageIds.CLOSE:
                    DestroyWindow(m_handle.Pointer);
                    break;
                case WindowsMessageIds.DESTROY:
                case WindowsMessageIds.QUIT:
                    Core.MessagePump.Push(new PlatformMessage(MessageIds.Quit));
                    PostQuitMessage(0);
                    break; 
            }

            return DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private void CreateMessageWindow()
        {
            // Ensure that the delegate doesn't get garbage collected by storing it as a field.
            m_wndProc = WindowProc;
            var hInstance = GetModuleHandle(null);

            var wndClassEx = new WindowClassEx
            {
                Size = (uint)Marshal.SizeOf<WindowClassEx>(),
                WindowProc = m_wndProc,
                InstanceHandle = hInstance,
                ClassName = m_windowClass
            };

            var atom = RegisterClassEx(ref wndClassEx);
            if (atom == 0)
            {
                var error = GetLastError();
                throw new Win32Exception((int)error);
            }

            var hwnd = CreateWindowEx(0,
                                      m_windowClass,
                                      null,
                                      WindowStyles.WS_OVERLAPPED,
                                      Constants.CW_USEDEFAULT,
                                      Constants.CW_USEDEFAULT,
                                      Constants.CW_USEDEFAULT,
                                      Constants.CW_USEDEFAULT,
                                      Constants.HWND_MESSAGE,
                                      IntPtr.Zero,
                                      hInstance,
                                      IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
            {
                var error = GetLastError();
                UnregisterClass(m_windowClass, hInstance);
                throw new Win32Exception((int)error);
            }

            m_handle = new NHandle(hwnd, "");
        }

        private bool HasMessages(out Message msg)
        {
            return PeekMessage(out msg, IntPtr.Zero, 0, 0, PeekMessageFlags.PM_REMOVE);
        }

        private void ProcessMessage(ref Message msg)
        {
            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
        }
        #endregion
    }
}