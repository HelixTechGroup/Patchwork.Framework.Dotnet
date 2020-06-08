namespace Patchwork.Framework.Platform.Interop.User32 {
    public enum SystemParametersUiEffectsInfo
    {
        /// <summary>
        ///     Determines whether the slide-open effect for combo boxes is enabled. The pvParam parameter must point to a BOOL
        ///     variable that receives TRUE for enabled, or FALSE for disabled.
        /// </summary>
        SPI_GETCOMBOBOXANIMATION = 0x1004,

        /// <summary>
        ///     Determines whether the cursor has a shadow around it. The pvParam parameter must point to a BOOL variable that
        ///     receives TRUE if the shadow is enabled, FALSE if it is disabled. This effect appears only if the system has a color
        ///     depth of more than 256 colors.
        /// </summary>
        SPI_GETCURSORSHADOW = 0x101A,

        /// <summary>
        ///     Determines whether the gradient effect for window title bars is enabled. The pvParam parameter must point to a BOOL
        ///     variable that receives TRUE for enabled, or FALSE for disabled. For more information about the gradient effect, see
        ///     the GetSysColor function.
        /// </summary>
        SPI_GETGRADIENTCAPTIONS = 0x1008,

        /// <summary>
        ///     Determines whether hot tracking of user-interface elements, such as menu names on menu bars, is enabled. The
        ///     pvParam parameter must point to a BOOL variable that receives TRUE for enabled, or FALSE for disabled.
        ///     Hot tracking means that when the cursor moves over an item, it is highlighted but not selected. You can query this
        ///     value to decide whether to use hot tracking in the user interface of your application.
        /// </summary>
        SPI_GETHOTTRACKING = 0x100E,

        /// <summary>
        ///     Determines whether the smooth-scrolling effect for list boxes is enabled. The pvParam parameter must point to a
        ///     BOOL variable that receives TRUE for enabled, or FALSE for disabled.
        /// </summary>
        SPI_GETLISTBOXSMOOTHSCROLLING = 0x1006,

        /// <summary>
        ///     Determines whether the menu animation feature is enabled. This master switch must be on to enable menu animation
        ///     effects. The pvParam parameter must point to a BOOL variable that receives TRUE if animation is enabled and FALSE
        ///     if it is disabled.
        ///     If animation is enabled, SPI_GETMENUFADE indicates whether menus use fade or slide animation.
        /// </summary>
        SPI_GETMENUANIMATION = 0x1002,

        /// <summary>
        ///     Same as SPI_GETKEYBOARDCUES.
        /// </summary>
        SPI_GETMENUUNDERLINES = 0x100A,

        /// <summary>
        ///     Determines whether the selection fade effect is enabled. The pvParam parameter must point to a BOOL variable that
        ///     receives TRUE if enabled or FALSE if disabled.
        ///     The selection fade effect causes the menu item selected by the user to remain on the screen briefly while fading
        ///     out after the menu is dismissed.
        /// </summary>
        SPI_GETSELECTIONFADE = 0x1014,

        /// <summary>
        ///     Determines whether ToolTip animation is enabled. The pvParam parameter must point to a BOOL variable that receives
        ///     TRUE if enabled or FALSE if disabled. If ToolTip animation is enabled, SPI_GETTOOLTIPFADE indicates whether
        ///     ToolTips use fade or slide animation.
        /// </summary>
        SPI_GETTOOLTIPANIMATION = 0x1016,

        /// <summary>
        ///     If SPI_SETTOOLTIPANIMATION is enabled, SPI_GETTOOLTIPFADE indicates whether ToolTip animation uses a fade effect or
        ///     a slide effect. The pvParam parameter must point to a BOOL variable that receives TRUE for fade animation or FALSE
        ///     for slide animation. For more information on slide and fade effects, see AnimateWindow.
        /// </summary>
        SPI_GETTOOLTIPFADE = 0x1018,

        /// <summary>
        ///     Determines whether UI effects are enabled or disabled. The pvParam parameter must point to a BOOL variable that
        ///     receives TRUE if all UI effects are enabled, or FALSE if they are disabled.
        /// </summary>
        SPI_GETUIEFFECTS = 0x103E,

        /// <summary>
        ///     Enables or disables the slide-open effect for combo boxes. Set the pvParam parameter to TRUE to enable the gradient
        ///     effect, or FALSE to disable it.
        /// </summary>
        SPI_SETCOMBOBOXANIMATION = 0x1005,

        /// <summary>
        ///     Enables or disables a shadow around the cursor. The pvParam parameter is a BOOL variable. Set pvParam to TRUE to
        ///     enable the shadow or FALSE to disable the shadow. This effect appears only if the system has a color depth of more
        ///     than 256 colors.
        /// </summary>
        SPI_SETCURSORSHADOW = 0x101B,

        /// <summary>
        ///     Enables or disables the gradient effect for window title bars. Set the pvParam parameter to TRUE to enable it, or
        ///     FALSE to disable it. The gradient effect is possible only if the system has a color depth of more than 256 colors.
        ///     For more information about the gradient effect, see the GetSysColor function.
        /// </summary>
        SPI_SETGRADIENTCAPTIONS = 0x1009,

        /// <summary>
        ///     Enables or disables hot tracking of user-interface elements such as menu names on menu bars. Set the pvParam
        ///     parameter to TRUE to enable it, or FALSE to disable it.
        ///     Hot-tracking means that when the cursor moves over an item, it is highlighted but not selected.
        /// </summary>
        SPI_SETHOTTRACKING = 0x100F,

        /// <summary>
        ///     Enables or disables the smooth-scrolling effect for list boxes. Set the pvParam parameter to TRUE to enable the
        ///     smooth-scrolling effect, or FALSE to disable it.
        /// </summary>
        SPI_SETLISTBOXSMOOTHSCROLLING = 0x1007,

        /// <summary>
        ///     Enables or disables menu animation. This master switch must be on for any menu animation to occur. The pvParam
        ///     parameter is a BOOL variable; set pvParam to TRUE to enable animation and FALSE to disable animation.
        ///     If animation is enabled, SPI_GETMENUFADE indicates whether menus use fade or slide animation.
        /// </summary>
        SPI_SETMENUANIMATION = 0x1003,

        /// <summary>
        ///     Same as SPI_SETKEYBOARDCUES.
        /// </summary>
        SPI_SETMENUUNDERLINES = 0x100B,

        /// <summary>
        ///     Set pvParam to TRUE to enable the selection fade effect or FALSE to disable it.
        ///     The selection fade effect causes the menu item selected by the user to remain on the screen briefly while fading
        ///     out after the menu is dismissed. The selection fade effect is possible only if the system has a color depth of more
        ///     than 256 colors.
        /// </summary>
        SPI_SETSELECTIONFADE = 0x1015,

        /// <summary>
        ///     Set pvParam to TRUE to enable ToolTip animation or FALSE to disable it. If enabled, you can use SPI_SETTOOLTIPFADE
        ///     to specify fade or slide animation.
        /// </summary>
        SPI_SETTOOLTIPANIMATION = 0x1017,

        /// <summary>
        ///     If the SPI_SETTOOLTIPANIMATION flag is enabled, use SPI_SETTOOLTIPFADE to indicate whether ToolTip animation uses a
        ///     fade effect or a slide effect. Set pvParam to TRUE for fade animation or FALSE for slide animation. The tooltip
        ///     fade effect is possible only if the system has a color depth of more than 256 colors. For more information on the
        ///     slide and fade effects, see the AnimateWindowfunction.
        /// </summary>
        SPI_SETTOOLTIPFADE = 0x1019,

        /// <summary>
        ///     Enables or disables UI effects. Set the pvParam parameter to TRUE to enable all UI effects or FALSE to disable all
        ///     UI effects.
        /// </summary>
        SPI_SETUIEFFECTS = 0x103F
    }
}