#region Usings
using System;
using System.Linq;
using SysRuntime = System.Runtime.InteropServices.RuntimeInformation;
using SysEnv = System.Environment;
#endregion

namespace Patchwork.Framework.Environment
{
    public class RuntimeInformation : IRuntimeInformation
    {
        #region Members
        protected RuntimeType m_runtimeType;
        protected Version m_runtimeVersion;
        #endregion

        #region Properties
        public RuntimeType Runtime
        {
            get { return m_runtimeType; }
        }

        public Version RuntimeVersion
        {
            get { return m_runtimeVersion; }
        }
        #endregion

        #region Methods
        public virtual void DetectRuntime()
        {
            var runtimeName = SysRuntime.FrameworkDescription;
            var types = new[] {".NET Core", ".NET Framework"};
            var test = types.FirstOrDefault(s => runtimeName.Contains(s));

            switch (test)
            {
                case ".NET Core":
                    m_runtimeType = RuntimeType.CoreCLR;
                    break;
                case ".NET Framework":
                    m_runtimeType = RuntimeType.CLR;
                    break;
                default:
                    m_runtimeType = RuntimeType.Unknown;
                    break;
            }

            if (Type.GetType("Mono.Runtime") != null
             || Type.GetType("Mono.Interop.IDispatch", false) != null)
                m_runtimeType = RuntimeType.Mono;

            m_runtimeVersion = SysEnv.Version;
        }
        #endregion
    }
}