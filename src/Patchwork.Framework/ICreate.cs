#region Usings
using System;
using Shin.Framework;
#endregion

namespace Patchwork.Framework
{
    public interface ICreate : IInitialize, IDispose
    {
        #region Events
        event EventHandler Created;
        event EventHandler Creating;
        #endregion

        #region Properties
        bool IsCreated { get; }
        #endregion

        #region Methods
        void Create();
        #endregion
    }
}