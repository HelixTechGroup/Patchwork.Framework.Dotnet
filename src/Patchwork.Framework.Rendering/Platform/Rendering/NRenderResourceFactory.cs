using System;
using System.Collections.Generic;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.IoC.DependencyInjection;

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract class NRenderResourceFactory : Initializable, INResourceFactory
    {
        protected IContainer m_iocContainer;
        protected ConcurrentList<Type> m_supportedResources;

        public NRenderResourceFactory(IContainer iocContainer)
        {
            m_iocContainer = iocContainer.CreateChildContainer();
        }

        /// <inheritdoc />
        public event EventHandler OnCreate;

        /// <inheritdoc />
        public event EventHandler OnDestroy;

        /// <inheritdoc />
        public IEnumerable<Type> SupportedResources
        {
            get { return m_supportedResources; }
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_supportedResources = new ConcurrentList<Type>();
            RegisterResources();
            foreach (var resource in m_supportedResources)
                m_iocContainer.Register(resource);
        }

        protected abstract void RegisterResources();

        /// <inheritdoc />
        public virtual T Create<T>(params object[] parameters) where T : INResource
        {
            Throw.If(!m_supportedResources.Contains(typeof(T))).InvalidOperationException();
            var r = m_iocContainer.Resolve<T>(null, parameters);
            if (m_isInitialized)
                r.Create();

            return r;
        }

        /// <inheritdoc />
        public virtual void Destroy<T>(T instance) where T : INResource
        {
            Throw.If(!m_supportedResources.Contains(typeof(T))).InvalidOperationException();
            instance.Dispose();
        }
    }
}