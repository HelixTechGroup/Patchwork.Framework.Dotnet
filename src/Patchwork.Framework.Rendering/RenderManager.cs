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
    public abstract class RenderManager : PlatformManager<AssemblyRenderingAttribute, IPlatformMessage<IWindowMessageData>>
    {
        protected IList<INativeRenderDevice> m_devices;

        protected override void InitializeResources()
        {
            base.InitializeResources();

            if (m_isInitialized)
                return;

            foreach (var device in m_devices)
                device.Initialize();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            foreach (var device in m_devices)
                device.Dispose();

            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void RunManager()
        {
            base.RunManager();
        }

        /// <inheritdoc />
        protected override void CreateManager(params AssemblyRenderingAttribute[] managers)
        {
            m_devices = new ConcurrentList<INativeRenderDevice>();
            foreach (var m in managers)
            {
                if (m.RenderDeviceType == null)
                    continue;

                if (m_devices.All(d => d.GetType() != m.RenderDeviceType))
                    m_devices.Add(Activator.CreateInstance(m.RenderDeviceType) as INativeRenderDevice);
            }
        }

        protected override void OnProcessMessage(IPlatformMessage<IWindowMessageData> message)
        {
            var data = message.Data;
            switch (data.MessageId)
            {
                
            }
        }
    }
}