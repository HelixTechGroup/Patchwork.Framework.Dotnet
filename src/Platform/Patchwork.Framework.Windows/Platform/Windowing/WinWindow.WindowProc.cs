#region Usings
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Rendering;
using Shin.Framework.Extensions;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Utilities;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public sealed partial class WinWindow
    {
        #region Members
        private readonly WindowProc m_wndProc;
        private bool m_inModalSizeLoop;
        private bool m_timerRunning;
        #endregion

        #region Properties
        /// <inheritdoc />
        public WindowProc Process
        {
            get { return m_wndProc; }
        }
        #endregion

        #region Methods
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

        private IntPtr OnMessage(WindowsMessage message)
        {
            var changed = false;
            switch (message.Id)
            {
                case WindowsMessageIds.ACTIVATEAPP:
                    changed = true;
                    Core.MessagePump.PushWindowMessage(message.WParam != IntPtr.Zero
                                                           ? WindowMessageIds.Activating
                                                           : WindowMessageIds.Deactivating,
                                                       this);
                    break;
                case WindowsMessageIds.KILLFOCUS:
                    changed = true;
                    Core.MessagePump.PushWindowUnfocusedMessage(this);
                    break;
                case WindowsMessageIds.SETFOCUS:
                    changed = true;
                    Core.MessagePump.PushWindowFocusedMessage(this);
                    break;
                case WindowsMessageIds.SHOWWINDOW:
                    changed = true;
                    Core.MessagePump.PushWindowMessage(message.WParam != IntPtr.Zero
                                                           ? WindowMessageIds.Shown
                                                           : WindowMessageIds.Hidden,
                                                       this);
                    break;
                case WindowsMessageIds.SIZE:
                    changed = true;
                    //InvalidateDataCache();
                    //Core.MessagePump.PushWindowMessage(WindowMessageIds.Resized, this);
                    uint xy = unchecked(IntPtr.Size == 8 ? (uint)message.LParam.ToInt64() : (uint)message.LParam.ToInt32());
                    int x = unchecked((short)xy);
                    int y = unchecked((short)(xy >> 16));
                    var nSize = new Size(x, y);
                    Core.MessagePump.PushWindowResizedMessage(this, nSize);
                    switch ((WindowSizeFlag)message.WParam)
                    {
                        case WindowSizeFlag.SIZE_MAXIMIZED:
                            changed = true;
                            Core.MessagePump.PushWindowStateChangedMessage(this, NWindowState.Maximized);
                            //Core.MessagePump.PushWindowMessage(WindowMessageIds.Restored, this);
                            break;
                        case WindowSizeFlag.SIZE_RESTORED:
                            changed = true;
                            Core.MessagePump.PushWindowStateChangedMessage(this, NWindowState.Restored);
                            break;
                        case WindowSizeFlag.SIZE_MINIMIZED:
                            changed = true;
                            Core.MessagePump.PushWindowStateChangedMessage(this, NWindowState.Minimized);
                            break;
                    }
                    break;
                case WindowsMessageIds.SIZING:
                    changed = true;
                    //InvalidateDataCache();
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Resizing, this);
                    break;
                case WindowsMessageIds.MOVE:
                    changed = true;
                    //InvalidateDataCache();
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Moved, this);
                    break;
                case WindowsMessageIds.MOVING:
                    changed = true;
                    //InvalidateDataCache();
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Moving, this);
                    break;
                case WindowsMessageIds.WINDOWPOSCHANGING:
                    //InvalidateDataCache();
                    //PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Resized, this));
                    break;
                case WindowsMessageIds.WINDOWPOSCHANGED:
                    //PlatformManager.MessagePump.Push(new DesktopWindowMessage(WindowMessageIds.Resizing, this));
                    break;
                case WindowsMessageIds.CLOSE:
                    //DestroyWindow(m_handle.Pointer);
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Closed, this);
                    break;
                case WindowsMessageIds.CREATE:
                    changed = true;
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Created, this);
                    break;
                case WindowsMessageIds.DESTROY:
                    if (m_isMainApplicationWindow)
                        PostMessage(m_parent.Handle.Pointer, WindowsMessageIds.QUIT, IntPtr.Zero, IntPtr.Zero);

                    m_inModalSizeLoop = false;
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Destroyed, this);
                    break;
                case WindowsMessageIds.ENABLE:
                    Core.MessagePump.PushWindowMessage(message.WParam != IntPtr.Zero
                                                           ? WindowMessageIds.Enabled
                                                           : WindowMessageIds.Disabled,
                                                       this);
                    break;
                case WindowsMessageIds.QUIT:
                    Core.MessagePump.Push(new PlatformMessage(MessageIds.Quit));
                    break;
                case WindowsMessageIds.PAINT:
                    //Core.MessagePump.PushRenderOsMessage(this,);
                    //Render();
                    break;
                case WindowsMessageIds.ERASEBKGND:
                    //return new IntPtr(1);
                    //break;
                case WindowsMessageIds.TIMER:
                    //changed = m_inModalSizeLoop;
                    if (m_inModalSizeLoop)
                    {
                        InvalidateDataCache();
                        SyncDataCache(true);
                        Core.Pump();
                        //Render();
                    }
                    break;
                case WindowsMessageIds.ENTERSIZEMOVE:
                    if (m_timerRunning)
                        break;

                    CheckOperation(SetTimer(m_handle.Pointer, new IntPtr(12345), 33, null) != IntPtr.Zero);
                    m_timerRunning = true;
                    m_inModalSizeLoop = true;
                    break;
                case WindowsMessageIds.EXITSIZEMOVE:
                    if (!m_timerRunning)
                        break;
                    
                    CheckOperation(KillTimer(m_handle.Pointer, new IntPtr(12345)));
                    m_timerRunning = false;
                    m_inModalSizeLoop = false;
                    break;
                case WindowsMessageIds.SYSCOMMAND:
                    switch ((SysCommand)message.WParam)
                    {
                        case SysCommand.SC_MAXIMIZE:
                            changed = true;
                            Core.MessagePump.PushWindowStateChangedMessage(this, NWindowState.Maximized);
                            break;
                        case SysCommand.SC_RESTORE:
                            changed = true;
                            Core.MessagePump.PushWindowStateChangedMessage(this, NWindowState.Restored);
                            //ore.MessagePump.PushWindowMessage(WindowMessageIds.Restored, this);
                            break;
                        case SysCommand.SC_MINIMIZE:
                            changed = true;
                            Core.MessagePump.PushWindowStateChangedMessage(this, NWindowState.Minimized);
                            break;
                    }

                    break;
                case WindowsMessageIds.NULL:
                case WindowsMessageIds.SETREDRAW:
                case WindowsMessageIds.SETTEXT:
                case WindowsMessageIds.GETTEXT:
                case WindowsMessageIds.GETTEXTLENGTH:
                case WindowsMessageIds.QUERYENDSESSION:
                case WindowsMessageIds.QUERYOPEN:
                case WindowsMessageIds.ENDSESSION:
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
                case WindowsMessageIds.MENUCOMMAND:
                case WindowsMessageIds.APPCOMMAND:
                case WindowsMessageIds.CAPTURECHANGED:
                case WindowsMessageIds.HOTKEY:
                case WindowsMessageIds.NCMOUSEMOVE:
                default:
                    break;
            }

            if (changed)
                InvalidateDataCache();

            return DefWindowProc(message.Hwnd, message.Id, message.WParam, message.LParam);
        }
        #endregion
    }
}