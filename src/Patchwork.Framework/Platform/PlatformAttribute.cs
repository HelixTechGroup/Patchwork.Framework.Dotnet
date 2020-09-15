#region Usings
using System;
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework.Platform
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class PlatformAttribute : Attribute
    {
        #region Properties
        public string Name { get; private set; }
        public int Priority { get; private set; }
        public OperatingSystemType RequiredOS { get; private set; }
        public Type ManagerType { get; private set; }
        #endregion

        public PlatformAttribute(OperatingSystemType requiredOperatingSystem,
                                         int priority,
                                         string name,
                                         Type managerType = null)
        {
            Name = name;
            RequiredOS = requiredOperatingSystem;
            Priority = priority;
            ManagerType = managerType;
        }
    }
}