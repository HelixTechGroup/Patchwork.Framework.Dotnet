#region Usings
using System;
using System.Runtime.InteropServices;
using SysRuntime = System.Runtime.InteropServices.RuntimeInformation;
using SysEnv = System.Environment;
#endregion

namespace Patchwork.Framework.Environment
{
    public class OSInformation : IOsInformation
    {
        #region Members
        protected PlatformID m_id;
        protected bool m_is64Bit;
        protected bool m_isUnixBased;
        protected string m_name;
        protected PlatformType m_platform;
        protected OsType m_type;
        protected Version m_version;
        #endregion

        #region Properties
        public bool Is64Bit
        {
            get { return m_is64Bit; }
        }

        public bool IsUnixBased
        {
            get { return m_isUnixBased; }
        }

        public string Name
        {
            get { return m_name; }
        }

        public PlatformType Platform
        {
            get { return m_platform; }
        }

        public OsType Type
        {
            get { return m_type; }
        }

        public Version Version
        {
            get { return m_version; }
        }
        #endregion

        #region Methods
        public virtual void DetectOperatingSystem()
        {
            GetOsType();
            GetOsDetails();
        }

        /// <inheritdoc />
        public bool IsType(OsType value)
        {
            return m_type == value;
        }

        protected virtual void GetOsDetails()
        {
            m_is64Bit = SysRuntime.OSArchitecture.HasFlag(Architecture.X64) || SysRuntime.OSArchitecture.HasFlag(Architecture.Arm64);
            m_version = SysEnv.OSVersion.Version;
            m_name = SysRuntime.OSDescription;
        }

        protected virtual void GetOsType()
        {
            m_platform = PlatformType.Desktop;
            m_id = SysEnv.OSVersion.Platform;
            switch ((int)m_id)
            {
                case 6: // PlatformID.MacOSX:
                    m_type = OsType.MacOS;
                    m_isUnixBased = true;
                    break;
                case 4: // PlatformID.Unix:	
                    m_type = OsType.Unix;
                    m_isUnixBased = true;
                    break;
                case 0: // PlatformID.Win32S:
                case 1: // PlatformID.Win32Windows:
                case 2: // PlatformID.Win32NT:
                case 3: // PlatformID.WinCE:
                    m_type = OsType.Windows;
                    break;
                default:
                    m_type = OsType.Unknown;
                    m_platform = PlatformType.Unknown;
                    break;
            }
        }
        #endregion
    }
}