#region Usings
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Environment;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Shield.Framework.IoC.Native.DependencyInjection;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.Logging.Loggers;
using Shin.Framework.Logging.Native;
using RuntimeInformation = Patchwork.Framework.Environment.RuntimeInformation;
using SysEnv = System.Environment;
#endregion

namespace Patchwork.Framework
{
    public partial class RenderManager : PlatformManager<AssemblyPlatformRenderingAttribute>
    {
        protected override void InitializeResources()
        { 
            if (m_isInitialized)
                return;

            ProcessMessage += OnProcessMessage;
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
        }

        /// <inheritdoc />
        protected override void RunManager()
        {
            base.RunManager();
        }

        /// <inheritdoc />
        protected override void CreateManager(params AssemblyPlatformRenderingAttribute[] managers)
        {
            base.CreateManager(managers);

            foreach (var m in managers)
            {
                var runtimeInfo = m.RuntimeType == null
                                      ? new RuntimeInformation()
                                      : Activator.CreateInstance(m.RuntimeType) as IRuntimeInformation;
            }
        }

        private void OnProcessMessage(IPlatformMessage message) 
        {
            Core.Logger.LogDebug("Found Messages.");
            switch (message.Id)
            {
                case MessageIds.Quit:
                    break;
            }
        }
    }
}