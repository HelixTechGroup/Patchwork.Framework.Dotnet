#region Usings
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop;
using Patchwork.Framework.Platform.Interop.Kernel32;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using FileAttributes = Patchwork.Framework.Platform.Interop.Kernel32.FileAttributes;
#endregion

namespace Patchwork.Framework.Platform
{
    public class WindowsDesktopApplication : Initializable, INativeApplication, IWindowsProcess
    {
        #region Members
        private WindowProc m_wndProc;
        private readonly string m_windowClass;
        private INativeHandle m_handle;
        private readonly IList<INativeWindow> m_windows;
        private readonly Thread m_thread;
        private INativeWindow m_appWindow;
        private static SafeFileHandle m_stdIn;
        private static SafeFileHandle m_stdOut;
        #endregion

        #region Properties
        public INativeHandle Handle
        {
            get { return m_handle; }
        }

        /// <inheritdoc />
        public WindowProc Process
        {
            get { return m_wndProc; }
        }

        public Thread Thread
        {
            get { return m_thread; }
        }

        /// <inheritdoc />
        public event EventHandler<INativeWindow> WindowCreated;

        /// <inheritdoc />
        public event EventHandler<INativeWindow> WindowDestroyed;

        /// <inheritdoc />
        public IEnumerable<INativeWindow> Windows
        {
            get { return m_windows; }
        }

        /// <inheritdoc />
        public INativeWindow CreateWindow()
        {
            Throw.If(!m_isInitialized).InvalidOperationException();
            return CreateNativeWindow();
        }
        #endregion

        public WindowsDesktopApplication()
        {
            m_windows = new ConcurrentList<INativeWindow>();
            m_windowClass = "ShieldMessageWindow-" + Guid.NewGuid();
            m_thread = Thread.CurrentThread;
            WireUpDesktopWindowEvents();
        }

        #region Methods
        protected override void InitializeResources()
        {
            CreateMessageWindow();
            base.InitializeResources();
        }

        private IntPtr WindowProc(IntPtr hWnd, WindowsMessageIds msg, IntPtr wParam, IntPtr lParam)
        {
            var platform = PlatformManager.Dispatcher as WindowsDesktopThreadDispatcher;
            switch (msg)
            {
                case WindowsMessageIds.DISPATCH_WORK_ITEM:
                    if (wParam.ToInt64() == Constants.SignalW &&
                        lParam.ToInt64() == Constants.SignalL) 
                        platform?.Signal();
                    break;
                case WindowsMessageIds.CLOSE:
                    DestroyWindow(m_handle.Pointer);
                    break;
                case WindowsMessageIds.DESTROY:
                    PostQuitMessage(0);
                    break;
                case WindowsMessageIds.QUIT:
                    break;
            }

            return DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private void CreateMessageWindow()
        {
            // Ensure that the delegate doesn't get garbage collected by storing it as a field.
            m_wndProc = WindowProc;
            var hInstance = GetModuleHandle(null);

            var wndClassEx = new WindowClassEx()
                                    {
                                        Size = (uint)Marshal.SizeOf<WindowClassEx>(),
                                        WindowProc = m_wndProc,
                                        InstanceHandle = hInstance,
                                        ClassName = m_windowClass
                                    };

            var atom = RegisterClassEx(ref wndClassEx);
            if (atom == 0) throw new Win32Exception();

            var hwnd = CreateWindowEx(0, m_windowClass, null, 
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

            m_handle = new NativeHandle(hwnd, "");
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            base.DisposeUnmanagedResources();
            UnregisterClass(m_windowClass, GetModuleHandle(null));
        }

        protected override void DisposeManagedResources()
        {
        }

        private INativeWindow CreateNativeWindow()
        {
            var def = new NativeDesktopWindowDefinition()
            {
                AcceptsInput = true,
                ActivationPolicy = NativeWindowActivationPolicy.FirstShown,
                DesiredSize = new Size(800, 600),
                IsRegularWindow = true,
                Title = "test",
                SupportedDecorations = NativeWindowDecorations.All,
                Type = NativeWindowType.Normal,
                IsMainApplicationWindow = true
            };
            var win = new WindowsDesktopWindow(def, this);
            win.Create();
            WindowCreated.Raise(this, win);

            return win;
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

        public void PumpMessages(CancellationToken cancellationToken)
        {
            do
            {
                if (!HasMessages(out var msg))
                    break;

                if (msg.Value == WindowsMessageIds.QUIT)
                {
                    PlatformManager.MessagePump.Push(new PlatformMessage(MessageIds.Quit));
                    break;
                }

                ProcessMessage(ref msg);
            } while (!cancellationToken.IsCancellationRequested);
        }

        protected virtual void OnWindowCreated(object sender, INativeWindow window)
        {
            m_windows.Add(window);

            if (window.IsMainApplicationWindow && m_appWindow is null)
                m_appWindow = window;
        }

        protected virtual void OnWindowDestroyed(object sender, INativeWindow window)
        {
            m_windows.Remove(window);
        }

        private void WireUpDesktopWindowEvents()
        {
            WindowCreated += OnWindowCreated;
            WindowDestroyed += OnWindowDestroyed;
        }

        public bool CreateConsole()
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
                                          IntPtr.Zero), true);

            if (m_stdOut.IsInvalid)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            m_stdIn = new SafeFileHandle(CreateFile("CONIN$",
                                          (uint)(DesiredAccess.GenericRead | DesiredAccess.GenericWrite),
                                          FileShareMode.FILE_SHARE_READ | FileShareMode.FILE_SHARE_WRITE,
                                          IntPtr.Zero,
                                          FileCreationDisposition.OPEN_ALWAYS,
                                          FileAttributes.FILE_ATTRIBUTE_NORMAL,
                                          IntPtr.Zero),true);

            if (m_stdIn.IsInvalid)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!SetStdHandle((uint)StdHandle.STD_OUTPUT_HANDLE, m_stdOut.DangerousGetHandle()))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!SetStdHandle((uint)StdHandle.STD_ERROR_HANDLE, m_stdOut.DangerousGetHandle()))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!SetStdHandle((uint)StdHandle.STD_INPUT_HANDLE, m_stdIn.DangerousGetHandle()))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            //var defaultStdout = new IntPtr(7);
            //var currentStdout = GetStdHandle((uint)StdHandle.STD_OUTPUT_HANDLE);

            //if (currentStdout != defaultStdout)
            //    SetStdHandle((uint)StdHandle.STD_OUTPUT_HANDLE, defaultStdout);

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true});
            Console.SetError(new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true});
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            return true;
        }

        public void CloseConsole()
        {
            m_stdOut.Close();
            m_stdIn.Close();
            if (!FreeConsole())
                throw new Win32Exception(Marshal.GetLastWin32Error());

            m_stdOut.Dispose();
            m_stdIn.Dispose();
        }

        [Flags]
        enum DesiredAccess : uint
        {
            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000
        }
        #endregion
    }
}