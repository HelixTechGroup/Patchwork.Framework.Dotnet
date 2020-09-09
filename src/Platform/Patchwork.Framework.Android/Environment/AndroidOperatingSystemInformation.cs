using Android.OS;
using ver = System.Version;

namespace Patchwork.Framework.Environment
{
	public class AndroidOperatingSystemInformation : OperatingSystemInformation
	{
	    protected override void GetOsDetails()
	    {
	        base.GetOsDetails();
            m_platform = PlatformType.Mobile;
            m_type = OperatingSystemType.Android;
            m_name = Build.VERSION.SdkInt.ToString();
            m_version = ver.Parse(Build.VERSION.Release);
        }
	}
}