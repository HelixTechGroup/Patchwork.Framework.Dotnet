﻿namespace Patchwork.Framework.Environment
{
    public class WinOSInformation : OSInformation
    {
        #region Methods
        protected override void GetOsDetails()
        {
            base.GetOsDetails();

            var ver = new OperatingSystemVersionDetection(m_version, m_id);
            m_name = string.Format("{0} {1}", ver.Name, ver.Edition);
            m_version = ver.Version;
        }
        #endregion
    }
}