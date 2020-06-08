#region Usings
using System;
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework.Platform
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyPlatformAttribute : Attribute
    {
        #region Properties
        public Type DispatcherType { get; private set; }
        public Type OperatingSystemType { get; }
        public Type RuntimeType { get; }
        public string Name { get; private set; }
        public int Priority { get; private set; }
        public OperatingSystemType RequiredOS { get; private set; }
        public Type ApplicationType { get; private set; }
        #endregion

        public AssemblyPlatformAttribute(OperatingSystemType requiredOperatingSystem,
                                         int priority,
                                         string name,
                                         Type applicationType,
                                         Type dispatcherType,
                                         Type operatingSystemType = null,
                                         Type runtimeType = null)
        {
            Name = name;
            ApplicationType = applicationType;
            DispatcherType = dispatcherType;
            OperatingSystemType = operatingSystemType;
            RuntimeType = runtimeType;
            RequiredOS = requiredOperatingSystem;
            Priority = priority;
        }
    }
}