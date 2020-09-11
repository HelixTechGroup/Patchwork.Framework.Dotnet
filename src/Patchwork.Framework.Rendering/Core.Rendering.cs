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
    public static partial class Core
    {
        #region Properties
        public static RenderManager RenderManager { get { return (RenderManager)m_managers[typeof(RenderManager)].Value; } }
        #endregion

        #region Methods
        static partial void InitializeResourcesShared()
        {
            if (IsInitialized)
                return;
        }

        static partial void DisposeManagedResourcesShared()
        {
            m_managers.Remove(typeof(RenderManager), out var manager);
            manager?.Value.Dispose();
        }

        static partial void CreateResourcesShared()
        {
            var manager = new RenderManager();
            m_managers[typeof(RenderManager)] = new Lazy<IPlatformManager>(manager);
        }

        static partial void PreRunResourcesShared(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            return;
        }

        static partial void PostRunResourcesShared(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            return;
        }
        #endregion
    }
}