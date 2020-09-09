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
        public static RenderManager RenderManager { get; private set; }
        #endregion

        #region Methods
        static partial void InitializeResourcesShared()
        {
            if (IsInitialized)
                return;

            RenderManager.Initialize();
            ProcessMessage += OnProcessRenderMessage;
        }

        static partial void DisposeManagedResourcesShared()
        {
            RenderManager.Dispose();
        }

        static partial void CreateResourcesShared()
        {
            RenderManager = new RenderManager();
            RenderManager.Create();
        }

        static partial void RunResourcesShared(CancellationToken token)
        {
            RenderManager.Run(token);
        }

        private static void OnProcessRenderMessage(IPlatformMessage message) 
        {
            Logger.LogDebug("Found Messages.");
            switch (message.Id)
            {
                case MessageIds.Quit:
                    break;
            }
        }
        #endregion
    }
}