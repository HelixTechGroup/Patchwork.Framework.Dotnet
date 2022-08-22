#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Manager;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.IoC.DependencyInjection;
using Shield.Framework.IoC.Native.DependencyInjection;
using Shin.Framework.Messaging;
using Shin.Framework.Threading;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderDevice : Initializable, INRenderDevice
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

        protected CancellationToken m_token;
        protected INRenderAdapter m_adapter;
        protected Priority m_priority;
        protected IList<Type> m_supportedRenderers;
        protected IContainer m_iocContainer;
        protected bool m_isRunning;
        protected bool m_isPumping;
        protected IPlatformMessagePump m_pump;
        protected Task m_runTask;
        protected MessageIds[] m_supportedMessageIds;
        protected IList<Task> m_tasks;
        protected IList<INRenderer> m_renderers;
        protected readonly ReaderWriterLockSlim m_lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        protected static readonly object m_lock = new object();
        protected bool m_hasLock;
        protected readonly int m_lockTimeout = 50;
        protected INRenderContext m_context;
        protected PointF m_dpiScale;

        public INRenderAdapter Adapter
        {
            get { return m_adapter; }
        }

        /// <inheritdoc />
        public INRenderContext Context
        {
            get { return m_context; }
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

        /// <inheritdoc />
        public PointF DpiScale
        {
            get { return m_dpiScale; }
        }

        protected NRenderDevice(IContainer iocContainer)
        {
            m_iocContainer = iocContainer.CreateChildContainer();
            m_pump = new PlatformMessagePump(Core.Logger);
            m_supportedRenderers = new ConcurrentList<Type>();
            m_renderers = new ConcurrentList<INRenderer>();
            ProcessMessage += OnProcessMessage;
            Core.MessagePump.MessagePopped += OnProcessCoreMessage;
            m_dpiScale = new PointF(1f, 1f);

        }

        protected NRenderDevice() : this(new IoCContainer()) { }

        #region Methods
        protected abstract void RegisterRenderers();

        /// <inheritdoc />
        public TRenderer GetRenderer<TRenderer>(params object[] parameters) where TRenderer : INRenderer
        {
            Throw.IfNot<NotSupportedException>(m_supportedRenderers.Contains(typeof(TRenderer)));

            if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
                Wait();

            if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
                Throw.Exception().InvalidOperationException();

            m_hasLock = true;

            try
            {
                lock (m_lock)
                {
                    //PlatformCreateRenderer<TRenderer>(parameters);

                    var tmp = parameters.ToList();
                    tmp.Insert(0, this);
                    var rend = m_iocContainer.Resolve<TRenderer>(parameters: tmp.ToArray());

                    if (!m_renderers.Contains(rend))
                        m_renderers.Add(rend);
                    else
                    {
                        rend.Dispose();
                        foreach (var r in m_renderers)
                        {
                            if (!Equals(r, rend as INRenderer))
                                continue;

                            rend = (TRenderer)r;
                            break;

                        }
                        //rend = (TRenderer)m_renderers.Where(r => r == rend as INRenderer);
                    }

                    if (m_isInitialized && !rend.IsInitialized)
                        rend.Initialize();

                    return rend;
                }
            }
            finally

            {
                if (m_hasLock)
                {
                    m_lockSlim.ExitWriteLock();
                    m_hasLock = false;
                }
            }
            //return PlatformCreateRenderer<TRenderer>();
        }

        /// <inheritdoc />
        public void Pump(CancellationToken token)
        {
            if ((!m_isInitialized | token.IsCancellationRequested) ^ m_isPumping)
                return;
            //if (!m_isInitialized)
            //    Throw.Exception<InvalidOperationException>();

            //if (m_isPumpiing)
            //    return;
            //Wait();
            //Throw.Exception<InvalidOperationException>();

            m_isPumping = true;
            //Core.Logger.LogDebug("Pumping Manager Messages.");
            //if (token.IsCancellationRequested)
            //    return;

            while (m_pump.Poll(out var e, token))
            {
                //var mt = typeof(TMessage);
                //var t = e.GetType();
                var message = e as IPlatformMessage;
                m_tasks.Add(Task.Run(() => ProcessMessage?.Invoke(message), token)
                                .ContinueWith(t => m_tasks.Remove(t), token));
            }

            RunManager();

            //Core.Logger.LogDebug("Exit Pumping Manager Messages.");
            m_isPumping = false;
        }

        /// <inheritdoc />
        public void Wait()
        {
            if (!m_isInitialized)
                return;//Throw.Exception<InvalidOperationException>();

            WaitManager();
            var whenAll = Task.WhenAll(m_tasks);
            Task.WhenAll(whenAll).ConfigureAwait(false);

            for (; ; )
            {
                while (whenAll.Status != TaskStatus.RanToCompletion)
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

            //if (m_isRunning)
            //Wait();
            //Throw.Exception<InvalidOperationException>();

            m_isRunning = true;
            m_token = token;
            //Core.Logger.LogDebug("Pumping Manager Messages.");
            while (!m_token.IsCancellationRequested)
            {
                Pump(m_token);
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
            PlatformSetFrameBuffer(buffer);
        }

        protected abstract void PlatformSetFrameBuffer(NFrameBuffer buffer);

        protected virtual void RunManager()
        {
            foreach (var r in m_renderers.Where(r => !r.OwnsRenderLoop))
            {
                r.Render();
            }
        }

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
            if (m_isInitialized)
                return;

            base.InitializeResources();

            m_tasks = new ConcurrentList<Task>();
            m_pump = new PlatformMessagePump(Core.Logger);
            m_supportedMessageIds = new[] { MessageIds.Quit, MessageIds.Rendering };
            m_pump.Initialize();
            RegisterRenderers();
        }

        protected virtual void OnProcessCoreMessage(object sender, IPumpMessage message)
        {
            if (!m_isInitialized)
                return;

            var p = message as IPlatformMessage;
            if (m_supportedMessageIds.Any(i => i == p?.Id))
                m_pump.Push(message);


            switch (p.Id)
            {
                case MessageIds.Quit:
                    m_pump.Push(message);
                    break;
            }
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

        protected abstract void PlatformGetDpi(INWindow window);

        //protected abstract TRenderer PlatformCreateRenderer<TRenderer>(params object[] parameters) where TRenderer : INRenderer;
        #endregion
    }

    public abstract class NRenderDevice<TAdapter> : NRenderDevice, INRenderDevice<TAdapter> 
        where TAdapter : class, INRenderAdapter
    {
        #region Members
        
        #endregion

        #region Properties
        /// <inheritdoc />
        public new TAdapter Adapter
        {
            get { return m_adapter as TAdapter; }
        }
        #endregion

        protected NRenderDevice(IContainer iocContainer) : base(iocContainer) { }

        #region Methods
        #endregion

    }

    public abstract class NRenderDevice<TAdapter, TContext> : NRenderDevice<TAdapter>, INRenderDevice<TAdapter, TContext>
        where TAdapter : class, INRenderAdapter
        where TContext : class, INRenderContext
    {
        public new TContext Context
        {
            get { return m_context as TContext; }
        }

        protected NRenderDevice(IContainer iocContainer) : base(iocContainer) { }
    }
}