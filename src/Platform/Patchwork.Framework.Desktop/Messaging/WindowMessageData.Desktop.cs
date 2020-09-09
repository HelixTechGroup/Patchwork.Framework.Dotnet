using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Window;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Messaging
{
    public partial class WindowMessageData
    {
        public PropertyChangingData<NativeWindowState> StateChangingData { get; set; }

        public PropertyChangedData<NativeWindowState> StateChangedData { get; set; }

        public PropertyChangingData<NativeWindowMode> ModeChangingData { get; set; }

        public PropertyChangedData<NativeWindowMode> ModeChangedData { get; set; }
    }
}
