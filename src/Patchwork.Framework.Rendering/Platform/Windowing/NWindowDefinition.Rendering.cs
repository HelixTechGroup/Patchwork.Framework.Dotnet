#region Usings
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public partial struct NWindowDefinition
    {
        #region Members
        private bool m_isRenderable;
        #endregion

        #region Properties
        public bool IsRenderable
        {
            get { return m_isRenderable; }
            set { m_isRenderable = value; }
        }
        #endregion
    }
}