using System;
using System.Collections.Generic;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.IoC.DependencyInjection;

namespace Patchwork.Framework.Platform
{
    public abstract class NFactory<T> : Initializable, INFactory<T>
    {
        protected IDIContainer m_iocContainer;
        protected ConcurrentList<Type> m_supportedTypes;

        protected NFactory(IDIChildContainer iocContainer)
        {
            m_iocContainer = iocContainer.CreateChildContainer();
        }

        /// <inheritdoc />
        public event EventHandler<T> OnCreate;

        /// <inheritdoc />
        public event EventHandler<T> OnDestroy;

        /// <inheritdoc />
        public IEnumerable<Type> SupportedTypes
        {
            get { return m_supportedTypes; }
        }

        /// <inheritdoc />
        public TType Create<TType>(params object[] parameters) where TType : class, T
        {
            //return ((INFactory<T>)this).Create<TType>(parameters) as TType;
            Throw.If(!m_isInitialized || m_isDisposed).InvalidOperationException();
            Throw.If(!m_supportedTypes.Contains(typeof(TType))).InvalidOperationException();

            lock(m_lock)
            {
                var r = (T)m_iocContainer.Resolve<TType>(parameters: parameters);
                CreateFactoryType(ref r);

                OnCreate?.Invoke(this, r);
                return r as TType;
            }
        }

        protected abstract void CreateFactoryType(ref T instance);

        protected abstract void DestroyFactoryType(ref T instance);

        /// <inheritdoc />
        public void Destroy<TType>(T instance) where TType : T
        {
            Throw.If(!m_supportedTypes.Contains(typeof(T))).InvalidOperationException();

            lock(m_lock)
            {
                OnDestroy?.Invoke(this, instance);
                DestroyFactoryType(ref instance);
            }
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_supportedTypes = new ConcurrentList<Type>();
            RegisterFactoryTypes();
            //foreach (var resource in m_supportedTypes)
            //    m_iocContainer.Register(resource);
        }

        protected abstract void RegisterFactoryTypes();

        //TType INFactory<T>.Create<TType>(params object[] parameters) where TType: class
        //{
        //    Throw.If(!m_supportedTypes.Contains(typeof(TType))).InvalidOperationException();
        //    var r = (T)m_iocContainer.Resolve<TType>(null, parameters);
        //    if (m_isInitialized)
        //        CreateFactoryType(ref r);

        //    OnCreate?.Invoke(this, r);
        //    return r as TType;
        //}
    }
}