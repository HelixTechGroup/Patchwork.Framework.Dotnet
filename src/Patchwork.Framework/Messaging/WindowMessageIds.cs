using System;
using System.Collections.Generic;
using System.Text;

namespace Patchwork.Framework.Messaging
{
    public enum WindowMessageIds
    {
        None,
        Created,
        Destroyed,
        Shown,
        Hidden,
        //SDL_WINDOWEVENT_EXPOSED,
        //Window has been exposed and should be redrawn
        Moving,
        Moved,
        Resizing,
        Resized,
        //SDL_WINDOWEVENT_SIZE_CHANGED,
        //The window size has changed, either as a result of an API call or through the system or user changing the window size. */
        Minimized,
        Maximized,
        Restored,
        Focused,
        Unfocused,
        //SDL_WINDOWEVENT_ENTER,
        //Window has gained mouse focus
        //SDL_WINDOWEVENT_LEAVE,
        //Window has lost mouse focus
        //SDL_WINDOWEVENT_FOCUS_GAINED,
        //Window has gained keyboard focus
        //SDL_WINDOWEVENT_FOCUS_LOST,
        //Window has lost keyboard focus
        Closing,
        Closed,
        //SDL_WINDOWEVENT_TAKE_FOCUS,
        //Window is being offered a focus (should SetWindowInputFocus() on itself or a subwindow, or ignore)
        HitTest,
        Activating,
        Activated,
        Deactivating,
        Deactivated,
        AttentionDrawn
    }
}
