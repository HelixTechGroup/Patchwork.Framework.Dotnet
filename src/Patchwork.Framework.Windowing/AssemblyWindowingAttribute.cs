#region Usings
using System;
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework.Platform
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyWindowingAttribute : PlatformAttribute
    {
        /// <inheritdoc />
        public AssemblyWindowingAttribute(OSType requiredOperatingSystem, 
                                          int priority, 
                                          string name,
                                          Type managerType  = null) 
            : base(requiredOperatingSystem, priority, name, managerType)
        {
        }
    }
}