#region Usings
using System;
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyWindowingAttribute : PlatformAttribute
    {
        /// <inheritdoc />
        public AssemblyWindowingAttribute(OsType requiredOperatingSystem,
                                          int priority,
                                          string name,
                                          Type managerType = null)
            : base(requiredOperatingSystem, priority, name, managerType) { }
    }
}