namespace Patchwork.Framework.Platform.Interop.User32
{
    public enum SystemParametersInputInfo
    {
        /// <summary>
        ///     Determines whether the warning beeper is on.
        ///     The pvParam parameter must point to a BOOL variable that receives TRUE if the beeper is on, or FALSE if it is off.
        /// </summary>
        SPI_GETBEEP = 0x0001,

        /// <summary>
        ///     Retrieves a BOOL indicating whether an application can reset the screensaver's timer by calling the SendInput
        ///     function to simulate keyboard or mouse input. The pvParam parameter must point to a BOOL variable that receives
        ///     TRUE if the simulated input will be blocked, or FALSE otherwise.
        /// </summary>
        SPI_GETBLOCKSENDINPUTRESETS = 0x1026,

        /// <summary>
        ///     Retrieves the current contact visualization setting. The pvParam parameter must point to a ULONG variable that
        ///     receives the setting. For more information, see Contact Visualization.
        /// </summary>
        SPI_GETCONTACTVISUALIZATION = 0x2018,

        /// <summary>
        ///     Retrieves the input locale identifier for the system default input language. The pvParam parameter must point to an
        ///     HKL variable that receives this value. For more information, see Languages, Locales, and Keyboard Layouts.
        /// </summary>
        SPI_GETDEFAULTINPUTLANG = 0x0059,

        /// <summary>
        ///     Retrieves the current gesture visualization setting. The pvParam parameter must point to a ULONG variable that
        ///     receives the setting. For more information, see Gesture Visualization.
        /// </summary>
        SPI_GETGESTUREVISUALIZATION = 0x201A,

        /// <summary>
        ///     Determines whether menu access keys are always underlined. The pvParam parameter must point to a BOOL variable that
        ///     receives TRUE if menu access keys are always underlined, and FALSE if they are underlined only when the menu is
        ///     activated by the keyboard.
        /// </summary>
        SPI_GETKEYBOARDCUES = 0x100A,

        /// <summary>
        ///     Retrieves the keyboard repeat-delay setting, which is a value in the range from 0 (approximately 250 ms delay)
        ///     through 3 (approximately 1 second delay). The actual delay associated with each value may vary depending on the
        ///     hardware. The pvParam parameter must point to an integer variable that receives the setting.
        /// </summary>
        SPI_GETKEYBOARDDELAY = 0x0016,

        /// <summary>
        ///     Determines whether the user relies on the keyboard instead of the mouse, and wants applications to display keyboard
        ///     interfaces that would otherwise be hidden. The pvParam parameter must point to a BOOL variable that receives TRUE
        ///     if the user relies on the keyboard; or FALSE otherwise.
        /// </summary>
        SPI_GETKEYBOARDPREF = 0x0044,

        /// <summary>
        ///     Retrieves the keyboard repeat-speed setting, which is a value in the range from 0 (approximately 2.5 repetitions
        ///     per second) through 31 (approximately 30 repetitions per second). The actual repeat rates are hardware-dependent
        ///     and may vary from a linear scale by as much as 20%. The pvParam parameter must point to a DWORD variable that
        ///     receives the setting.
        /// </summary>
        SPI_GETKEYBOARDSPEED = 0x000A,

        /// <summary>
        ///     Retrieves the two mouse threshold values and the mouse acceleration. The pvParam parameter must point to an array
        ///     of three integers that receives these values. See mouse_event for further information.
        /// </summary>
        SPI_GETMOUSE = 0x0003,

        /// <summary>
        ///     Retrieves the height, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent to
        ///     generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the height.
        /// </summary>
        SPI_GETMOUSEHOVERHEIGHT = 0x0064,

        /// <summary>
        ///     Retrieves the time, in milliseconds, that the mouse pointer has to stay in the hover rectangle for TrackMouseEvent
        ///     to generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the time.
        /// </summary>
        SPI_GETMOUSEHOVERTIME = 0x0066,

        /// <summary>
        ///     Retrieves the width, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent to
        ///     generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the width.
        /// </summary>
        SPI_GETMOUSEHOVERWIDTH = 0x0062,

        /// <summary>
        ///     Retrieves the current mouse speed. The mouse speed determines how far the pointer will move based on the distance
        ///     the mouse moves. The pvParam parameter must point to an integer that receives a value which ranges between 1
        ///     (slowest) and 20 (fastest). A value of 10 is the default. The value can be set by an end-user using the mouse
        ///     control panel application or by an application using SPI_SETMOUSESPEED.
        /// </summary>
        SPI_GETMOUSESPEED = 0x0070,

        /// <summary>
        ///     Determines whether the Mouse Trails feature is enabled. This feature improves the visibility of mouse cursor
        ///     movements by briefly showing a trail of cursors and quickly erasing them.
        ///     The pvParam parameter must point to an integer variable that receives a value. if  the value is zero or 1, the
        ///     feature is disabled. If the value is greater than 1, the feature is enabled and the value indicates the number of
        ///     cursors drawn in the trail. The uiParam parameter is not used.
        ///     Windows 2000:  This parameter is not supported.
        /// </summary>
        SPI_GETMOUSETRAILS = 0x005E,

        /// <summary>
        ///     Retrieves the routing setting for wheel button input. The routing setting determines whether wheel button input is
        ///     sent to the app with focus (foreground) or the app under the mouse cursor.
        ///     The pvParam parameter must point to a DWORD variable that receives the routing option.
        ///     If  the value is zero or MOUSEWHEEL_ROUTING_FOCUS, mouse wheel input is delivered to the app with focus. If the
        ///     value is 1 or MOUSEWHEEL_ROUTING_HYBRID (default), mouse wheel input is delivered to the app with focus (desktop
        ///     apps) or the app under the mouse cursor (Windows Store apps).
        ///     The uiParam parameter is not used.
        /// </summary>
        SPI_GETMOUSEWHEELROUTING = 0x201C,

        /// <summary>
        ///     Retrieves the current pen gesture visualization setting. The pvParam parameter must point to a ULONG variable that
        ///     receives the setting. For more information, see Pen Visualization.
        /// </summary>
        SPI_GETPENVISUALIZATION = 0x201E,

        /// <summary>
        ///     Determines whether the snap-to-default-button feature is enabled. If enabled, the mouse cursor automatically moves
        ///     to the default button, such as OK or Apply, of a dialog box. The pvParam parameter must point to a BOOL variable
        ///     that receives TRUE if the feature is on, or FALSE if it is off.
        /// </summary>
        SPI_GETSNAPTODEFBUTTON = 0x005F,

        /// <summary>
        ///     Starting with Windows 8: Determines whether the system language bar is enabled or disabled. The pvParam parameter
        ///     must point to a BOOL variable that receives TRUE if the language bar is enabled, or FALSE otherwise.
        /// </summary>
        SPI_GETSYSTEMLANGUAGEBAR = 0x1050,

        /// <summary>
        ///     Starting with Windows 8: Determines whether the active input settings have Local (per-thread, TRUE) or Global
        ///     (session, FALSE) scope. The pvParam parameter must point to a BOOL variable.
        /// </summary>
        SPI_GETTHREADLOCALINPUTSETTINGS = 0x104E,

        /// <summary>
        ///     Retrieves the number of characters to scroll when the horizontal mouse wheel is moved. The pvParam parameter must
        ///     point to a UINT variable that receives the number of lines. The default value is 3.
        /// </summary>
        SPI_GETWHEELSCROLLCHARS = 0x006C,

        /// <summary>
        ///     Retrieves the number of lines to scroll when the vertical mouse wheel is moved. The pvParam parameter must point to
        ///     a UINT variable that receives the number of lines. The default value is 3.
        /// </summary>
        SPI_GETWHEELSCROLLLINES = 0x0068,

        /// <summary>
        ///     Turns the warning beeper on or off. The uiParam parameter specifies TRUE for on, or FALSE for off.
        /// </summary>
        SPI_SETBEEP = 0x0002,

        /// <summary>
        ///     Determines whether an application can reset the screensaver's timer by calling the SendInput function to simulate
        ///     keyboard or mouse input. The uiParam parameter specifies TRUE if the screensaver will not be deactivated by
        ///     simulated input, or FALSE if the screensaver will be deactivated by simulated input.
        /// </summary>
        SPI_SETBLOCKSENDINPUTRESETS = 0x1027,

        /// <summary>
        ///     Sets the current contact visualization setting. The pvParam parameter must point to a ULONG variable that
        ///     identifies the setting. For more information, see Contact Visualization.
        ///     Note  If contact visualizations are disabled, gesture visualizations cannot be enabled.
        /// </summary>
        SPI_SETCONTACTVISUALIZATION = 0x2019,

        /// <summary>
        ///     Sets the default input language for the system shell and applications. The specified language must be displayable
        ///     using the current system character set. The pvParam parameter must point to an HKL variable that contains the input
        ///     locale identifier for the default language. For more information, see Languages, Locales, and Keyboard Layouts.
        /// </summary>
        SPI_SETDEFAULTINPUTLANG = 0x005A,

        /// <summary>
        ///     Sets the double-click time for the mouse to the value of the uiParam parameter. If the uiParam value is greater
        ///     than 5000 milliseconds, the system sets the double-click time to 5000 milliseconds.
        ///     The double-click time is the maximum number of milliseconds that can occur between the first and second clicks of a
        ///     double-click. You can also call the SetDoubleClickTime function to set the double-click time. To get the current
        ///     double-click time, call the GetDoubleClickTime function.
        /// </summary>
        SPI_SETDOUBLECLICKTIME = 0x0020,

        /// <summary>
        ///     Sets the height of the double-click rectangle to the value of the uiParam parameter.
        ///     The double-click rectangle is the rectangle within which the second click of a double-click must fall for it to be
        ///     registered as a double-click.
        ///     To retrieve the height of the double-click rectangle, call  GetSystemMetrics with the SM_CYDOUBLECLK flag.
        /// </summary>
        SPI_SETDOUBLECLKHEIGHT = 0x001E,

        /// <summary>
        ///     Sets the width of the double-click rectangle to the value of the uiParam parameter.
        ///     The double-click rectangle is the rectangle within which the second click of a double-click must fall for it to be
        ///     registered as a double-click.
        ///     To retrieve the width of the double-click rectangle, call GetSystemMetrics with the SM_CXDOUBLECLK flag.
        /// </summary>
        SPI_SETDOUBLECLKWIDTH = 0x001D,

        /// <summary>
        ///     Sets the current gesture visualization setting. The pvParam parameter must point to a ULONG variable that
        ///     identifies the setting. For more information, see Gesture Visualization.
        ///     Note  If contact visualizations are disabled, gesture visualizations cannot be enabled.
        /// </summary>
        SPI_SETGESTUREVISUALIZATION = 0x201B,

        /// <summary>
        ///     Sets the underlining of menu access key letters. The pvParam parameter is a BOOL variable. Set pvParam to TRUE to
        ///     always underline menu access keys, or FALSE to underline menu access keys only when the menu is activated from the
        ///     keyboard.
        /// </summary>
        SPI_SETKEYBOARDCUES = 0x100B,

        /// <summary>
        ///     Sets the keyboard repeat-delay setting. The uiParam parameter must specify 0, 1, 2, or 3, where zero sets the
        ///     shortest delay approximately 250 ms) and 3 sets the longest delay (approximately 1 second). The actual delay
        ///     associated with each value may vary depending on the hardware.
        /// </summary>
        SPI_SETKEYBOARDDELAY = 0x0017,

        /// <summary>
        ///     Sets the keyboard preference. The uiParam parameter specifies TRUE if the user relies on the keyboard instead of
        ///     the mouse, and wants applications to display keyboard interfaces that would otherwise be hidden; uiParam is FALSE
        ///     otherwise.
        /// </summary>
        SPI_SETKEYBOARDPREF = 0x0045,

        /// <summary>
        ///     Sets the keyboard repeat-speed setting. The uiParam parameter must specify a value in the range from 0
        ///     (approximately 2.5 repetitions per second) through 31 (approximately 30 repetitions per second). The actual repeat
        ///     rates are hardware-dependent and may vary from a linear scale by as much as 20%. If uiParam is greater than 31, the
        ///     parameter is set to 31.
        /// </summary>
        SPI_SETKEYBOARDSPEED = 0x000B,

        /// <summary>
        ///     Sets the hot key set for switching between input languages. The uiParam and pvParam parameters are not used. The
        ///     value sets the shortcut keys in the keyboard property sheets by reading the registry again. The registry must be
        ///     set before this flag is used. the path in the registry is
        ///     HKEY_CURRENT_USER\Keyboard Layout\Toggle. Valid values are "1" = ALT+SHIFT, "2" = CTRL+SHIFT, and "3" = none.
        /// </summary>
        SPI_SETLANGTOGGLE = 0x005B,

        /// <summary>
        ///     Sets the two mouse threshold values and the mouse acceleration. The pvParam parameter must point to an array of
        ///     three integers that specifies these values. See mouse_event for further information.
        /// </summary>
        SPI_SETMOUSE = 0x0004,

        /// <summary>
        ///     Swaps or restores the meaning of the left and right mouse buttons. The uiParam parameter specifies TRUE to swap the
        ///     meanings of the buttons, or FALSE to restore their original meanings.
        ///     To retrieve the current setting, call GetSystemMetrics with the SM_SWAPBUTTON flag.
        /// </summary>
        SPI_SETMOUSEBUTTONSWAP = 0x0021,

        /// <summary>
        ///     Sets the height, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent to
        ///     generate a WM_MOUSEHOVER message. Set the uiParam parameter to the new height.
        /// </summary>
        SPI_SETMOUSEHOVERHEIGHT = 0x0065,

        /// <summary>
        ///     Sets the time, in milliseconds, that the mouse pointer has to stay in the hover rectangle for TrackMouseEvent to
        ///     generate a WM_MOUSEHOVER message. This is used only if you pass HOVER_DEFAULT in the dwHoverTime parameter in the
        ///     call to TrackMouseEvent. Set the uiParamparameter to the new time.
        ///     The time specified should be between USER_TIMER_MAXIMUM and USER_TIMER_MINIMUM. If uiParam is less than
        ///     USER_TIMER_MINIMUM, the function will use USER_TIMER_MINIMUM. If uiParam is greater than USER_TIMER_MAXIMUM, the
        ///     function will be USER_TIMER_MAXIMUM.
        ///     Windows Server 2003 and Windows XP:  The operating system does not enforce the use of USER_TIMER_MAXIMUM and
        ///     USER_TIMER_MINIMUM until Windows Server 2003 with SP1 and Windows XP with SP2.
        /// </summary>
        SPI_SETMOUSEHOVERTIME = 0x0067,

        /// <summary>
        ///     Sets the width, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent to
        ///     generate a WM_MOUSEHOVER message. Set the uiParam parameter to the new width.
        /// </summary>
        SPI_SETMOUSEHOVERWIDTH = 0x0063,

        /// <summary>
        ///     Sets the current mouse speed. The pvParam parameter is an integer between 1 (slowest) and 20 (fastest). A value of
        ///     10 is the default. This value is typically set using the mouse control panel application.
        /// </summary>
        SPI_SETMOUSESPEED = 0x0071,

        /// <summary>
        ///     Enables or disables the Mouse Trails feature, which improves the visibility of mouse cursor movements by briefly
        ///     showing a trail of cursors and quickly erasing them.
        ///     To disable the feature, set the uiParam parameter to zero or 1. To enable the  feature, set uiParam to a value
        ///     greater than 1 to indicate the number of cursors drawn in the trail.
        ///     Windows 2000:  This parameter is not supported.
        /// </summary>
        SPI_SETMOUSETRAILS = 0x005D,

        /// <summary>
        ///     Sets the routing setting for wheel button input. The routing setting determines whether wheel button input is sent
        ///     to the app with focus (foreground) or the app under the mouse cursor.
        ///     The pvParam parameter must point to a DWORD variable that receives the routing option.
        ///     If  the value is zero or MOUSEWHEEL_ROUTING_FOCUS, mouse wheel input is delivered to the app with focus. If the
        ///     value is 1 or MOUSEWHEEL_ROUTING_HYBRID (default), mouse wheel input is delivered to the app with focus (desktop
        ///     apps) or the app under the mouse cursor (Windows Store apps).
        ///     Set the uiParam parameter to zero.
        /// </summary>
        SPI_SETMOUSEWHEELROUTING = 0x201D,

        /// <summary>
        ///     Sets the current pen gesture visualization setting. The pvParam parameter must point to a ULONG variable that
        ///     identifies the setting. For more information, see Pen Visualization.
        /// </summary>
        SPI_SETPENVISUALIZATION = 0x201F,

        /// <summary>
        ///     Enables or disables the snap-to-default-button feature. If enabled, the mouse cursor automatically moves to the
        ///     default button, such as OK or Apply, of a dialog box. Set the uiParam parameter to TRUE to enable the feature, or
        ///     FALSE to disable it. Applications should use the ShowWindow function when displaying a dialog box so the dialog
        ///     manager can position the mouse cursor.
        /// </summary>
        SPI_SETSNAPTODEFBUTTON = 0x0060,

        /// <summary>
        ///     Starting with Windows 8: Turns the legacy language bar feature on or off. The pvParam parameter is a pointer to a
        ///     BOOL variable. Set pvParam to TRUE to enable the legacy language bar, or FALSE to disable it. The flag is supported
        ///     on Windows 8 where the legacy language bar is replaced by Input Switcher and therefore turned off by default.
        ///     Turning the legacy language bar on is provided for compatibility reasons and has no effect on the Input Switcher.
        /// </summary>
        SPI_SETSYSTEMLANGUAGEBAR = 0x1051,

        /// <summary>
        ///     Starting with Windows 8: Determines whether the active input settings have Local (per-thread, TRUE) or Global
        ///     (session, FALSE) scope. The pvParam parameter must point to a BOOL variable, casted by PVOID.
        /// </summary>
        SPI_SETTHREADLOCALINPUTSETTINGS = 0x104F,

        /// <summary>
        ///     Sets the number of characters to scroll when the horizontal mouse wheel is moved. The number of characters is set
        ///     from the uiParam parameter.
        /// </summary>
        SPI_SETWHEELSCROLLCHARS = 0x006D,

        /// <summary>
        ///     Sets the number of lines to scroll when the vertical mouse wheel is moved. The number of lines is set from the
        ///     uiParam parameter.
        ///     The number of lines is the suggested number of lines to scroll when the mouse wheel is rolled without using
        ///     modifier keys. If the number is 0, then no scrolling should occur. If the number of lines to scroll is  greater
        ///     than the number of lines viewable, and in particular if it is WHEEL_PAGESCROLL (#defined as UINT_MAX), the scroll
        ///     operation should be interpreted as clicking once in the page down or page up regions of the scroll bar.
        /// </summary>
        SPI_SETWHEELSCROLLLINES = 0x0069
    }
}