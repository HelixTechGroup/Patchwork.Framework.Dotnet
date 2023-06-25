using System;
using System.Collections.Generic;

using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public interface INFactory<T> : IInitialize, IDispose
    {
        #region Events
        /// <inheritdoc />
        event EventHandler<T> OnCreate;

        /// <inheritdoc />
        event EventHandler<T> OnDestroy;
        #endregion

        #region Properties
        /// <inheritdoc />
        IEnumerable<Type> SupportedTypes { get; }
        #endregion

        #region Methods
        /// <inheritdoc />
        TType Create<TType>(params object[] parameters) where TType : class, T;

        /// <inheritdoc />
        void Destroy<TType>(T instance) where TType : T;
        #endregion
    }
}