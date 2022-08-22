#region Usings
using System;
using System.Collections.Generic;
using Patchwork.Framework.Platform.Rendering.Resources;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.IoC.DependencyInjection;
#endregion

namespace Patchwork.Framework.Platform.Rendering;

public sealed class GdiResourceFactory : Initializable, INResourceFactory
{
    #region Events
    /// <inheritdoc />
    public event EventHandler OnCreate;

    /// <inheritdoc />
    public event EventHandler OnDestroy;
    #endregion

    #region Members
    protected readonly IContainer m_iocContainer;
    private ConcurrentList<Type> m_supportedResources;
    #endregion

    #region Properties
    /// <inheritdoc />
    public IEnumerable<Type> SupportedResources
    {
        get { return m_supportedResources; }
    }
    #endregion

    public GdiResourceFactory(IContainer iocContainer)
    {
        m_iocContainer = iocContainer.CreateChildContainer();
    }

    #region Methods
    /// <inheritdoc />
    public T Create<T>(params object[] parameters) where T : INResource
    {
        Throw.If(!m_supportedResources.Contains(typeof(T))).InvalidOperationException();
        var r = m_iocContainer.Resolve<T>(null, parameters);
        if (m_isInitialized)
            r.Create();

        return r;
    }

    /// <inheritdoc />
    public void Destroy<T>(T instance) where T : INResource
    {
        Throw.If(!m_supportedResources.Contains(typeof(T))).InvalidOperationException();
        instance.Dispose();
    }

    /// <inheritdoc />
    protected override void InitializeResources()
    {
        base.InitializeResources();

        m_supportedResources = new ConcurrentList<Type>();
        m_supportedResources.Add(typeof(GdiSurface));

        foreach (var resource in m_supportedResources) m_iocContainer.Register(resource);
    }
    #endregion
}