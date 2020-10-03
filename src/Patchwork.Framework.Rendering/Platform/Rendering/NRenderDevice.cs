#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hatzap.Rendering;
using Patchwork.Framework.Manager;
using Patchwork.Framework.Messaging;
using Shield.Framework.IoC.Native.DependencyInjection;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.IoC.DependencyInjection;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderDevice<TAdapter> : Initializable, INRenderDevice<TAdapter> where TAdapter : INRenderAdapter
    {
        #region Events
        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceLost;

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceReset;

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceResetting;

        /// <inheritdoc />
        public event EventHandler<ResourceCreatedEventArgs> ResourceCreated;

        /// <inheritdoc />
        public event EventHandler<ResourceDestroyedEventArgs> ResourceDestroyed;

        public event ProcessMessageHandler ProcessMessage;
        #endregion

        #region Members
        protected INRenderAdapter m_adapter;
        protected Priority m_priority;
        protected IEnumerable<Type> m_supportedRenderers;
        protected IContainer m_iocContainer;
        protected bool m_isRunning;
        protected IPlatformMessagePump m_pump;
        protected Task m_runTask;
        protected MessageIds[] m_supportedMessageIds;
        protected IList<Task> m_tasks;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INRenderAdapter Adapter
        {
            get { return m_adapter; }
        }

        /// <inheritdoc />
        public Priority Priority
        {
            get { return m_priority; }
        }

        /// <inheritdoc />
        public IEnumerable<Type> SupportedRenderers
        {
            get { return m_supportedRenderers; }
        }
        #endregion

        #region Methods
        protected NRenderDevice(IContainer iocContainer)
        {
            m_iocContainer = iocContainer.CreateChildContainer();
            m_pump = new PlatformMessagePump(Core.Logger);
        }

        protected NRenderDevice() : this(new IoCContainer()) { }

        protected abstract void RegisterRenderers();

        /// <inheritdoc />
        public TRenderer GetRenderer<TRenderer>(params object[] parameters) where TRenderer : INRenderer
        {
            Throw.IfNot<NotSupportedException>(m_supportedRenderers.Contains(typeof(TRenderer)));

            var tmp = parameters.ToList(); 
            tmp.Insert(0, this);
            return m_iocContainer.Resolve<TRenderer>(parameters: tmp);
            //return PlatformCreateRenderer<TRenderer>();
        }

        /// <inheritdoc />
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
                //var mt = typeof(TMessage);
                //var t = e.GetType();
                var message = e as IPlatformMessage;
                m_tasks.Add(Task.Run(() => OnProcessMessage(message)).ContinueWith(t => m_tasks.Remove(t)));
            }

            RunManager();

            //Core.Logger.LogDebug("Exit Pumping Manager Messages.");
            m_isRunning = false;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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
                Pump(token);   
            }

            Wait();
            //Core.Logger.LogDebug("Exit Pumping Manager Messages.");
            m_isRunning = false;
        }

        /// <inheritdoc />
        public void RunAsync(CancellationToken token)
        {
            m_runTask = new Task(() => { Run(token); }); //Task.Run(() => { Run(token); });
            //.ContinueWith((t) => { Dispose(); })
            //m_runTask.ConfigureAwait(false);
            m_runTask.Start();
        }

        /// <inheritdoc />
        public bool Push(IPlatformMessage message)
        {
            return m_pump.Push(message);
        }

        /// <inheritdoc />
        public void SetFrameBuffer(NFrameBuffer buffer)
        {
            
        }

        protected virtual void RunManager() { }

        protected virtual void WaitManager() { }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            Wait();
            m_runTask?.ConfigureAwait(false);
            m_runTask?.Dispose();
            m_pump.Dispose();
            m_supportedMessageIds = null;
            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            if (m_isInitialized)
                return;

            m_tasks = new ConcurrentList<Task>();
            m_pump = new PlatformMessagePump(Core.Logger);
            m_supportedMessageIds = new[] {MessageIds.Quit, MessageIds.Rendering};
            m_pump.Initialize();
            RegisterRenderers();
        }

        protected virtual void OnProcessCoreMessage(IPlatformMessage message)
        {
            if (!m_isInitialized)
                return;

            if (!m_supportedMessageIds.Any(i => i.HasValue(message.Id)))
                m_pump.Push(message);

            
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

        protected abstract TRenderer PlatformCreateRenderer<TRenderer>() where TRenderer : INRenderer;
        #endregion
    }
}