namespace Patchwork.Framework.Platform.Interop.User32 {
    public enum SystemParametersWindowInfo
    {
        /// <summary>
        ///     Determines whether active window tracking (activating the window the mouse is on) is on or off. The pvParam
        ///     parameter must point to a BOOL variable that receives TRUE for on, or FALSE for off.
        /// </summary>
        SPI_GETACTIVEWINDOWTRACKING = 0x1000,

        /// <summary>
        ///     Determines whether windows activated through active window tracking will be brought to the top. The pvParam
        ///     parameter must point to a BOOL variable that receives TRUE for on, or FALSE for off.
        /// </summary>
        SPI_GETACTIVEWNDTRKZORDER = 0x100C,

        /// <summary>
        ///     Retrieves the active window tracking delay, in milliseconds. The pvParam parameter must point to a DWORD variable
        ///     that receives the time.
        /// </summary>
        SPI_GETACTIVEWNDTRKTIMEOUT = 0x2002,

        /// <summary>
        ///     Retrieves the animation effects associated with user actions. The pvParam parameter must point to an ANIMATIONINFO
        ///     structure that receives the information. Set the cbSize member of this structure and the uiParam parameter to
        ///     sizeof(ANIMATIONINFO).
        /// </summary>
        SPI_GETANIMATION = 0x0048,

        /// <summary>
        ///     Retrieves the border multiplier factor that determines the width of a window's sizing border. The pvParamparameter
        ///     must point to an integer variable that receives this value.
        /// </summary>
        SPI_GETBORDER = 0x0005,

        /// <summary>
        ///     Retrieves the caret width in edit controls, in pixels. The pvParam parameter must point to a DWORD variable that
        ///     receives this value.
        /// </summary>
        SPI_GETCARETWIDTH = 0x2006,

        /// <summary>
        ///     Determines whether a window is docked when it is moved to the top, left, or right edges of a monitor or monitor
        ///     array. The pvParam parameter must point to a BOOL variable that receives TRUE if enabled, or FALSE otherwise.
        ///     Use SPI_GETWINARRANGING to determine whether this behavior is enabled.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETDOCKMOVING = 0x0090,

        /// <summary>
        ///     Determines whether a maximized window is restored when its caption bar is dragged. The pvParam parameter must point
        ///     to a BOOL variable that receives TRUE if enabled, or FALSE otherwise.
        ///     Use SPI_GETWINARRANGING to determine whether this behavior is enabled.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETDRAGFROMMAXIMIZE = 0x008C,

        /// <summary>
        ///     Determines whether dragging of full windows is enabled. The pvParam parameter must point to a BOOL variable that
        ///     receives TRUE if enabled, or FALSE otherwise.
        /// </summary>
        SPI_GETDRAGFULLWINDOWS = 0x0026,

        /// <summary>
        ///     Retrieves the number of times SetForegroundWindow will flash the taskbar button when rejecting a foreground switch
        ///     request. The pvParam parameter must point to a DWORD variable that receives the value.
        /// </summary>
        SPI_GETFOREGROUNDFLASHCOUNT = 0x2004,

        /// <summary>
        ///     Retrieves the amount of time following user input, in milliseconds, during which the system will not allow
        ///     applications to force themselves into the foreground. The pvParam parameter must point to a DWORD variable that
        ///     receives the time.
        /// </summary>
        SPI_GETFOREGROUNDLOCKTIMEOUT = 0x2000,

        /// <summary>
        ///     Retrieves the metrics associated with minimized windows. The pvParam parameter must point to a MINIMIZEDMETRICS
        ///     structure that receives the information. Set the cbSize member of this structure and the uiParam parameter to
        ///     sizeof(MINIMIZEDMETRICS).
        /// </summary>
        SPI_GETMINIMIZEDMETRICS = 0x002B,

        /// <summary>
        ///     Retrieves the threshold in pixels where docking behavior is triggered by using a mouse to drag a window to the edge
        ///     of a monitor or monitor array. The default threshold is 1. The pvParam parameter must point to a DWORD variable
        ///     that receives the value.
        ///     Use SPI_GETWINARRANGING to determine whether this behavior is enabled.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETMOUSEDOCKTHRESHOLD = 0x007E,

        /// <summary>
        ///     Retrieves the threshold in pixels where undocking behavior is triggered by using a mouse to drag a window from the
        ///     edge of a monitor or a monitor array toward the center. The default threshold is 20.
        ///     Use SPI_GETWINARRANGING to determine whether this behavior is enabled.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETMOUSEDRAGOUTTHRESHOLD = 0x0084,

        /// <summary>
        ///     Retrieves the threshold in pixels from the top of a monitor or a monitor array where a vertically maximized window
        ///     is restored when dragged with the mouse. The default threshold is 50.
        ///     Use SPI_GETWINARRANGING to determine whether this behavior is enabled.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETMOUSESIDEMOVETHRESHOLD = 0x0088,

        /// <summary>
        ///     Retrieves the metrics associated with the nonclient area of nonminimized windows. The pvParam parameter must point
        ///     to a NONCLIENTMETRICS structure that receives the information. Set the cbSize member of this structure and the
        ///     uiParam parameter to sizeof(NONCLIENTMETRICS).
        /// </summary>
        SPI_GETNONCLIENTMETRICS = 0x0029,

        /// <summary>
        ///     Retrieves the threshold in pixels where docking behavior is triggered by using a pen to drag a window to the edge
        ///     of a monitor or monitor array. The default is 30.
        ///     Use SPI_GETWINARRANGING to determine whether this behavior is enabled.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETPENDOCKTHRESHOLD = 0x0080,

        /// <summary>
        ///     Retrieves the threshold in pixels where undocking behavior is triggered by using a pen to drag a window from the
        ///     edge of a monitor or monitor array toward its center. The default threshold is 30.
        ///     Use SPI_GETWINARRANGING to determine whether this behavior is enabled.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETPENDRAGOUTTHRESHOLD = 0x0086,

        /// <summary>
        ///     Retrieves the threshold in pixels from the top of a monitor or monitor array where a vertically maximized window
        ///     is restored when dragged with the mouse. The default threshold is 50.
        ///     Use SPI_GETWINARRANGING to determine whether this behavior is enabled.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETPENSIDEMOVETHRESHOLD = 0x008A,

        /// <summary>
        ///     Determines whether the IME status window is visible (on a per-user basis). The pvParam parameter must point to a
        ///     BOOL variable that receives TRUE if the status window is visible, or FALSE if it is not.
        /// </summary>
        SPI_GETSHOWIMEUI = 0x006E,

        /// <summary>
        ///     Determines whether a window is vertically maximized when it is sized to the top or bottom of a monitor or monitor
        ///     array. The pvParam parameter must point to a BOOL variable that receives TRUE if enabled, or FALSE otherwise.
        ///     Use SPI_GETWINARRANGING to determine whether this behavior is enabled.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETSNAPSIZING = 0x008E,

        /// <summary>
        ///     Determines whether window arrangement is enabled. The pvParam parameter must point to a BOOL variable that receives
        ///     TRUE if enabled, or FALSE otherwise.
        ///     Window arrangement reduces the number of mouse, pen, or touch interactions needed to move and size top-level
        ///     windows by simplifying the default behavior of a window when it is dragged or sized.
        ///     The following parameters retrieve individual window arrangement settings:
        ///     SPI_GETDOCKMOVING
        ///     SPI_GETMOUSEDOCKTHRESHOLD
        ///     SPI_GETMOUSEDRAGOUTTHRESHOLD
        ///     SPI_GETMOUSESIDEMOVETHRESHOLD
        ///     SPI_GETPENDOCKTHRESHOLD
        ///     SPI_GETPENDRAGOUTTHRESHOLD
        ///     SPI_GETPENSIDEMOVETHRESHOLD
        ///     SPI_GETSNAPSIZING
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETWINARRANGING = 0x0082,

        /// <summary>
        ///     Sets active window tracking (activating the window the mouse is on) either on or off. Set pvParam to TRUE for on or
        ///     FALSE for off.
        /// </summary>
        SPI_SETACTIVEWINDOWTRACKING = 0x1001,

        /// <summary>
        ///     Determines whether or not windows activated through active window tracking should be brought to the top. Set
        ///     pvParam to TRUE for on or FALSE for off.
        /// </summary>
        SPI_SETACTIVEWNDTRKZORDER = 0x100D,

        /// <summary>
        ///     Sets the active window tracking delay. Set pvParam to the number of milliseconds to delay before activating the
        ///     window under the mouse pointer.
        /// </summary>
        SPI_SETACTIVEWNDTRKTIMEOUT = 0x2003,

        /// <summary>
        ///     Sets the animation effects associated with user actions. The pvParam parameter must point to an ANIMATIONINFO
        ///     structure that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to
        ///     sizeof(ANIMATIONINFO).
        /// </summary>
        SPI_SETANIMATION = 0x0049,

        /// <summary>
        ///     Sets the border multiplier factor that determines the width of a window's sizing border. The uiParam parameter
        ///     specifies the new value.
        /// </summary>
        SPI_SETBORDER = 0x0006,

        /// <summary>
        ///     Sets the caret width in edit controls. Set pvParam to the desired width, in pixels. The default and minimum value
        ///     is 1.
        /// </summary>
        SPI_SETCARETWIDTH = 0x2007,

        /// <summary>
        ///     Sets whether a window is docked when it is moved to the top, left, or right docking targets on a monitor or monitor
        ///     array. Set pvParam to TRUE for on or FALSE for off.
        ///     SPI_GETWINARRANGING must be TRUE to enable this behavior.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETDOCKMOVING = 0x0091,

        /// <summary>
        ///     Sets whether a maximized window is restored when its caption bar is dragged. Set pvParam to TRUE for on or FALSE
        ///     for off.
        ///     SPI_GETWINARRANGING must be TRUE to enable this behavior.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETDRAGFROMMAXIMIZE = 0x008D,

        /// <summary>
        ///     Sets dragging of full windows either on or off. The uiParam parameter specifies TRUE for on, or FALSE for off.
        /// </summary>
        SPI_SETDRAGFULLWINDOWS = 0x0025,

        /// <summary>
        ///     Sets the height, in pixels, of the rectangle used to detect the start of a drag operation. Set uiParam to the new
        ///     value. To retrieve the drag height, call GetSystemMetrics with the SM_CYDRAG flag.
        /// </summary>
        SPI_SETDRAGHEIGHT = 0x004D,

        /// <summary>
        ///     Sets the width, in pixels, of the rectangle used to detect the start of a drag operation. Set uiParam to the new
        ///     value. To retrieve the drag width, call GetSystemMetrics with the SM_CXDRAG flag.
        /// </summary>
        SPI_SETDRAGWIDTH = 0x004C,

        /// <summary>
        ///     Sets the number of times SetForegroundWindow will flash the taskbar button when rejecting a foreground switch
        ///     request. Set pvParam to the number of times to flash.
        /// </summary>
        SPI_SETFOREGROUNDFLASHCOUNT = 0x2005,

        /// <summary>
        ///     Sets the amount of time following user input, in milliseconds, during which the system does not allow applications
        ///     to force themselves into the foreground. Set pvParam to the new time-out value.
        ///     The calling thread must be able to change the foreground window, otherwise the call fails.
        /// </summary>
        SPI_SETFOREGROUNDLOCKTIMEOUT = 0x2001,

        /// <summary>
        ///     Sets the metrics associated with minimized windows. The pvParam parameter must point to a MINIMIZEDMETRICS
        ///     structure that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to
        ///     sizeof(MINIMIZEDMETRICS).
        /// </summary>
        SPI_SETMINIMIZEDMETRICS = 0x002C,

        /// <summary>
        ///     Sets the threshold in pixels where docking behavior is triggered by using a mouse to drag a window to the edge of a
        ///     monitor or monitor array. The default threshold is 1. The pvParam parameter must point to a DWORD variable that
        ///     contains the new value.
        ///     SPI_GETWINARRANGING must be TRUE to enable this behavior.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETMOUSEDOCKTHRESHOLD = 0x007F,

        /// <summary>
        ///     Sets the threshold in pixels where undocking behavior is triggered by using a mouse to drag a window from the edge
        ///     of a monitor or monitor array to its center. The default threshold is 20. The pvParam parameter must point to a
        ///     DWORD variable that contains the new value.
        ///     SPI_GETWINARRANGING must be TRUE to enable this behavior.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETMOUSEDRAGOUTTHRESHOLD = 0x0085,

        /// <summary>
        ///     Sets the threshold in pixels from the top of the monitor where a vertically maximized window is restored when
        ///     dragged with the mouse. The default threshold is 50. The pvParam parameter must point to a DWORD variable that
        ///     contains the new value.
        ///     SPI_GETWINARRANGING must be TRUE to enable this behavior.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETMOUSESIDEMOVETHRESHOLD = 0x0089,

        /// <summary>
        ///     Sets the metrics associated with the nonclient area of nonminimized windows. The pvParam parameter must point to a
        ///     NONCLIENTMETRICS structure that contains the new parameters. Set the cbSize member of this structure and the
        ///     uiParam parameter to sizeof(NONCLIENTMETRICS). Also, the lfHeight member of the LOGFONT structure must be a
        ///     negative value.
        /// </summary>
        SPI_SETNONCLIENTMETRICS = 0x002A,

        /// <summary>
        ///     Sets the threshold in pixels where docking behavior is triggered by using a pen to drag a window to the edge of a
        ///     monitor or monitor array. The default threshold is 30. The pvParam parameter must point to a DWORD variable that
        ///     contains the new value.
        ///     SPI_GETWINARRANGING must be TRUE to enable this behavior.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETPENDOCKTHRESHOLD = 0x0081,

        /// <summary>
        ///     Sets the threshold in pixels where undocking behavior is triggered by using a pen to drag a window from the edge of
        ///     a monitor or monitor array to its center. The default threshold is 30. The pvParam parameter must point to a DWORD
        ///     variable that contains the new value.
        ///     SPI_GETWINARRANGING must be TRUE to enable this behavior.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETPENDRAGOUTTHRESHOLD = 0x0087,

        /// <summary>
        ///     Sets the threshold in pixels from the top of the monitor where a vertically maximized window is restored when
        ///     dragged with a pen. The default threshold is 50. The pvParam parameter must point to a DWORD variable that contains
        ///     the new value.
        ///     SPI_GETWINARRANGING must be TRUE to enable this behavior.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETPENSIDEMOVETHRESHOLD = 0x008B,

        /// <summary>
        ///     Sets whether the IME status window is visible or not on a per-user basis. The uiParam parameter specifies TRUE for
        ///     on or FALSE for off.
        /// </summary>
        SPI_SETSHOWIMEUI = 0x006F,

        /// <summary>
        ///     Sets whether a window is vertically maximized when it is sized to the top or bottom of the monitor. Set pvParam to
        ///     TRUE for on or FALSE for off.
        ///     SPI_GETWINARRANGING must be TRUE to enable this behavior.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETSNAPSIZING = 0x008F,

        /// <summary>
        ///     Sets whether window arrangement is enabled. Set pvParam to TRUE for on or FALSE for off.
        ///     Window arrangement reduces the number of mouse, pen, or touch interactions needed to move and size top-level
        ///     windows by simplifying the default behavior of a window when it is dragged or sized.
        ///     The following parameters set individual window arrangement settings:
        ///     SPI_SETDOCKMOVING
        ///     SPI_SETMOUSEDOCKTHRESHOLD
        ///     SPI_SETMOUSEDRAGOUTTHRESHOLD
        ///     SPI_SETMOUSESIDEMOVETHRESHOLD
        ///     SPI_SETPENDOCKTHRESHOLD
        ///     SPI_SETPENDRAGOUTTHRESHOLD
        ///     SPI_SETPENSIDEMOVETHRESHOLD
        ///     SPI_SETSNAPSIZING
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETWINARRANGING = 0x0083
    }
}