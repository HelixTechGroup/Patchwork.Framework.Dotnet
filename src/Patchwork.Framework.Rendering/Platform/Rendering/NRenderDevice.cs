#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.IoC.DependencyInjection;
using Shin.Framework.Threading;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderDevice : Creatable, INRenderDevice, INRenderDevice.INRenderDevicePump
    {
        #region Events
        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceLost;

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceReset;

        /// <inheritdoc />
        public event EventHandler<EventArgs> DeviceResetting;

        public event ProcessMessageHandler ProcessMessage;

        /// <inheritdoc />
        public event EventHandler<ResourceCreatedEventArgs> ResourceCreated;

        /// <inheritdoc />
        public event EventHandler<ResourceDestroyedEventArgs> ResourceDestroyed;
        #endregion

        #region Members
        //protected static readonly object m_lock = new object();
        //protected static readonly ReaderWriterLockSlim m_lockSlim = new ReaderWriterLockSlim();
        //protected readonly int m_lockTimeout = 50;

        //protected INRenderDeviceConfiguration m_configuration;
        protected PointF m_dpiScale;

        protected bool m_hasLock;

        //protected IList<Type> m_supportedRenderers;
        protected IDIContainer m_iocContainer;
        protected bool m_isPumping;
        protected bool m_isRunning;
        protected Priority m_priority;
        protected IPlatformMessagePump m_pump;
        protected IList<INRender> m_renderers;
        protected Task m_runTask;
        protected MessageIds[] m_supportedMessageIds;
        protected IList<Task> m_tasks;
        protected bool m_isRegistered;
        protected CancellationToken m_token;

        //private INRenderContext m_context;
        //private INRenderFactory m_rendererFactory;
        //private INRenderResourceFactory m_resourceFactory;
        private PointF m_dpi;
        private bool m_isWaiting;
        #endregion

        #region Properties
        /// <inheritdoc />
        public INRenderDeviceConfiguration Configuration
        {
            get { return m_iocContainer.Resolve<INRenderDeviceConfiguration>(strategy: DIResolutionStrategy.SelfOnly); }
        }

        /// <inheritdoc />
        public INRenderContext Context
        {
            get { return m_iocContainer.Resolve<INRenderContext>(strategy: DIResolutionStrategy.SelfOnly); }
        }

        /// <inheritdoc />
        public PointF Dpi
        {
            get { return m_dpi; }
        }

        /// <inheritdoc />
        public Priority Priority
        {
            get { return m_priority; }
        }

        /// <inheritdoc />
        public INRenderFactory Renderer
        {
            get { return m_iocContainer.Resolve<INRenderFactory>(strategy: DIResolutionStrategy.SelfOnly); }
        }

        /// <inheritdoc />
        public INRenderResourceFactory Resource
        {
            get { return m_iocContainer.Resolve<INRenderResourceFactory>(strategy: DIResolutionStrategy.SelfOnly); }
        }

        /// <inheritdoc />
        public IEnumerable<Type> SupportedRenderers
        {
            get { return m_iocContainer.Resolve<INRenderFactory>(strategy: DIResolutionStrategy.SelfOnly)?.SupportedTypes ?? new List<Type>(); }
        }

        /// <inheritdoc />
        public IEnumerable<Type> SupportedResources
        {
            get { return m_iocContainer.Resolve<INRenderResourceFactory>(strategy: DIResolutionStrategy.SelfOnly).SupportedTypes; }
        }
        #endregion

        protected NRenderDevice(IDIChildContainer iocContainer)
        {
            m_iocContainer = iocContainer; //.CreateChildContainer();
            //m_pump = m_iocContainer.Resolve<IPlatformMessagePump>();
            //m_supportedRenderers = new ConcurrentList<Type>();
            m_renderers = new ConcurrentList<INRender>();
            ProcessMessage += OnProcessMessage;
            //Core.MessagePump.MessagePopped += OnProcessCoreMessage;
            m_dpiScale = new PointF(961f, 96f);
            m_iocContainer.Resolve<IPlatformWindowManager>().WindowCreated += OnWindowCreated;
        }

        #region Methods
        //protected abstract void RegisterRenderers();

        /// <inheritdoc />
        public TRenderer GetRenderer<TRenderer>(params object[] parameters) where TRenderer : class, INRender
        {
            var factory = m_iocContainer.Resolve<INRenderFactory>(strategy: DIResolutionStrategy.SelfOnly);
            Throw.IfNot<NotSupportedException>(factory.SupportedTypes.Contains(typeof(TRenderer)));

            //if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
            //    Wait();

            //if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
            //    Throw.Exception().InvalidOperationException();

            //m_hasLock = true;

            try
            {
                lock(m_lock)
                {
                    if (!parameters.Contains(this))
                    {
                        var tmpParam = new ConcurrentList<object>();
                        tmpParam.Add(this);
                        tmpParam.AddRange(parameters);
                        parameters = tmpParam.ToArray();
                    }

                    return factory.Create<TRenderer>(parameters);

                    //PlatformCreateRenderer<TRenderer>(parameters);

                    //var tmp = parameters.ToList();
                    //tmp.Insert(0, this);
                    //var rend = m_iocContainer.Resolve<TRenderer>(parameters: tmp.ToArray());

                    //if (!m_renderers.Contains(rend))
                    //    m_renderers.Add(rend);
                    //else
                    //{
                    //    rend.Dispose();
                    //    foreach (var r in m_renderers)
                    //    {
                    //        if (!Equals(r, rend as INRender))
                    //            continue;

                    //        rend = (TRenderer)r;
                    //        break;

                    //    }
                    //    //rend = (TRenderer)m_renderers.Where(r => r == rend as INRenderer);
                    //}

                    //if (m_isInitialized)
                    //    rend.Initialize();

                    //return rend;
                }
            }
            finally

            {
                //if (m_hasLock)
                //{
                //    m_lockSlim.ExitWriteLock();
                //    m_hasLock = false;
                //}
            }
            //return PlatformCreateRenderer<TRenderer>();
        }

        /// <inheritdoc />
        public TResource GetResource<TResource>(params object[] parameters) where TResource : class, INRenderResource
        {
            
                var factory = m_iocContainer.Resolve<INRenderResourceFactory>(strategy: DIResolutionStrategy.SelfOnly);
                Throw.IfNot<NotSupportedException>(factory.SupportedTypes.Contains(typeof(TResource)));

                //if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
                //    Wait();

                //if (!m_lockSlim.TryEnter(SynchronizationAccess.Write))
                //    Throw.Exception().InvalidOperationException();

                //m_hasLock = true;

                try
                {
                    lock (m_lock)
                    {
                        return factory.Create<TResource>(parameters);
                    }
                }
                finally
                {
                    //if (m_hasLock)
                    //{
                    //    m_lockSlim.ExitWriteLock();
                    //    m_hasLock = false;
                    //}
                }
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

            lock(m_lock)
            {
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
        }

        /// <inheritdoc />
        public void Wait()
        {
            if (!m_isInitialized ^ m_isWaiting)
                return; //Throw.Exception<InvalidOperationException>();

            lock(m_lock)
            {
                try
                {
                    m_isWaiting = true;
                    WaitManager();
                    var whenAll = Task.WhenAll(m_tasks);
                    Task.WhenAll(whenAll).ConfigureAwait(false);

                    for (;;)
                    {
                        while (whenAll.Status != TaskStatus.RanToCompletion)
                        {
                            Console.Write(".");
                            Thread.Sleep(500);
                        }

                        break;
                    }
                }
                finally
                {
                    m_isWaiting = true;
                    m_tasks.Clear();
                }
            }
        }

        /// <inheritdoc />
        public void Run(CancellationToken token)
        {
            if (!m_isInitialized)
                Throw.Exception<InvalidOperationException>();

            //if (m_isRunning)
            //Wait();
            //Throw.Exception<InvalidOperationException>();

            lock(m_lock)
            {
                m_isRunning = true;
                m_token = token;
                //Core.Logger.LogDebug("Pumping Manager Messages.");
                while (!m_token.IsCancellationRequested) Pump(m_token);

                //Core.Logger.LogDebug("Exit Pumping Manager Messages.");
                m_isRunning = false;
            }

            Wait();
        }

        /// <inheritdoc />
        public void RunAsync(CancellationToken token)
        {
            m_runTask = new Task(() =>
                                 {
                                     Run(token);
                                 }); //Task.Run(() => { Run(token); });
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

        //protected virtual void OnProcessCoreMessage(object sender, IPumpMessage message)
        //{
        //    if (!m_isInitialized)
        //        return;

        //    var p = message as IPlatformMessage;
        //    if (m_supportedMessageIds.Any(i => i == p?.Id))
        //        m_pump.Push(message);


        //    switch (p.Id)
        //    {
        //        case MessageIds.Quit:
        //            m_pump.Push(message);
        //            break;
        //    }
        //}

        protected virtual void OnProcessMessage(IPlatformMessage message)
        {
            //Core.Logger.LogDebug("Found Messages.");
            switch (message.Id)
            {
                case MessageIds.Quit:
                    break;
            }
        }

        protected virtual void RegisterTypes()
        {
            m_isRegistered = true;
        }

        //protected virtual void RunManager()
        //{
        //    //foreach (var r in m_renderers/*.Where(r => !r.OwnsRenderLoop)*/)
        //    //{
        //    //    r.Render();
        //    //}
        //}

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

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            if (!m_isRegistered)
                RegisterTypes();

            return true;
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            if (!m_isRegistered)
                RegisterTypes();

            m_tasks = new ConcurrentList<Task>();
            m_pump = new PlatformMessagePump(Core.Logger);
            m_supportedMessageIds = new[] {MessageIds.Quit, MessageIds.Rendering};
            m_pump.Initialize();

            if (m_iocContainer.TryResolve<INRenderFactory>(out var renderFactory,
                                                           strategy: DIResolutionStrategy.SelfOnly))
                renderFactory?.Initialize();

            if (m_iocContainer.TryResolve<INRenderResourceFactory>(out var renderResourceFactory,
                                                                   strategy: DIResolutionStrategy.SelfOnly))
                renderResourceFactory?.Initialize();

            //PlatformCreateDevice();
        }

        protected void OnWindowCreated(object sender, INWindow window)
        {
            if (!window.IsRenderable)
                return;

            //window.Created += (o, args) =>
            //{
            //Create();


            //if (!m_iocContainer.TryResolve<INRenderContext>(out var renderContext,
            //                                                strategy: DIResolutionStrategy.SelfOnly))
            //    return;

            //renderContext?.Create();
            //renderContext?.Initialize();
            //renderContext?.Bind(window);
            //}
        }

        protected virtual void PlatformCreateDevice(bool force)
        {

        }

        protected abstract void PlatformSetFrameBuffer(NFrameBuffer buffer);

        protected abstract void RunManager();

        protected abstract void PlatformGetDpi(INWindow window);
        #endregion

        //protected NRenderDevice() : this(new DIContainer()) { }

        //protected abstract TRenderer PlatformCreateRenderer<TRenderer>(params object[] parameters) where TRenderer : INRenderer;
    }

    //public abstract class NRenderDevice<TAdapter> : NRenderDevice, INRenderDevice<TAdapter> 
    //    where TAdapter : class, INRenderAdapter
    //{
    //    #region Members
    //    protected TAdapter m_adapter;
    //    #endregion

    //    #region Properties
    //    /// <inheritdoc />
    //    public new TAdapter Adapter
    //    {
    //        get { return m_adapter; }
    //    }
    //    #endregion

    //    protected NRenderDevice(IDIContainer iocContainer) : base(iocContainer) { }

    //    #region Methods
    //    /// <inheritdoc />
    //    protected override void InitializeResources()
    //    {
    //        base.InitializeResources();

    //        m_adapter = m_iocContainer.TryResolve<TAdapter>();
    //        m_adapter?.Initialize();
    //    }
    //    #endregion

    //}

    //public abstract class NRenderDevice<TAdapter, TContext> : NRenderDevice<TAdapter>, INRenderDevice<TAdapter, TContext>
    //    where TAdapter : class, INRenderAdapter
    //    where TContext : class, INRenderContext
    //{
    //    protected TContext m_context;

    //    public new TContext Context
    //    {
    //        get { return m_context; }
    //    }

    //    protected NRenderDevice(IDIContainer iocContainer) : base(iocContainer) { }

    //    protected override void InitializeResources()
    //    {
    //        base.InitializeResources();

    //        m_context = m_iocContainer.TryResolve<TContext>();
    //        m_context?.Initialize();

    //    }
    //}
}