#region Usings
using System;
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework.Platform
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyRenderingAttribute : PlatformAttribute
    {
        public Type RenderDeviceType { get; }

        public Type RenderAdapterType { get; }

        /// <inheritdoc />
        public AssemblyRenderingAttribute(OperatingSystemType requiredOperatingSystem, 
                                          int priority, 
                                          string name,
                                          Type renderDevice = null,
                                          Type renderAdapater = null) 
            : base(requiredOperatingSystem, priority, name)
        {
            RenderDeviceType = renderDevice;
            RenderAdapterType = renderAdapater;
        }
    }
}