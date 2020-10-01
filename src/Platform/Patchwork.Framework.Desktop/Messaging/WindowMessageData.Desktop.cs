using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;

namespace Patchwork.Framework.Messaging
{
    public partial class WindowMessageData
    {
        public PropertyChangingData<NWindowState> StateChangingData { get; set; }

        public PropertyChangedData<NWindowState> StateChangedData { get; set; }

        public PropertyChangingData<NWindowMode> ModeChangingData { get; set; }

        public PropertyChangedData<NWindowMode> ModeChangedData { get; set; }
    }
}
