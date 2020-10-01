using System;
using System.Collections.Generic;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public interface INResourceFactory<T> : INResourceFactory where T : INResource
    {
        #region Events
        new event EventHandler<T> OnCreate;

        new event EventHandler<T> OnDestroy;
        #endregion

        #region Methods
        new T Create();

        void Destroy(T instance);
        #endregion
    }

    public interface INResourceFactory : IInitialize, IDispose
    {
        #region Events
        event EventHandler OnCreate;

        event EventHandler OnDestroy;
        #endregion

        IEnumerable<Type> SupportedResources { get; }

        #region Methods
        object Create();

        void Destroy(object instance);
        #endregion
    }
} 