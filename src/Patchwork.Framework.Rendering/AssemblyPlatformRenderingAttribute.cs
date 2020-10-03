#region Usings
using System;
using Patchwork.Framework.Environment;
#endregion

namespace Patchwork.Framework
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyRenderingAttribute : PlatformAttribute
    {
        #region Properties
        public Type RenderDeviceType { get; private set; }
        #endregion

        /// <inheritdoc />
        public AssemblyRenderingAttribute(OsType requiredOperatingSystem,
                                          int priority,
                                          string name,
                                          Type managerType = null,
                                          Type renderDevice = null)
            : base(requiredOperatingSystem, priority, name, managerType)
        {
            RenderDeviceType = renderDevice;
        }
    }
}