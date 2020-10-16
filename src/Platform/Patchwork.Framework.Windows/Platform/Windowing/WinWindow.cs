#region Usings
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Patchwork.Framework.Platform.Interop;
using Patchwork.Framework.Platform.Interop.User32;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public sealed partial class WinWindow : NWindow, IWindowsProcess, IEquatable<WinWindow>
    {
        #region Members
        private readonly string m_windowClass;
        #endregion

        public WinWindow(INObject parent, NWindowDefinition definition) : base(parent, definition)
        {
            // Ensure that the delegate doesn't get garbage collected by storing it as a field.
            m_wndProc = WindowProc;
            m_windowClass = "ShieldWindow-" + Guid.NewGuid();
        }

        #region Methods
        /// <inheritdoc />
        public override void BringToFront(bool force)
        {
            if (m_cache.Definition.IsRegularWindow)
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

                if (m_cache.Definition.IsTopmostWindow)
                    hWndInsertAfter = HwndZOrder.HWND_TOPMOST;


                SetWindowPos(m_handle.Pointer, hWndInsertAfter, 0, 0, 0, 0, flags);
            }
        }

        /// <inheritdoc />
        public override void ForceToFront() { }

        /// <inheritdoc />
        public override void CenterToScreen() { }

        /// <inheritdoc />
        public bool Equals(WinWindow other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(m_wndProc, other.m_wndProc) &&
                   string.Equals(m_windowClass, other.m_windowClass, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(WinWindow)) return false;
            return Equals((WinWindow)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(m_wndProc);
            hashCode.Add(m_windowClass, StringComparer.OrdinalIgnoreCase);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(WinWindow left, WinWindow right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WinWindow left, WinWindow right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            //m_renderer.Dispose();
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            base.DisposeUnmanagedResources();
            UnregisterClass(m_windowClass, m_parent.Handle.Pointer);
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            //m_renderer.Initialize();
        }

        /// <inheritdoc />
        protected override void PlatformActivate()
        {
            SetActiveWindow(m_handle.Pointer);
        }

        /// <inheritdoc />
        protected override bool PlatformCheckActive()
        {
            return GetActiveWindow() == m_handle.Pointer;
        }

        protected override bool PlatformCheckEnable()
        {
            return IsWindowEnabled(m_handle.Pointer);
        }

        /// <inheritdoc />
        protected override bool PlatformCheckFocus()
        {
            return GetFocus() == m_handle.Pointer;
        }

        /// <inheritdoc />
        protected override bool PlatformCheckVisible()
        {
            return IsWindowVisible(m_handle.Pointer);
        }

        /// <inheritdoc />
        protected override void PlatformCreate()
        {
            m_isMainApplicationWindow = m_cache.Definition.IsMainApplicationWindow;
            var x = m_cache.Position.X;
            var y = m_cache.Position.Y;
            WindowStyles style = 0;
            WindowExStyles styleEx = 0;

            if (m_cache.Size.Width > 0 && m_cache.Size.Height > 0)
            {
                var screenWidth = GetSystemMetrics(SystemMetrics.SM_CXSCREEN);
                var screenHeight = GetSystemMetrics(SystemMetrics.SM_CYSCREEN);

                // Place the window in the middle of the screen.WS_EX_APPWINDOW
                x = (screenWidth - m_cache.Size.Width) / 2;
                y = (screenHeight - m_cache.Size.Height) / 2;
            }

            if (m_cache.Definition.SupportedDecorations.HasFlag(NWindowDecorations.CloseButton))
                style = WindowStyles.WS_OVERLAPPEDWINDOW;
            else
                style = WindowStyles.WS_POPUP | WindowStyles.WS_BORDER | WindowStyles.WS_CAPTION | WindowStyles.WS_SYSMENU;

            styleEx = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;
            style |= WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;

            int windowWidth;
            int windowHeight;

            if (m_cache.Size.Width > 0 && m_cache.Size.Height > 0)
            {
                var rect = new Rectangle(0, 0, m_cache.Size.Width, m_cache.Size.Height);

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

            var hInstance = GetModuleHandle(null);

            var wndClassEx = new WindowClassEx
            {
                Size = (uint)Marshal.SizeOf<WindowClassEx>(),
                Styles = WindowClassStyles.CS_OWNDC | WindowClassStyles.CS_HREDRAW |
                                          WindowClassStyles.CS_VREDRAW, // Unique DC helps with performance when using Gpu based rendering
                WindowProc = m_wndProc,
                InstanceHandle = hInstance,
                ClassName = m_windowClass,
                BackgroundBrushHandle = IntPtr.Zero //(IntPtr)SystemColor.COLOR_WINDOW
            };

            var atom = RegisterClassEx(ref wndClassEx);
            if (atom == 0)
                throw new Win32Exception();

            var hwnd = CreateWindowEx(
                                      styleEx,
                                      m_windowClass,
                                      m_cache.Title,
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

            m_handle = new NHandle(hwnd);
            /// = windowWidth;
            //m_height = windowHeight;
            //m_renderer = new WindowsDesktopWindowRenderer(this, null);            
            //m_input = new NInput(this);
        }

        /// <inheritdoc />
        protected override INWindow PlatformCreateChildWindow(NWindowDefinition definition)
        {
            return new WinWindow(this, definition);
        }

        /// <inheritdoc />
        protected override INPopupWindow PlatformCreatePopupWindow(NWindowDefinition definition)
        {
            return null;
        }

        /// <inheritdoc />
        protected override void PlatformDeactivate() { }

        /// <inheritdoc />
        protected override void PlatformDestroy()
        {
            DestroyWindow(m_handle.Pointer);
        }

        /// <inheritdoc />
        protected override void PlatformDisable()
        {
            EnableWindow(m_handle.Pointer, false);
        }

        /// <inheritdoc />
        protected override void PlatformDrawAttention() { }

        /// <inheritdoc />
        protected override void PlatformEnable()
        {
            EnableWindow(m_handle.Pointer, true);
        }

        /// <inheritdoc />
        protected override void PlatformFocus()
        {
            SetFocus(m_handle.Pointer);
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
            return new Size(rect.Right, rect.Bottom);
            //return Size.Round(new SizeF(rect.Right, rect.Bottom) / (float)((INDesktopWindowRenderer)m_renderer).DpiScaling);
        }

        /// <inheritdoc />
        protected override Size PlatformGetMaxClientSize()
        {
            //var r = m_renderer as INDesktopRenderer;
            //return Size.Round((new Size(
            //                            GetSystemMetrics(SystemMetrics.SM_CXMAXTRACK),
            //                            GetSystemMetrics(SystemMetrics.SM_CYMAXTRACK))
            //                 - r.BorderThickness) / (float)r.DpiScaling);

            return new Size(
                            GetSystemMetrics(SystemMetrics.SM_CXMAXTRACK),
                            GetSystemMetrics(SystemMetrics.SM_CYMAXTRACK));
        }

        /// <inheritdoc />
        protected override NWindowMode PlatformGetMode()
        {
            return NWindowMode.Windowed;
        }

        /// <inheritdoc />
        protected override Point PlatformGetPosition()
        {
            GetWindowRect(m_handle.Pointer, out var rect);
            return new Point(rect.Left, rect.Top);
        }

        /// <inheritdoc />
        protected override Rectangle PlatformGetRestoreArea()
        {
            return Rectangle.Empty;
        }

        /// <inheritdoc />
        protected override NWindowState PlatformGetState()
        {
            GetWindowPlacement(m_handle.Pointer, out var placement);
            switch (placement.ShowCmd)
            {
                case ShowWindowCommands.SW_MAXIMIZE:
                    return NWindowState.Maximized;
                case ShowWindowCommands.SW_MINIMIZE:
                    return NWindowState.Minimized;
                default:
                    return NWindowState.Normal;
            }
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
        protected override Size PlatformGetWindowSize()
        {
            GetWindowRect(m_handle.Pointer, out var rect);
            return new Size(rect.Right, rect.Bottom);
            //return Size.Round(new SizeF(rect.Right, rect.Bottom) / (float)((INDesktopWindowRenderer)m_renderer).DpiScaling);
        }

        /// <inheritdoc />
        protected override void PlatformHide()
        {
            ShowWindow(m_handle.Pointer, ShowWindowCommands.SW_HIDE);
        }

        /// <inheritdoc />
        protected override bool PlatformIsPointInWindow(Point point)
        {
            return true;
        }

        /// <inheritdoc />
        protected override void PlatformMaximize() { }

        /// <inheritdoc />
        protected override void PlatformMinimize() { }

        /// <inheritdoc />
        protected override Point PlatformPointToClient(Point point)
        {
            var p = point;
            ScreenToClient(m_handle.Pointer, ref p);
            return new Point(p.X, p.Y);
        }

        /// <inheritdoc />
        protected override Point PlatformPointToScreen(Point point)
        {
            var p = point;
            ClientToScreen(m_handle.Pointer, ref p);
            return new Point(p.X, p.Y);
        }

        /// <inheritdoc />
        protected override void PlatformRestore()
        {
            ShowWindow(m_handle.Pointer, ShowWindowCommands.SW_RESTORE);
        }

        /// <inheritdoc />
        protected override void PlatformSetPosition(Point position)
        {
            var x = position.X;
            var y = position.Y;
            //m_renderer.SupportedDecorations.HasFlag(NWindowDecorations.Border)
            if (true)
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
        protected override void PlatformSetTitle(string value)
        {
            SetWindowText(m_handle.Pointer, value);
        }

        /// <inheritdoc />
        protected override void PlatformSetWindowSize(Size size) { }

        /// <inheritdoc />
        protected override void PlatformShow()
        {
            var shouldActivate = false;
            if (m_cache.Definition.AcceptsInput)
                shouldActivate = m_cache.Definition.ActivationPolicy == NWindowActivationPolicy.Always ||
                                 m_isFirstTimeVisible && m_cache.Definition.ActivationPolicy == NWindowActivationPolicy.FirstShown;

            var showWindowCommand = shouldActivate ? ShowWindowCommands.SW_SHOW : ShowWindowCommands.SW_SHOWNOACTIVATE;
            if (m_isFirstTimeVisible)
            {
                m_isFirstTimeVisible = false;
                switch (m_initialState)
                {
                    case NWindowState.Minimized:
                        showWindowCommand = shouldActivate ? ShowWindowCommands.SW_MINIMIZE : ShowWindowCommands.SW_SHOWMINNOACTIVE;
                        break;
                    case NWindowState.Maximized:
                        showWindowCommand = shouldActivate ? ShowWindowCommands.SW_SHOWMAXIMIZED : ShowWindowCommands.SW_MAXIMIZE;
                        break;
                }
            }


            ShowWindow(m_handle.Pointer, showWindowCommand);
        }

        /// <inheritdoc />
        protected override void PlatformSyncDataCache()
        {
            m_cache.ClientSize = PlatformGetClientSize();
            m_cache.MaxClientSize = PlatformGetMaxClientSize();
            m_cache.ClientArea = PlatformGetClientArea();
            m_cache.IsActive = PlatformCheckActive();
            m_cache.IsEnabled = PlatformCheckEnable();
            m_cache.IsFocused = PlatformCheckFocus();
            m_cache.IsVisible = PlatformCheckVisible();
            m_cache.Position = PlatformGetPosition();
            m_cache.Size = PlatformGetWindowSize();
            //m_cache.IsVisibleInTaskbar
            m_cache.Mode = PlatformGetMode();
            m_cache.State = PlatformGetState();
        }

        /// <inheritdoc />
        protected override void PlatformUnfocus()
        {
            SetFocus(IntPtr.Zero);
        }

        private void MaximizeWithoutCoveringTaskbar()
        {
            var monitor = MonitorFromWindow(m_handle.Pointer, MonitorFlag.MONITOR_DEFAULTTONEAREST);

            if (monitor == IntPtr.Zero)
                return;
            var monitorInfo = new MonitorInfo {Size = (uint)Marshal.SizeOf<MonitorInfo>()};
            GetMonitorInfo(monitor, ref monitorInfo);

            if (!GetMonitorInfo(monitor, ref monitorInfo))
                return;

            var x = monitorInfo.WorkRect.Left;
            var y = monitorInfo.WorkRect.Top;
            var cx = Math.Abs(monitorInfo.WorkRect.Right - x);
            var cy = Math.Abs(monitorInfo.WorkRect.Bottom - y);

            SetWindowPos(m_handle.Pointer, HwndZOrder.HWND_NOTOPMOST, x, y, cx, cy, WindowPositionFlags.SWP_SHOWWINDOW);
        }
        #endregion
    }
}