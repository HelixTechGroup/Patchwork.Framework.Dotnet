#region Usings
#endregion

using System;
using Patchwork.Framework.Environment;

namespace Patchwork.Framework.Runtime
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
            //operatingSystemType ??= typeof(OSInformation);
            //runtimeType ??= typeof(RuntimeInformation);

            ApplicationType = applicationType;
            DispatcherType = dispatcherType;
            OperatingSystemType = operatingSystemType ??= typeof(OSInformation);
            RuntimeType = runtimeType ??= typeof(RuntimeInformation);
        }
    }
}