#region Usings
using System;
using UniverseSol.Framework.System;
using UniverseSol.Framework.System.IoC;
using UniverseSol.Framework.System.Threading;

#endregion

namespace UniverseSol.Framework.Factory
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Interface for game factory.</summary>
    ///
    /// <seealso cref="IId"/>
    /// <seealso cref="IIntialize"/>
    /// <seealso cref="IUpdate"/>
    ///-------------------------------------------------------------------------------------------------
    public interface IGameFactory : IUniverseSolObject, IInitializeAsync, IUpdateAsync, IDependency, IEquatable<IGameFactory>
    {
        #region Events
        event EventHandler<UniverseSolPropertyChangeEventArgs<string>> TemplateFileNameChangedEventHandler;
        #endregion

        #region Properties
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the XML file.</summary>
        ///
        /// <value>The XML file.</value>
        ///-------------------------------------------------------------------------------------------------
        string TemplateFileName { get; set; }
        #endregion
    }

    public interface IGameFactory<T> : IGameFactory where T : IUniverseSolObject
    {
        #region Events
        event Action<T> OnCreate;

        event Action<T> OnDestroy;
        #endregion

        #region Methods
        T Create(string templateName);

        void Destroy(T instance);
        #endregion
    }
}