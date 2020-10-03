#region Usings
using System;
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class PlatformAttribute : Attribute
    {
        #region Properties
        public Type ManagerType { get; private set; }
        public string Name { get; private set; }
        public int Priority { get; private set; }
        public OsType RequiredOS { get; private set; }
        #endregion

        protected PlatformAttribute(OsType requiredOperatingSystem,
                                    int priority,
                                    string name) : this(requiredOperatingSystem, priority, name, null) { }

        protected PlatformAttribute(OsType requiredOperatingSystem,
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