using System;
using System.Collections.Generic;
using System.Text;
using Patchwork.Framework.Environment;

namespace Patchwork.Framework.Platform
{
    public sealed class AssemblyPlatformAttribute : PlatformAttribute
    {
        public Type ApplicationType { get; private set; }
        public Type DispatcherType { get; private set; }
        public Type OperatingSystemType { get; }
        public Type RuntimeType { get; }

        public AssemblyPlatformAttribute(OperatingSystemType requiredOperatingSystem,
                                 int priority,
                                 string name,
                                 Type applicationType,
                                 Type dispatcherType,
                                 Type operatingSystemType = null,
                                 Type runtimeType = null) 
        : base(requiredOperatingSystem, priority, name)
        {
            ApplicationType = applicationType;
            DispatcherType = dispatcherType;
            OperatingSystemType = operatingSystemType;
            RuntimeType = runtimeType;
        }
    }
}
