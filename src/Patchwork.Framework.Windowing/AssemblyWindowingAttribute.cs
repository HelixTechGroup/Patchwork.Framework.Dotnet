#region Usings
using System;
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyWindowingAttribute : PlatformAttribute
    {
        public Type WindowType { get; private set; }

        /// <inheritdoc />
        public AssemblyWindowingAttribute(OsType requiredOperatingSystem,
                                          int priority,
                                          string name,
                                          Type windowType)
            : base(requiredOperatingSystem, priority, name)
        {
            WindowType = windowType;
        }
    }
}