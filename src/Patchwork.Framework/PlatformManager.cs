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
    public partial class PlatformManager<TManager> : Initializable, IPlatformManager<TManager> where TManager : AssemblyPlatformAttribute
    {
        public event ProcessMessageHandler ProcessMessage;

        public event Action Startup;

        public event Action Shutdown;

        #region Members
        //private PlatformManager m_platform;
        //private CancellationTokenSource m_tokenSource;
        //private CancellationToken m_token;
        //private IoCContainer m_container;
        private Task m_runTask;
        #endregion

        #region Properties
        public bool IsRunning { get; private set; }

        public PlatformMessagePump MessagePump { get; private set; }
        #endregion

        #region Methods
        /// <inheritdoc />
        protected override void InitializeResources()
        { 
            if (m_isInitialized)
                return;

            //m_container.CreateChildContainer();            
            MessagePump.Initialize();
            ProcessMessage += OnProcessMessage;
            //m_token = token;
            //m_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(m_token);   
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_runTask?.ConfigureAwait(false);
            m_runTask?.Dispose();
            MessagePump.Dispose();
            //Application.CloseConsole();
            //ProcessMessage.Dispose();
        }

        public void Run(CancellationToken token)
        {
            if (!m_isInitialized)
                Throw.Exception<InvalidOperationException>();

            token.Register(() => Core.Dispatcher.Signal(NativeThreadDispatcherPriority.Send));
            var tasks = new ConcurrentList<Task>();
            Startup?.Invoke();
            IsRunning = true;
            Core.Logger.LogDebug("Pumping Render Messages.");
            while (!token.IsCancellationRequested)
            {
                while (MessagePump.Poll(out var e, token))
                {
                    var message = e;
                    tasks.Add(Task.Run(() => ProcessMessage?.Invoke(message)).ContinueWith((t) => tasks.Remove(t)));
                }

                RunManager();
            }

            var whenAll = Task.WhenAll(tasks);
            Task.WhenAll(whenAll).ConfigureAwait(false);

            for (; ; )
            {
                while (!whenAll.IsCompleted)
                {
                    Console.Write(".");
                    Thread.Sleep(500);
                }
                break;
            }

            Core.Logger.LogDebug("Exit Pumping Render Messages.");
            IsRunning = false;
            Shutdown?.Invoke();
        }

        protected virtual void RunManager() { }

        public void RunAsync(CancellationToken token)
        {
            m_runTask = new Task(() => { Run(token); });//Task.Run(() => { Run(token); });
            //.ContinueWith((t) => { Dispose(); })
            //m_runTask.ConfigureAwait(false);
            m_runTask.Start();
        }

        public void Create()
        {
            var os = Core.Environment.OperatingSystem;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                      .Where(a => Attribute.IsDefined(a, typeof(TManager))).ToArray();

            var platform = assemblies
                          .Where(a => Attribute.IsDefined(a, typeof(TManager)))
                          .GetAssemblyAttribute<TManager>()
                          .Where(attribute => attribute.RequiredOS == os.Type)
                          .OrderBy(attribute => attribute.Priority);

            if (platform == null || platform.IsEmpty())
                throw new InvalidOperationException("No platform found. Are you missing assembly references?");

            CreateManager(platform.ToArray());
            MessagePump = new PlatformMessagePump(Core.Logger, Core.Application);

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

        protected virtual void CreateManager(params TManager[] managers) { }
        #endregion
    }
}