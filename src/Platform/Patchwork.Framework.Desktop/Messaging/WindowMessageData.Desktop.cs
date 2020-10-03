#region Usings
using Patchwork.Framework.Platform.Windowing;
#endregion

namespace Patchwork.Framework.Messaging
{
    public partial class WindowMessageData
    {
        #region Properties
        public PropertyChangedData<NWindowMode> ModeChangedData { get; set; }

        public PropertyChangingData<NWindowMode> ModeChangingData { get; set; }

        public PropertyChangedData<NWindowState> StateChangedData { get; set; }
        public PropertyChangingData<NWindowState> StateChangingData { get; set; }
        #endregion
    }
}