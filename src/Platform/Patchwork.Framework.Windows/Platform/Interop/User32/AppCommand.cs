namespace Patchwork.Framework.Platform.Interop.User32 {
    public enum AppCommand
    {
        /// <summary>
        ///     Toggle the bass boost on and off.
        /// </summary>
        APPCOMMAND_BASS_BOOST = 20,

        /// <summary>
        ///     Decrease the bass.
        /// </summary>
        APPCOMMAND_BASS_DOWN = 19,

        /// <summary>
        ///     Increase the bass.
        /// </summary>
        APPCOMMAND_BASS_UP = 21,

        /// <summary>
        ///     Navigate backward.
        /// </summary>
        APPCOMMAND_BROWSER_BACKWARD = 1,

        /// <summary>
        ///     Open favorites.
        /// </summary>
        APPCOMMAND_BROWSER_FAVORITES = 6,

        /// <summary>
        ///     Navigate forward.
        /// </summary>
        APPCOMMAND_BROWSER_FORWARD = 2,

        /// <summary>
        ///     Navigate home.
        /// </summary>
        APPCOMMAND_BROWSER_HOME = 7,

        /// <summary>
        ///     Refresh page.
        /// </summary>
        APPCOMMAND_BROWSER_REFRESH = 3,

        /// <summary>
        ///     Open search.
        /// </summary>
        APPCOMMAND_BROWSER_SEARCH = 5,

        /// <summary>
        ///     Stop download.
        /// </summary>
        APPCOMMAND_BROWSER_STOP = 4,

        /// <summary>
        ///     Close the window (not the application).
        /// </summary>
        APPCOMMAND_CLOSE = 31,

        /// <summary>
        ///     Copy the selection.
        /// </summary>
        APPCOMMAND_COPY = 36,

        /// <summary>
        ///     Brings up the correction list when a word is incorrectly identified during speech input.
        /// </summary>
        APPCOMMAND_CORRECTION_LIST = 45,

        /// <summary>
        ///     Cut the selection.
        /// </summary>
        APPCOMMAND_CUT = 37,

        /// <summary>
        ///     Toggles between two modes of speech input: dictation and command/control (giving commands to an application or
        ///     accessing menus).
        /// </summary>
        APPCOMMAND_DICTATE_OR_COMMAND_CONTROL_TOGGLE = 43,

        /// <summary>
        ///     Open the Find dialog.
        /// </summary>
        APPCOMMAND_FIND = 28,

        /// <summary>
        ///     Forward a mail message.
        /// </summary>
        APPCOMMAND_FORWARD_MAIL = 40,

        /// <summary>
        ///     Open the Help dialog.
        /// </summary>
        APPCOMMAND_HELP = 27,

        /// <summary>
        ///     Start App1.
        /// </summary>
        APPCOMMAND_LAUNCH_APP1 = 17,

        /// <summary>
        ///     Start App2.
        /// </summary>
        APPCOMMAND_LAUNCH_APP2 = 18,

        /// <summary>
        ///     Open mail.
        /// </summary>
        APPCOMMAND_LAUNCH_MAIL = 15,

        /// <summary>
        ///     Go to Media Select mode.
        /// </summary>
        APPCOMMAND_LAUNCH_MEDIA_SELECT = 16,

        /// <summary>
        ///     Decrement the channel value, for example, for a TV or radio tuner.
        /// </summary>
        APPCOMMAND_MEDIA_CHANNEL_DOWN = 52,

        /// <summary>
        ///     Increment the channel value, for example, for a TV or radio tuner.
        /// </summary>
        APPCOMMAND_MEDIA_CHANNEL_UP = 51,

        /// <summary>
        ///     Increase the speed of stream playback. This can be implemented in many ways, for example, using a fixed speed or
        ///     toggling through a series of increasing speeds.
        /// </summary>
        APPCOMMAND_MEDIA_FAST_FORWARD = 49,

        /// <summary>
        ///     Go to next track.
        /// </summary>
        APPCOMMAND_MEDIA_NEXTTRACK = 11,

        /// <summary>
        ///     Pause. If already paused, take no further action. This is a direct PAUSE command that has no state. If there are
        ///     discrete Play and Pause buttons, applications should take action on this command as well as
        ///     APPCOMMAND_MEDIA_PLAY_PAUSE.
        /// </summary>
        APPCOMMAND_MEDIA_PAUSE = 47,

        /// <summary>
        ///     Begin playing at the current position. If already paused, it will resume. This is a direct PLAY command that has no
        ///     state. If there are discrete Play and Pause buttons, applications should take action on this command as well as
        ///     APPCOMMAND_MEDIA_PLAY_PAUSE.
        /// </summary>
        APPCOMMAND_MEDIA_PLAY = 46,

        /// <summary>
        ///     Play or pause playback. If there are discrete Play and Pause buttons, applications should take action on this
        ///     command as well as APPCOMMAND_MEDIA_PLAY and APPCOMMAND_MEDIA_PAUSE.
        /// </summary>
        APPCOMMAND_MEDIA_PLAY_PAUSE = 14,

        /// <summary>
        ///     Go to previous track.
        /// </summary>
        APPCOMMAND_MEDIA_PREVIOUSTRACK = 12,

        /// <summary>
        ///     Begin recording the current stream.
        /// </summary>
        APPCOMMAND_MEDIA_RECORD = 48,

        /// <summary>
        ///     Go backward in a stream at a higher rate of speed. This can be implemented in many ways, for example, using a fixed
        ///     speed or toggling through a series of increasing speeds.
        /// </summary>
        APPCOMMAND_MEDIA_REWIND = 50,

        /// <summary>
        ///     Stop playback.
        /// </summary>
        APPCOMMAND_MEDIA_STOP = 13,

        /// <summary>
        ///     Toggle the microphone.
        /// </summary>
        APPCOMMAND_MIC_ON_OFF_TOGGLE = 44,

        /// <summary>
        ///     Increase microphone volume.
        /// </summary>
        APPCOMMAND_MICROPHONE_VOLUME_DOWN = 25,

        /// <summary>
        ///     Mute the microphone.
        /// </summary>
        APPCOMMAND_MICROPHONE_VOLUME_MUTE = 24,

        /// <summary>
        ///     Decrease microphone volume.
        /// </summary>
        APPCOMMAND_MICROPHONE_VOLUME_UP = 26,

        /// <summary>
        ///     Create a new window.
        /// </summary>
        APPCOMMAND_NEW = 29,

        /// <summary>
        ///     Open a window.
        /// </summary>
        APPCOMMAND_OPEN = 30,

        /// <summary>
        ///     Paste
        /// </summary>
        APPCOMMAND_PASTE = 38,

        /// <summary>
        ///     Print current document.
        /// </summary>
        APPCOMMAND_PRINT = 33,

        /// <summary>
        ///     Redo last action.
        /// </summary>
        APPCOMMAND_REDO = 35,

        /// <summary>
        ///     Reply to a mail message.
        /// </summary>
        APPCOMMAND_REPLY_TO_MAIL = 39,

        /// <summary>
        ///     Save current document.
        /// </summary>
        APPCOMMAND_SAVE = 32,

        /// <summary>
        ///     Send a mail message.
        /// </summary>
        APPCOMMAND_SEND_MAIL = 41,

        /// <summary>
        ///     Initiate a spell check.
        /// </summary>
        APPCOMMAND_SPELL_CHECK = 42,

        /// <summary>
        ///     Decrease the treble.
        /// </summary>
        APPCOMMAND_TREBLE_DOWN = 22,

        /// <summary>
        ///     Increase the treble.
        /// </summary>
        APPCOMMAND_TREBLE_UP = 23,

        /// <summary>
        ///     Undo last action.
        /// </summary>
        APPCOMMAND_UNDO = 34,

        /// <summary>
        ///     Lower the volume.
        /// </summary>
        APPCOMMAND_VOLUME_DOWN = 9,

        /// <summary>
        ///     Mute the volume.
        /// </summary>
        APPCOMMAND_VOLUME_MUTE = 8,

        /// <summary>
        ///     Raise the volume.
        /// </summary>
        APPCOMMAND_VOLUME_UP = 10
    }
}