using System;
using Android.OS;
using ver = System.Version;

namespace Patchwork.Framework.Environment
{
	public class AndroidOSInformation : OSInformation
	{
	    protected override void GetOsDetails()
	    {
	        base.GetOsDetails();
            m_platform = PlatformType.Mobile;
            m_type = OsType.Android;
            m_name = Build.VERSION.SdkInt.ToString();
            if (!ver.TryParse(Build.VERSION.Release, out m_version))
                m_version = new ver(Int32.Parse(Build.VERSION.Release), 0);
        }
	}
}