#region Usings
using System;
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework
{
    public sealed class AssemblyPlatformAttribute : PlatformAttribute
    {
        #region Properties
        public Type ApplicationType { get; private set; }
        public Type DispatcherType { get; private set; }
        public Type OperatingSystemType { get; }
        public Type RuntimeType { get; }
        #endregion

        public AssemblyPlatformAttribute(OsType requiredOperatingSystem,
                                         int priority,
                                         string name,
                                         Type applicationType,
                                         Type dispatcherType,
                                         Type operatingSystemType = null,
                                         Type runtimeType = null)
            : base(requiredOperatingSystem, priority, name)
        {
            if (operatingSystemType == null)
                operatingSystemType = typeof(OSInformation);

            if (runtimeType == null)
                runtimeType = typeof(RuntimeInformation);

            ApplicationType = applicationType;
            DispatcherType = dispatcherType;
            OperatingSystemType = operatingSystemType;
            RuntimeType = runtimeType;
        }
    }
}