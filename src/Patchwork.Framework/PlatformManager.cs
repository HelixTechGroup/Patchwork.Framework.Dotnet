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
using Shin.Framework.Messaging;
using RuntimeInformation = Patchwork.Framework.Environment.RuntimeInformation;
using SysEnv = System.Environment;
#endregion

namespace Patchwork.Framework
{
    public abstract class PlatformManager<TAssembly, TMessage> : Initializable, IPlatformManager<TAssembly, TMessage> 
        where TAssembly : PlatformAttribute
        where TMessage : IPlatformMessage
    {
        public event ProcessMessageHandler<TMessage> ProcessMessage;

        public event Action Startup;

        public event Action Shutdown;

        #region Members
        //private PlatformManager m_platform;
        //private CancellationTokenSource m_tokenSource;
        //private CancellationToken m_token;
        //private IoCContainer m_container;
        protected Task m_runTask;
        protected bool m_isRunning;
        protected IList<Task> m_tasks;
        protected IPlatformMessagePump m_pump;
        protected static IPlatformManager<TAssembly, TMessage> m_instnace;
        #endregion

        #region Properties
        public static IPlatformManager<TAssembly, TMessage> Instance
        {
            get { return m_instnace; }
        }

        public bool IsRunning { get { return m_isRunning; } }

        public IPlatformMessagePump MessagePump { get { return m_pump; } }
        #endregion

        #region Methods
        /// <inheritdoc />
        protected override void InitializeResources()
        { 
            base.InitializeResources();

            if (m_isInitialized)
                return;

            m_tasks = new ConcurrentList<Task>();
            //m_container.CreateChildContainer();  
            m_pump = new PlatformMessagePump<TMessage>(Core.Logger);  
            m_pump.Initialize();
            ProcessMessage += OnProcessMessage;
            Core.ProcessMessage += OnProcessCoreMessage;
            Startup?.Invoke();
            //m_token = token;
            //m_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(m_token);   
        }

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
            //Application.CloseConsole();
            //ProcessMessage.Dispose();
            base.DisposeManagedResources();
        }

        public void Pump(CancellationToken token)
        {
            if (!m_isInitialized)
                Throw.Exception<InvalidOperationException>();

            if (m_isRunning)
                Throw.Exception<InvalidOperationException>();

            m_isRunning = true;
            //Core.Logger.LogDebug("Pumping Manager Messages.");
            if (token.IsCancellationRequested)
                return;

            while (m_pump.Poll(out var e, token))
            {
                var message = (TMessage)e;
                m_tasks.Add(Task.Run(() => ProcessMessage?.Invoke(message)).ContinueWith((t) => m_tasks.Remove(t)));
            }

            RunManager();

            //Core.Logger.LogDebug("Exit Pumping Manager Messages.");
            m_isRunning = false;
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

        protected virtual void WaitManager() { }

        public void Run(CancellationToken token)
        {
            if (!m_isInitialized)
                Throw.Exception<InvalidOperationException>();

            if (m_isRunning)
                Throw.Exception<InvalidOperationException>();

            m_isRunning = true;
            //Core.Logger.LogDebug("Pumping Manager Messages.");
            while (!token.IsCancellationRequested)
            {
                while (m_pump.Poll(out var e, token))
                {
                    var message = (TMessage)e;
                    m_tasks.Add(Task.Run(() => ProcessMessage?.Invoke(message)).ContinueWith((t) => m_tasks.Remove(t)));
                }

                RunManager();
            }

            Wait();
            //Core.Logger.LogDebug("Exit Pumping Manager Messages.");
            m_isRunning = false;
        }

        protected virtual void RunManager() { }

        public void RunAsync(CancellationToken token)
        {
            m_runTask = new Task(() => { Run(token); });//Task.Run(() => { Run(token); });
            //.ContinueWith((t) => { Dispose(); })
            //m_runTask.ConfigureAwait(false);
            m_runTask.Start();
        }

        /// <inheritdoc />
        public void Create()
        {
            var os = Core.Environment.OperatingSystem;
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

        protected virtual void OnProcessMessage(TMessage message) 
        {
            //Core.Logger.LogDebug("Found Messages.");
            switch (message.Id)
            {
                case MessageIds.Quit:
                    break;
            }
        }

        protected virtual void OnProcessCoreMessage(IPlatformMessage message)
        {
            if (!m_isInitialized)
                return;

            switch (message.Id)
            {
                case MessageIds.Quit:
                    m_pump.Push(message);
                    break;
            }
        }

        protected abstract void CreateManager(params TAssembly[] managers);

        public static void Executar(string namespaceClass, string metodo, List<Parameter> parametros = null)
        {
            Type type = Type.GetType(namespaceClass);
            MethodInfo methodInfo = type.GetMethod(metodo);
            Object objectToInvoke;
            if (type.IsAbstract && type.IsSealed)
            {
                objectToInvoke = type;
            }
            else
            {
                objectToInvoke = Activator.CreateInstance(type);
            }

            ParameterInfo[] parametersFromMethod = methodInfo.GetParameters();



            if (parametros != null || (methodInfo != null && parametersFromMethod != null && parametersFromMethod.Length > 0))
            {
                List<object> myParams = new List<object>();
                foreach (ParameterInfo parameterFound in parametersFromMethod)
                {
                    Parameter parametroEspecificado = parametros.Where(p => p.name == parameterFound.Name).FirstOrDefault();
                    if (parametroEspecificado != null)
                    {
                        myParams.Add(parametroEspecificado.value);
                    }
                    else
                    {
                        myParams.Add(null);
                    }

                }

                methodInfo.Invoke(objectToInvoke, myParams.ToArray());

            }
            else
            {
                methodInfo.Invoke(objectToInvoke, null);
            }
        }

        public class Parameter
        {
            public string type { get; set; }
            public string name { get; set; }
            public object value { get; set; }

        }
    }
        #endregion
}