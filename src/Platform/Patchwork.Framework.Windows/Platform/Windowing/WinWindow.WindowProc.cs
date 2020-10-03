#region Usings
using System;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop.User32;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public sealed partial class WinWindow
    {
        #region Members
        private readonly WindowProc m_wndProc;
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
                    Core.MessagePump.PushWindowMessage(message.WParam != IntPtr.Zero
                                                           ? WindowMessageIds.Activating
                                                           : WindowMessageIds.Deactivating,
                                                       this);
                    break;
                case WindowsMessageIds.KILLFOCUS:
                    Core.MessagePump.PushWindowUnfocusedMessage(this);
                    break;
                case WindowsMessageIds.SETFOCUS:
                    Core.MessagePump.PushWindowFocusedMessage(this);
                    break;
                case WindowsMessageIds.SHOWWINDOW:
                    Core.MessagePump.PushWindowMessage(message.WParam != IntPtr.Zero
                                                           ? WindowMessageIds.Shown
                                                           : WindowMessageIds.Hidden,
                                                       this);
                    break;
                case WindowsMessageIds.SIZE:
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Resized, this);
                    break;
                case WindowsMessageIds.SIZING:
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Resizing, this);
                    break;
                case WindowsMessageIds.MOVE:
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Moved, this);
                    break;
                case WindowsMessageIds.MOVING:
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Moving, this);
                    break;
                case WindowsMessageIds.WINDOWPOSCHANGING:
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
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Created, this);
                    break;
                case WindowsMessageIds.DESTROY:
                    if (m_isMainApplicationWindow)
                        PostMessage(m_parent.Handle.Pointer, WindowsMessageIds.CLOSE, IntPtr.Zero, IntPtr.Zero);
                    Core.MessagePump.PushWindowMessage(WindowMessageIds.Destroyed, this);
                    break;
                case WindowsMessageIds.ENABLE:
                    Core.MessagePump.PushWindowMessage(message.WParam != IntPtr.Zero
                                                           ? WindowMessageIds.Enabled
                                                           : WindowMessageIds.Disabled,
                                                       this);
                    break;
                case WindowsMessageIds.QUIT:
                    break;
                case WindowsMessageIds.PAINT:
                case WindowsMessageIds.NULL:

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

            if (changed)
                InvalidateDataCache();

            return DefWindowProc(message.Hwnd, message.Id, message.WParam, message.LParam);
        }
        #endregion
    }
}