#region Usings
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop;
using Patchwork.Framework.Platform.Interop.User32;
using Shin.Framework.Collections.Concurrent;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
#endregion

namespace Patchwork.Framework.Platform
{
    public class WindowsDesktopWindow : NativeDesktopWindow, IWindowsProcess
    {
        private WindowProc m_wndProc;
        private string m_windowClass;
        public WindowsDesktopWindow(NativeDesktopWindowDefinition definition) : this(definition, null) { }

        public WindowsDesktopWindow(NativeDesktopWindowDefinition definition, INativeObject parent) : base(definition, parent)
        {
            m_windowClass = "ShieldWindow-" + Guid.NewGuid();
            m_childWindows = new ConcurrentList<INativeDesktopWindow>();
        }

        #region Methods
        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_renderer.Initialize();
        }

        protected virtual IntPtr OnMessage(WindowsMessage message)
        {
            switch (message.Id)
            {
                case WindowsMessageIds.ACTIVATEAPP:
                    if (message.WParam != IntPtr.Zero)
                        PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Activating, this));
                    else
                        PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Deactivating, this));
                    break;
                case WindowsMessageIds.KILLFOCUS:
                    PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Unfocused, this));
                    break;
                case WindowsMessageIds.SETFOCUS:
                    PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Focused, this));
                    break;
                case WindowsMessageIds.SHOWWINDOW:
                    if (message.WParam != IntPtr.Zero)
                        PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Shown, this));
                    else
                        PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Hidden, this));
                    break;
                case WindowsMessageIds.SIZING:
                    PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Resizing, this));
                    break;
                case WindowsMessageIds.MOVING:
                    PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Moving, this));
                    break;
                case WindowsMessageIds.WINDOWPOSCHANGING:
                    //PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Resized, this));
                    break;
                case WindowsMessageIds.WINDOWPOSCHANGED:
                    //PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Resizing, this));
                    break;
                case WindowsMessageIds.CLOSE:
                    //DestroyWindow(m_handle.Pointer);
                    PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Closed, this));
                    break;
                case WindowsMessageIds.CREATE:
                    PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Created, this));
                    break;
                case WindowsMessageIds.DESTROY:
                    if (m_isMainApplicationWindow)
                        PostMessage(m_parent.Handle.Pointer, WindowsMessageIds.CLOSE, IntPtr.Zero, IntPtr.Zero);
                    PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Destroyed, this));
                    break;
                case WindowsMessageIds.QUIT:
                    break;
                case WindowsMessageIds.PAINT:
                case WindowsMessageIds.NULL:
                case WindowsMessageIds.MOVE:
                case WindowsMessageIds.SIZE:
                case WindowsMessageIds.ENABLE:
                case WindowsMessageIds.SETREDRAW:
                case WindowsMessageIds.SETTEXT:
                case WindowsMessageIds.GETTEXT:
                case WindowsMessageIds.GETTEXTLENGTH:
                case WindowsMessageIds.QUERYENDSESSION:
                case WindowsMessageIds.QUERYOPEN:
                case WindowsMessageIds.ENDSESSION:
                case WindowsMessageIds.ERASEBKGND:
                case WindowsMessageIds.SYSCOLORCHANGE:
                case WindowsMessageIds.WININICHANGE:
                case WindowsMessageIds.DEVMODECHANGE:
                case WindowsMessageIds.FONTCHANGE:
                case WindowsMessageIds.CANCELMODE:
                case WindowsMessageIds.SETCURSOR:
                case WindowsMessageIds.CHILDACTIVATE:
                case WindowsMessageIds.QUEUESYNC:
                case WindowsMessageIds.GETMINMAXINFO:
                case WindowsMessageIds.PAINTICON:
                case WindowsMessageIds.ICONERASEBKGND:
                case WindowsMessageIds.NEXTDLGCTL:
                case WindowsMessageIds.SPOOLERSTATUS:
                case WindowsMessageIds.DRAWITEM:
                case WindowsMessageIds.MEASUREITEM:
                case WindowsMessageIds.DELETEITEM:
                case WindowsMessageIds.VKEYTOITEM:
                case WindowsMessageIds.CHARTOITEM:
                case WindowsMessageIds.SETFONT:
                case WindowsMessageIds.GETFONT:
                case WindowsMessageIds.SETHOTKEY:
                case WindowsMessageIds.GETHOTKEY:
                case WindowsMessageIds.QUERYDRAGICON:
                case WindowsMessageIds.COMPAREITEM:
                case WindowsMessageIds.GETOBJECT:
                case WindowsMessageIds.COMPACTING:
                case WindowsMessageIds.COMMNOTIFY:                
                case WindowsMessageIds.POWER:
                case WindowsMessageIds.COPYDATA:
                case WindowsMessageIds.CANCELJOURNAL:
                case WindowsMessageIds.NOTIFY:
                case WindowsMessageIds.INPUTLANGCHANGEREQUEST:
                case WindowsMessageIds.INPUTLANGCHANGE:
                case WindowsMessageIds.TCARD:
                case WindowsMessageIds.HELP:
                case WindowsMessageIds.USERCHANGED:
                case WindowsMessageIds.NOTIFYFORMAT:
                case WindowsMessageIds.CONTEXTMENU:
                case WindowsMessageIds.STYLECHANGING:
                case WindowsMessageIds.STYLECHANGED:
                case WindowsMessageIds.DISPLAYCHANGE:
                case WindowsMessageIds.GETICON:
                case WindowsMessageIds.SETICON:
                case WindowsMessageIds.NCCREATE:
                case WindowsMessageIds.NCCALCSIZE:
                case WindowsMessageIds.NCHITTEST:
                case WindowsMessageIds.NCPAINT:
                case WindowsMessageIds.GETDLGCODE:
                case WindowsMessageIds.SYNCPAINT:
                case WindowsMessageIds.NCLBUTTONDOWN:
                case WindowsMessageIds.NCLBUTTONUP:
                case WindowsMessageIds.NCLBUTTONDBLCLK:
                case WindowsMessageIds.NCRBUTTONDOWN:
                case WindowsMessageIds.NCRBUTTONUP:
                case WindowsMessageIds.NCRBUTTONDBLCLK:
                case WindowsMessageIds.NCMBUTTONDOWN:
                case WindowsMessageIds.NCMBUTTONUP:
                case WindowsMessageIds.NCMBUTTONDBLCLK:
                case WindowsMessageIds.NCXBUTTONDOWN:
                case WindowsMessageIds.NCXBUTTONUP:
                case WindowsMessageIds.NCXBUTTONDBLCLK:
                case WindowsMessageIds.INPUT_DEVICE_CHANGE:
                case WindowsMessageIds.INPUT:
                case WindowsMessageIds.UNICHAR:
                case WindowsMessageIds.IME_STARTCOMPOSITION:
                case WindowsMessageIds.IME_ENDCOMPOSITION:
                case WindowsMessageIds.IME_COMPOSITION:
                case WindowsMessageIds.INITDIALOG:
                case WindowsMessageIds.TIMER:
                case WindowsMessageIds.HSCROLL:
                case WindowsMessageIds.VSCROLL:
                case WindowsMessageIds.INITMENU:
                case WindowsMessageIds.INITMENUPOPUP:
                case WindowsMessageIds.GESTURE:
                case WindowsMessageIds.GESTURENOTIFY:
                case WindowsMessageIds.MENUSELECT:
                case WindowsMessageIds.MENUCHAR:
                case WindowsMessageIds.ENTERIDLE:
                case WindowsMessageIds.MENURBUTTONUP:
                case WindowsMessageIds.MENUDRAG:
                case WindowsMessageIds.MENUGETOBJECT:
                case WindowsMessageIds.UNINITMENUPOPUP:
                case WindowsMessageIds.CHANGEUISTATE:
                case WindowsMessageIds.UPDATEUISTATE:
                case WindowsMessageIds.QUERYUISTATE:
                case WindowsMessageIds.CTLCOLORMSGBOX:
                case WindowsMessageIds.CTLCOLOREDIT:
                case WindowsMessageIds.CTLCOLORLISTBOX:
                case WindowsMessageIds.CTLCOLORBTN:
                case WindowsMessageIds.CTLCOLORDLG:
                case WindowsMessageIds.CTLCOLORSCROLLBAR:
                case WindowsMessageIds.CTLCOLORSTATIC:
                case WindowsMessageIds.PARENTNOTIFY:
                case WindowsMessageIds.ENTERMENULOOP:
                case WindowsMessageIds.EXITMENULOOP:
                case WindowsMessageIds.NEXTMENU:
                case WindowsMessageIds.POWERBROADCAST:
                case WindowsMessageIds.DEVICECHANGE:
                case WindowsMessageIds.MDICREATE:
                case WindowsMessageIds.MDIDESTROY:
                case WindowsMessageIds.MDIACTIVATE:
                case WindowsMessageIds.MDIRESTORE:
                case WindowsMessageIds.MDINEXT:
                case WindowsMessageIds.MDIMAXIMIZE:
                case WindowsMessageIds.MDITILE:
                case WindowsMessageIds.MDICASCADE:
                case WindowsMessageIds.MDIICONARRANGE:
                case WindowsMessageIds.MDIGETACTIVE:
                case WindowsMessageIds.MDISETMENU:
                case WindowsMessageIds.ENTERSIZEMOVE:
                case WindowsMessageIds.EXITSIZEMOVE:
                case WindowsMessageIds.DROPFILES:
                case WindowsMessageIds.MDIREFRESHMENU:
                case WindowsMessageIds.POINTERDEVICECHANGE:
                case WindowsMessageIds.POINTERDEVICEINRANGE:
                case WindowsMessageIds.POINTERDEVICEOUTOFRANGE:
                case WindowsMessageIds.TOUCH:
                case WindowsMessageIds.NCPOINTERUPDATE:
                case WindowsMessageIds.NCPOINTERDOWN:
                case WindowsMessageIds.NCPOINTERUP:
                case WindowsMessageIds.POINTERUPDATE:
                case WindowsMessageIds.POINTERDOWN:
                case WindowsMessageIds.POINTERUP:
                case WindowsMessageIds.POINTERENTER:
                case WindowsMessageIds.POINTERLEAVE:
                case WindowsMessageIds.POINTERACTIVATE:
                case WindowsMessageIds.POINTERCAPTURECHANGED:
                case WindowsMessageIds.TOUCHHITTESTING:
                case WindowsMessageIds.POINTERWHEEL:
                case WindowsMessageIds.POINTERHWHEEL:
                case WindowsMessageIds.IME_SETCONTEXT:
                case WindowsMessageIds.IME_NOTIFY:
                case WindowsMessageIds.IME_CONTROL:
                case WindowsMessageIds.IME_COMPOSITIONFULL:
                case WindowsMessageIds.IME_SELECT:
                case WindowsMessageIds.IME_CHAR:
                case WindowsMessageIds.IME_REQUEST:
                case WindowsMessageIds.IME_KEYDOWN:
                case WindowsMessageIds.IME_KEYUP:
                case WindowsMessageIds.NCMOUSEHOVER:
                case WindowsMessageIds.NCMOUSELEAVE:
                case WindowsMessageIds.WTSSESSION_CHANGE:
                case WindowsMessageIds.TABLET_FIRST:
                case WindowsMessageIds.TABLET_LAST:
                case WindowsMessageIds.DPICHANGED:
                case WindowsMessageIds.CUT:
                case WindowsMessageIds.COPY:
                case WindowsMessageIds.PASTE:
                case WindowsMessageIds.CLEAR:
                case WindowsMessageIds.UNDO:
                case WindowsMessageIds.RENDERFORMAT:
                case WindowsMessageIds.RENDERALLFORMATS:
                case WindowsMessageIds.DESTROYCLIPBOARD:
                case WindowsMessageIds.DRAWCLIPBOARD:
                case WindowsMessageIds.PAINTCLIPBOARD:
                case WindowsMessageIds.VSCROLLCLIPBOARD:
                case WindowsMessageIds.SIZECLIPBOARD:
                case WindowsMessageIds.ASKCBFORMATNAME:
                case WindowsMessageIds.CHANGECBCHAIN:
                case WindowsMessageIds.HSCROLLCLIPBOARD:
                case WindowsMessageIds.QUERYNEWPALETTE:
                case WindowsMessageIds.PALETTEISCHANGING:
                case WindowsMessageIds.PALETTECHANGED:
                case WindowsMessageIds.PRINT:
                case WindowsMessageIds.PRINTCLIENT:
                case WindowsMessageIds.THEMECHANGED:
                case WindowsMessageIds.CLIPBOARDUPDATE:
                case WindowsMessageIds.DWMCOMPOSITIONCHANGED:
                case WindowsMessageIds.DWMNCRENDERINGCHANGED:
                case WindowsMessageIds.DWMCOLORIZATIONCOLORCHANGED:
                case WindowsMessageIds.DWMWINDOWMAXIMIZEDCHANGE:
                case WindowsMessageIds.DWMSENDICONICTHUMBNAIL:
                case WindowsMessageIds.DWMSENDICONICLIVEPREVIEWBITMAP:
                case WindowsMessageIds.GETTITLEBARINFOEX:
                case WindowsMessageIds.HANDHELDFIRST:
                case WindowsMessageIds.HANDHELDLAST:
                case WindowsMessageIds.AFXFIRST:
                case WindowsMessageIds.AFXLAST:
                case WindowsMessageIds.PENWINFIRST:
                case WindowsMessageIds.PENWINLAST:
                case WindowsMessageIds.APP:
                case WindowsMessageIds.USER:

                case WindowsMessageIds.NCDESTROY:
                case WindowsMessageIds.TIMECHANGE:
                case WindowsMessageIds.MOUSELEAVE:
                case WindowsMessageIds.NCACTIVATE:                                
                case WindowsMessageIds.ACTIVATE:                
                case WindowsMessageIds.MOUSEMOVE:
                case WindowsMessageIds.LBUTTONUP:
                case WindowsMessageIds.LBUTTONDOWN:
                case WindowsMessageIds.LBUTTONDBLCLK:
                case WindowsMessageIds.RBUTTONUP:
                case WindowsMessageIds.RBUTTONDOWN:
                case WindowsMessageIds.RBUTTONDBLCLK:
                case WindowsMessageIds.MBUTTONUP:
                case WindowsMessageIds.MBUTTONDOWN:
                case WindowsMessageIds.MBUTTONDBLCLK:
                case WindowsMessageIds.XBUTTONUP:
                case WindowsMessageIds.XBUTTONDOWN:
                case WindowsMessageIds.XBUTTONDBLCLK:
                case WindowsMessageIds.MOUSEACTIVATE:
                case WindowsMessageIds.MOUSEHOVER:
                case WindowsMessageIds.MOUSEWHEEL:
                case WindowsMessageIds.MOUSEHWHEEL:
                case WindowsMessageIds.CHAR:
                case WindowsMessageIds.SYSCHAR:
                case WindowsMessageIds.DEADCHAR:
                case WindowsMessageIds.SYSDEADCHAR:
                case WindowsMessageIds.KEYUP:
                case WindowsMessageIds.KEYDOWN:
                case WindowsMessageIds.SYSKEYUP:
                case WindowsMessageIds.SYSKEYDOWN:
                case WindowsMessageIds.COMMAND:
                case WindowsMessageIds.SYSCOMMAND:
                case WindowsMessageIds.MENUCOMMAND:
                case WindowsMessageIds.APPCOMMAND:                
                case WindowsMessageIds.CAPTURECHANGED:
                case WindowsMessageIds.HOTKEY:
                case WindowsMessageIds.NCMOUSEMOVE:
                default:
                    break;
            }

            return DefWindowProc(message.Hwnd, message.Id, message.WParam, message.LParam);
        }

        /// <inheritdoc />
        protected override Rectangle PlatformGetClientArea()
        {
            GetClientRect(m_handle.Pointer, out var rect);
            return rect;
        }

        /// <inheritdoc />
        protected override Size PlatformGetClientSize()
        {
            GetClientRect(m_handle.Pointer, out var rect);
            return Size.Round(new SizeF(rect.Right, rect.Bottom) / (float)((INativeDesktopWindowRenderer)m_renderer).DpiScaling);
        }

        /// <inheritdoc />
        protected override Size PlatformGetMaxClientSize()
        {
            //var r = m_renderer as INativeDesktopRenderer;
            //return Size.Round((new Size(
            //                            GetSystemMetrics(SystemMetrics.SM_CXMAXTRACK),
            //                            GetSystemMetrics(SystemMetrics.SM_CYMAXTRACK))
            //                 - r.BorderThickness) / (float)r.DpiScaling);
 
            return new Size(
                            GetSystemMetrics(SystemMetrics.SM_CXMAXTRACK),
                            GetSystemMetrics(SystemMetrics.SM_CYMAXTRACK));
        }

        /// <inheritdoc />
        protected override void PlatformSetTitle(string value)
        {
            SetWindowText(m_handle.Pointer, value);
        }

        /// <inheritdoc />
        protected override string PlatformGetTitle()
        {

            var size = GetWindowTextLength(m_handle.Pointer);
            if (size <= 0) 
                return string.Empty;
            var len = size + 1;
            var sb = new StringBuilder(len);
            return GetWindowText(m_handle.Pointer, sb, len) > 0 ? sb.ToString() : string.Empty;

        }

        /// <inheritdoc />
        protected override void PlatformShow()
        {
            var shouldActivate = false;
            if (m_definition.AcceptsInput)
                shouldActivate = m_definition.ActivationPolicy == NativeWindowActivationPolicy.Always || m_isFirstTimeVisible && m_definition.ActivationPolicy == NativeWindowActivationPolicy.FirstShown;

            var showWindowCommand = shouldActivate ? ShowWindowCommands.SW_SHOW : ShowWindowCommands.SW_SHOWNOACTIVATE;
            if (m_isFirstTimeVisible)
            {
                m_isFirstTimeVisible = false;
                switch (m_initialState) {
                    case NativeWindowState.Minimized:
                        showWindowCommand = shouldActivate ? ShowWindowCommands.SW_MINIMIZE : ShowWindowCommands.SW_SHOWMINNOACTIVE;
                        break;
                    case NativeWindowState.Maximized:
                        showWindowCommand = shouldActivate ? ShowWindowCommands.SW_SHOWMAXIMIZED : ShowWindowCommands.SW_MAXIMIZE;
                        break;
                }
            }


            ShowWindow(m_handle.Pointer, showWindowCommand);
        }

        /// <inheritdoc />
        protected override void PlatformHide()
        {
            ShowWindow(m_handle.Pointer, ShowWindowCommands.SW_HIDE);
        }

        /// <inheritdoc />
        protected override void PlatformCreate()
        {
            m_isMainApplicationWindow = m_definition.IsMainApplicationWindow;
            var x = m_definition.DesiredPosition.X;
            var y = m_definition.DesiredPosition.Y;
            WindowStyles style = 0;
            WindowExStyles styleEx = 0;

            if (m_definition.DesiredSize.Width > 0 && m_definition.DesiredSize.Height > 0)
            {
                var screenWidth = GetSystemMetrics(SystemMetrics.SM_CXSCREEN);
                var screenHeight = GetSystemMetrics(SystemMetrics.SM_CYSCREEN);

                // Place the window in the middle of the screen.WS_EX_APPWINDOW
                x = (screenWidth - m_definition.DesiredSize.Width) / 2;
                y = (screenHeight - m_definition.DesiredSize.Height) / 2;
            }

            if (m_definition.SupportedDecorations.HasFlag(NativeWindowDecorations.CloseButton))
                style = WindowStyles.WS_OVERLAPPEDWINDOW;
            else
                style = WindowStyles.WS_POPUP | WindowStyles.WS_BORDER | WindowStyles.WS_CAPTION | WindowStyles.WS_SYSMENU;

            styleEx = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;
            style |= WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;

            int windowWidth;
            int windowHeight;

            if (m_definition.DesiredSize.Width > 0 && m_definition.DesiredSize.Height > 0)
            {
                var rect = new Rectangle(0, 0, m_definition.DesiredSize.Width, m_definition.DesiredSize.Height);

                // Adjust according to window styles
                AdjustWindowRectEx(
                                   ref rect,
                                   style,
                                   false,
                                   styleEx);

                windowWidth = rect.Right - rect.Left;
                windowHeight = rect.Bottom - rect.Top;
            }
            else
                x = y = windowWidth = windowHeight = Constants.CW_USEDEFAULT;

            // Ensure that the delegate doesn't get garbage collected by storing it as a field.
            m_wndProc = WindowProc;
            var hInstance = GetModuleHandle(null);

            var wndClassEx = new WindowClassEx
                             {
                                 Size = (uint)Marshal.SizeOf<WindowClassEx>(),
                                 Styles = WindowClassStyles.CS_OWNDC | WindowClassStyles.CS_HREDRAW |
                                          WindowClassStyles.CS_VREDRAW, // Unique DC helps with performance when using Gpu based rendering
                                 WindowProc = m_wndProc,
                                 InstanceHandle = hInstance,
                                 ClassName = m_windowClass,
                                 BackgroundBrushHandle = (IntPtr)SystemColor.COLOR_WINDOW
                             };

            var atom = RegisterClassEx(ref wndClassEx);
            if (atom == 0)
                throw new Win32Exception();

            var hwnd = CreateWindowEx(
                                      styleEx,
                                      m_windowClass,
                                      m_definition.Title,
                                      style,
                                      x,
                                      y,
                                      windowWidth,
                                      windowHeight,
                                      m_parent?.Handle.Pointer ?? IntPtr.Zero,
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
            /// = windowWidth;
            //m_height = windowHeight;
            m_renderer = new WindowsDesktopWindowRenderer(this, null);            
            //m_input = new NativeInput(this);
        }

        /// <inheritdoc />
        protected override void PlatformDestroy()
        {
            DestroyWindow(m_handle.Pointer);
        }

        /// <inheritdoc />
        protected override void PlatformDeactivate()
        {
            return;
        }

        /// <inheritdoc />
        protected override bool PlatformCheckActive()
        {
            return (GetActiveWindow() == m_handle.Pointer);
        }

        /// <inheritdoc />
        public override bool IsPointInWindow(Point point)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Point PointToClient(Point point)
        {
            var p = point;
            ScreenToClient(m_handle.Pointer, ref p);
            return new Point(p.X, p.Y);
        }

        /// <inheritdoc />
        public override Point PointToScreen(Point point)
        {
            var p = point;
            ClientToScreen(m_handle.Pointer, ref p);
            return new Point(p.X, p.Y);
        }

        /// <inheritdoc />
        protected override void PlatformActivate()
        {
            SetActiveWindow(m_handle.Pointer);
        }

        /// <inheritdoc />
        protected override bool PlatformCheckVisible()
        {
            return IsWindowVisible(m_handle.Pointer);
        }

        private IntPtr WindowProc(IntPtr hwnd, WindowsMessageIds msg, IntPtr wParam, IntPtr lParam)
        {
            var wMsg = new WindowsMessage
                       {
                           Id = msg,
                           WParam = wParam,
                           LParam = lParam,
                           Result = IntPtr.Zero,
                           Hwnd = hwnd
                       };

            return OnMessage(wMsg);
        }
        #endregion

        /// <inheritdoc />
        public WindowProc Process
        {
            get { return m_wndProc; }
        }

        /// <inheritdoc />
        protected override Point PlatformGetPosition()
        {
            GetWindowRect(m_handle.Pointer, out var rect);
            return new Point(rect.Left, rect.Top);
        }

        /// <inheritdoc />
        protected override Size PlatformGetWindowSize()
        {
            GetWindowRect(m_handle.Pointer, out var rect);
            return Size.Round(new SizeF(rect.Right, rect.Bottom) / (float)((INativeDesktopWindowRenderer)m_renderer).DpiScaling);
        }

        /// <inheritdoc />
        protected override void PlatformSetPosition(Point position)
        {
            var x = position.X;
            var y = position.Y;
            if (((INativeDesktopWindowRenderer)m_renderer).SupportedDecorations.HasFlag(NativeWindowDecorations.Border))
            {
                var windowStyle = (WindowStyles)GetWindowLongPtr(m_handle.Pointer, WindowLongFlags.GWL_STYLE);
                var windowExStyle = (WindowExStyles)GetWindowLongPtr(m_handle.Pointer, WindowLongFlags.GWL_EXSTYLE);

                // This adjusts a zero rect to give us the size of the border
                var borderRect = Rectangle.Empty;
                AdjustWindowRectEx(ref borderRect, windowStyle, false, windowExStyle);

                // Border rect size is negative
                x += borderRect.Left;
                y += borderRect.Top;
            }

            SetWindowPos(
                         m_handle.Pointer,
                         HwndZOrder.HWND_TOP,
                         x,
                         y,
                         0,
                         0,
                         WindowPositionFlags.SWP_NOSIZE | WindowPositionFlags.SWP_NOACTIVATE | WindowPositionFlags.SWP_NOZORDER);
        }

        /// <inheritdoc />
        protected override void PlatformSetWindowSize(Size size)
        {
            
        }

        /// <inheritdoc />
        protected override void PlatformFocus()
        {
            SetFocus(m_handle.Pointer);
        }

        /// <inheritdoc />
        protected override void PlatformUnfocus()
        {
            SetFocus(IntPtr.Zero);
        }

        /// <inheritdoc />
        protected override bool PlatformCheckFocus()
        {
            return (GetFocus() == m_handle.Pointer);
        }

        /// <inheritdoc />
        protected override void PlatformRestore()
        {
            ShowWindow(m_handle.Pointer, ShowWindowCommands.SW_RESTORE);
        }

        /// <inheritdoc />
        protected override Rectangle PlatformGetRestoreArea()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformMinimize()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformMaximize()
        {
            throw new NotImplementedException();
        }

        private void MaximizeWithoutCoveringTaskbar()
        {
            var monitor = MonitorFromWindow(m_handle.Pointer, MonitorFlag.MONITOR_DEFAULTTONEAREST);

            if (monitor == IntPtr.Zero) 
                return;
            var monitorInfo = new MonitorInfo() { Size = (uint)Marshal.SizeOf<MonitorInfo>() };
            GetMonitorInfo(monitor, ref monitorInfo);

            if (!GetMonitorInfo(monitor, ref monitorInfo)) 
                return;

            var x = monitorInfo.WorkRect.Left;
            var y = monitorInfo.WorkRect.Top;
            var cx = Math.Abs(monitorInfo.WorkRect.Right - x);
            var cy = Math.Abs(monitorInfo.WorkRect.Bottom - y);

            SetWindowPos(m_handle.Pointer, HwndZOrder.HWND_NOTOPMOST, x, y, cx, cy, WindowPositionFlags.SWP_SHOWWINDOW);
        }

        /// <inheritdoc />
        protected override NativeWindowState PlatformGetState()
        {
            GetWindowPlacement(m_handle.Pointer, out var placement);
            switch (placement.ShowCmd)
            {
                case ShowWindowCommands.SW_MAXIMIZE:
                    return NativeWindowState.Maximized;
                case ShowWindowCommands.SW_MINIMIZE:
                    return NativeWindowState.Minimized;
                default:
                    return NativeWindowState.Normal;
            }
        }

        /// <inheritdoc />
        protected override void PlatformEnable()
        {
            EnableWindow(m_handle.Pointer, true);
        }

        /// <inheritdoc />
        protected override void PlatformDisable()
        {
            EnableWindow(m_handle.Pointer, false);
        }

        protected override bool PlatformCheckEnable()
        {
            return IsWindowEnabled(m_handle.Pointer);
        }

        /// <inheritdoc />
        public override void BringToFront(bool force)
        {
            if (m_definition.IsRegularWindow)
            {
                if (IsIconic(m_handle.Pointer))
                    Restore();
                else
                    Activate();
            }
            else
            {
                var hWndInsertAfter = HwndZOrder.HWND_TOP;
                // By default we activate the window or it isn't actually brought to the front 
                var flags = WindowPositionFlags.SWP_NOMOVE | WindowPositionFlags.SWP_NOSIZE | WindowPositionFlags.SWP_NOOWNERZORDER;

                if (!force)
                    flags |= WindowPositionFlags.SWP_NOACTIVATE;

                if (m_definition.IsTopmostWindow)
                    hWndInsertAfter = HwndZOrder.HWND_TOPMOST;


                SetWindowPos(m_handle.Pointer, hWndInsertAfter, 0, 0, 0, 0, flags);
            }
        }

        /// <inheritdoc />
        public override void ForceToFront()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void CenterToScreen()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformDrawAttention()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override INativeDesktopWindow PlatformCreateChildWindow(NativeDesktopWindowDefinition definition)
        {
            return new WindowsDesktopWindow(definition);
        }

        /// <inheritdoc />
        protected override INativePopupWindow PlatformCreatePopupWindow(NativeDesktopWindowDefinition definition)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {            
            base.DisposeUnmanagedResources();
            UnregisterClass(m_windowClass, m_parent.Handle.Pointer);
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            m_renderer.Dispose();
        }
    }
}