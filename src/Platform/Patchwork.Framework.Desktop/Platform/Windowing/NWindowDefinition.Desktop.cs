#region Usings
using System.Drawing;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public partial struct NWindowDefinition
    {
        #region Members
        private NWindowActivationPolicy m_activationPolicy;
        private NWindowMode m_initialMode;
        private NWindowState m_initialState;
        private readonly NWindowSizeLimits m_sizeLimits;
        private NWindowDecorations m_supportedDecorations;
        private NWindowTransparency m_transparencySupport;
        #endregion

        #region Properties
        public NWindowActivationPolicy ActivationPolicy
        {
            get { return m_activationPolicy; }
            set { m_activationPolicy = value; }
        }

        public static NWindowDefinition Default
        {
            get
            {
                return new NWindowDefinition
                       {
                           AcceptsInput = true,
                           ActivationPolicy = NWindowActivationPolicy.FirstShown,
                           DesiredSize = new Size(800, 600),
                           IsRegularWindow = true,
                           Title = "test",
                           SupportedDecorations = NWindowDecorations.All,
                           Type = NWindowType.Normal,
                           IsRenderable = true,
                           IsMainApplicationWindow = true
                       };
            }
        }

        public NWindowMode InitialMode
        {
            get { return m_initialMode; }
            set { m_initialMode = value; }
        }

        //bool ManualDPI;

        public NWindowState InitialState
        {
            get { return m_initialState; }
            set { m_initialState = value; }
        }

        //bool SizeWillChangeOften;
        public NWindowDecorations SupportedDecorations
        {
            get { return m_supportedDecorations; }
            set { m_supportedDecorations = value; }
        }

        public NWindowTransparency TransparencySupport
        {
            get { return m_transparencySupport; }
            set { m_transparencySupport = value; }
        }
        #endregion
    }
}