#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
#endregion

namespace Patchwork.Framework.Manager
{
    public abstract class PlatformManager : Creatable, IPlatformManager
    {
        protected bool m_isRunning;
        protected bool m_isPumping;
        protected MessageIds[] m_supportedMessages;
        protected IPlatformMessagePump m_pump;
        protected Task m_runTask;
        protected MessageIds[] m_supportedMessageIds;
        protected IList<Task> m_tasks;

        /// <inheritdoc />
        public event Action Shutdown;

        public event ProcessMessageHandler ProcessMessage;

        /// <inheritdoc />
        public event Action Startup;

        /// <inheritdoc />
        public bool IsRunning
        {
            get { return m_isRunning; }
        }

        /// <inheritdoc />
        public IPlatformMessagePump MessagePump
        {
            get { return m_pump; }
        }

        /// <inheritdoc />
        public MessageIds[] SupportedMessages
        {
            get { return m_supportedMessages; }
        }

        /// <inheritdoc />
        public void Pump(CancellationToken token)
        {
            if ((!m_isInitialized | token.IsCancellationRequested) ^ m_isPumping)
                return;
            //Throw.Exception<InvalidOperationException>();

            //if (token.IsCancellationRequested)
            //    return;

            //if (m_isPumping)
            //    return;
                //Wait();
                //Throw.Exception<InvalidOperationException>();

            m_isPumping = true;
            //Core.Logger.LogDebug("Pumping Manager Messages.");

            while (m_pump.Poll(out var e, token))
            {
                var message = e as IPlatformMessage;
                if (m_supportedMessageIds.All(i => i != (message?.Id)))
                    continue;

                m_tasks.Add(Task.Run(() => ProcessMessage?.Invoke(message)).ContinueWith(t => m_tasks.Remove(t)));
            }

            RunManager(token);

            //Core.Logger.LogDebug("Exit Pumping Manager Messages.");
            m_isPumping = false;
        }

        public void Wait()
        {
            if (!m_isInitialized)
                Throw.Exception<InvalidOperationException>();

            WaitManager();
            var whenAll = Task.WhenAll(m_tasks);
            Task.WhenAll(whenAll).ConfigureAwait(false);

            for (;;)
            {
                while (!whenAll.IsCompleted)
                {
                    Console.Write(".");
                    Thread.Sleep(500);
                }

                break;
            }

            m_tasks.Clear();
        }

        public void Run(CancellationToken token)
        {
            if (!m_isInitialized)
                Throw.Exception<InvalidOperationException>();

            if (m_isRunning)
                return;
            //    Wait();
                //Throw.Exception<InvalidOperationException>();

            m_isRunning = true;
            //RunManager(token);
            //Core.Logger.LogDebug("Pumping Manager Messages.");
            while (!token.IsCancellationRequested)
            {
                Pump(token);
                //while (m_pump.Poll(out var e, token))
                //{
                //    var message = e as IPlatformMessage;
                //    if (!m_supportedMessageIds.Any(i => i == (message?.Id)))
                //        continue;

                //    m_tasks.Add(Task.Run(() => ProcessMessage?.Invoke(message)).ContinueWith(t => m_tasks.Remove(t)));
                //}
            }

            Wait();
            //Core.Logger.LogDebug("Exit Pumping Manager Messages.");
            m_isRunning = false;
        }

        public void RunAsync(CancellationToken token)
        {
            m_runTask = new Task(() => { Run(token); }); //Task.Run(() => { Run(token); });
            //.ContinueWith((t) => { Dispose(); })
            //m_runTask.ConfigureAwait(false);
            m_runTask.Start();
        }
        
        public void RunOnce(CancellationToken token)
        {
            if (!m_isInitialized)
                Throw.Exception<InvalidOperationException>();

            if (m_isRunning)
                return;

            m_isRunning = true;
            Pump(token);
            m_isRunning = false;
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            if (m_isInitialized)
                return;

            m_tasks = new ConcurrentList<Task>();
            //m_container.CreateChildContainer();  
            m_pump = new PlatformMessagePump(Core.Logger);
            m_supportedMessageIds = new[] {MessageIds.Quit};
            m_pump.Initialize();
            ProcessMessage += OnProcessMessage;
            Core.ProcessMessage += OnProcessCoreMessage;
            Startup?.Invoke();
            //m_token = token;
            //m_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(m_token);   
        }

        protected virtual void OnProcessCoreMessage(IPlatformMessage message)
        {
            if (!m_isInitialized)
                return;

            if (message.Id == MessageIds.Rendering || message.Id == MessageIds.Window)
                m_pump.Push(message);

            //if (m_supportedMessageIds.Any(i => i == (message?.Id)))
            //m_pump.Push(message);

            //switch (message.Id)
            //{
            //    case MessageIds.Quit:
            //        m_pump.Push(message);
            //        break;
            //}
        }

        protected virtual void OnProcessMessage(IPlatformMessage message)
        {
            //Core.Logger.LogDebug("Found Messages.");
            switch (message.Id)
            {
                case MessageIds.Quit:
                    break;
            }
        }

        protected virtual void RunManager(CancellationToken token)
        {
            m_pump.Pump(token);
        }

        protected virtual void WaitManager() { }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            Wait();
            ProcessMessage -= OnProcessMessage;
            Core.ProcessMessage -= OnProcessCoreMessage;
            Shutdown?.Invoke();
            m_runTask?.ConfigureAwait(false);
            m_runTask?.Dispose();
            m_pump.Dispose();
            m_supportedMessageIds = null;
            //Application.CloseConsole();
            //ProcessMessage.Dispose();
            base.DisposeManagedResources();
        }
    }

    public abstract class PlatformManager<TAssembly, TMessage> : PlatformManager, IPlatformManager<TAssembly, TMessage>
        where TAssembly : PlatformAttribute
        where TMessage : IPlatformMessage
    {
        #region Members
        protected static IPlatformManager<TAssembly, TMessage> m_instnace;

        //private PlatformManager m_platform;
        //private CancellationTokenSource m_tokenSource;
        //private CancellationToken m_token;
        //private IoCContainer m_container;
        #endregion

        #region Properties
        public static IPlatformManager<TAssembly, TMessage> Instance
        {
            get { return m_instnace; }
        }
        #endregion

        #region Methods
        public static void Exec(string namespaceClass, string metodo, List<Parameter> parametros = null)
        {
            var type = Type.GetType(namespaceClass);
            var methodInfo = type.GetMethod(metodo);
            object objectToInvoke;
            if (type.IsAbstract && type.IsSealed)
                objectToInvoke = type;
            else
                objectToInvoke = Activator.CreateInstance(type);

            var parametersFromMethod = methodInfo.GetParameters();


            if (parametros != null || methodInfo != null && parametersFromMethod != null && parametersFromMethod.Length > 0)
            {
                var myParams = new List<object>();
                foreach (var parameterFound in parametersFromMethod)
                {
                    var parametroEspecificado = parametros.Where(p => p.name == parameterFound.Name).FirstOrDefault();
                    if (parametroEspecificado != null)
                        myParams.Add(parametroEspecificado.value);
                    else
                        myParams.Add(null);
                }

                methodInfo.Invoke(objectToInvoke, myParams.ToArray());
            }
            else
                methodInfo.Invoke(objectToInvoke, null);
        }

        protected virtual void CreateManager(params TAssembly[] managers)
        {
            foreach (var manager in managers)
            {
                if (manager.ManagerType == null)
                    continue;

                var i = manager.ManagerType.GetTopLevelInterfaces();
                foreach (var type in i)
                    Core.IoCContainer.Register(type, this);
            }
        }
        
        /// <inheritdoc />
        protected override void CreateResources()
        {
            var os = Core.Environment.OS;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                      .Where(a => Attribute.IsDefined(a, typeof(TAssembly))).ToArray();

            var platform = assemblies
                          .Where(a => Attribute.IsDefined(a, typeof(TAssembly)))
                          .GetAssemblyAttribute<TAssembly>()
                          .Where(attribute => attribute.RequiredOS == os.Type)
                          .OrderBy(attribute => attribute.Priority);

            if (platform == null || platform.IsEmpty())
                throw new InvalidOperationException("No platform found. Are you missing assembly references?");

            CreateManager(platform.ToArray());
        }
        #endregion

        #region Nested Types
        public class Parameter
        {
            #region Properties
            public string name { get; set; }
            public string type { get; set; }
            public object value { get; set; }
            #endregion
        }
        #endregion
    }
}