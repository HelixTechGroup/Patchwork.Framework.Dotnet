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

        public IWindowMessageData StateChanged(NWindowState requestedState)
        {
            switch (requestedState)
            {
                case NWindowState.Maximized:
                    m_messageId = WindowMessageIds.Maximized;
                    break;
                case NWindowState.Minimized:
                    m_messageId = WindowMessageIds.Minimized;
                    break;
                case NWindowState.Restored:
                    m_messageId = WindowMessageIds.Restored;
                    break;
                default:
                    m_messageId = WindowMessageIds.StateChanged;
                    break;
            }

            var data = m_window.GetDataCache();
            StateChangedData = new PropertyChangedData<NWindowState>(m_window.State, requestedState, data.PreviousState);
            return this;
        }
    }
}