#region Usings
using System.Drawing;
#endregion

namespace Patchwork.Framework.Platform.Window
{
    public partial struct NativeWindowDefinition
    {
        #region Members
        private NativeWindowActivationPolicy m_activationPolicy;
        private NativeWindowSizeLimits m_sizeLimits;
        private NativeWindowState m_initialState;
        private NativeWindowMode m_initialMode;
        private NativeWindowTransparency m_transparencySupport;
        private NativeWindowDecorations m_supportedDecorations;
        #endregion

        #region Properties
        public NativeWindowActivationPolicy ActivationPolicy
        {
            get { return m_activationPolicy; }
            set { m_activationPolicy = value; }
        }

        //bool SizeWillChangeOften;
        public NativeWindowDecorations SupportedDecorations
        {
            get { return m_supportedDecorations; }
            set { m_supportedDecorations = value; }
        }

        public NativeWindowTransparency TransparencySupport
        {
            get { return m_transparencySupport; }
            set { m_transparencySupport = value; }
        }

        //bool ManualDPI;

        public NativeWindowState InitialState
        {
            get { return m_initialState; }
            set { m_initialState = value; }
        }

        public NativeWindowMode InitialMode
        {
            get { return m_initialMode; }
            set { m_initialMode = value; }
        }

        public static NativeWindowDefinition Default
        {
            get
            {
                return new NativeWindowDefinition
                          {
                              AcceptsInput = true,
                              ActivationPolicy = NativeWindowActivationPolicy.FirstShown,
                              DesiredSize = new Size(800, 600),
                              IsRegularWindow = true,
                              Title = "test",
                              SupportedDecorations = NativeWindowDecorations.All,
                              Type = NativeWindowType.Normal,
                              IsMainApplicationWindow = true
                          };
            }
        }
        #endregion
    }
}