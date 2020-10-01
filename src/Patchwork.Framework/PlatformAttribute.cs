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
        public OSType RequiredOS { get; private set; }
        public Type ManagerType { get; private set; }
        #endregion

        protected PlatformAttribute(OSType requiredOperatingSystem,
                                    int priority,
                                    string name) : this(requiredOperatingSystem, priority, name, null) { }

        protected PlatformAttribute(OSType requiredOperatingSystem,
                                    int priority,
                                    string name,
                                    Type managerType)
        {
            Name = name;
            RequiredOS = requiredOperatingSystem;
            Priority = priority;
            ManagerType = managerType;
        }
    }
}