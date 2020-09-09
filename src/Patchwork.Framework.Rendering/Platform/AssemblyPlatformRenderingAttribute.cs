#region Usings
using System;
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework.Platform
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyPlatformRenderingAttribute : AssemblyPlatformAttribute
    {
        /// <inheritdoc />
        public AssemblyPlatformRenderingAttribute(OperatingSystemType requiredOperatingSystem, int priority, string name, Type applicationType, Type dispatcherType, Type operatingSystemType = null, Type runtimeType = null) : base(requiredOperatingSystem, priority, name, applicationType, dispatcherType, operatingSystemType, runtimeType) { }
    }
}