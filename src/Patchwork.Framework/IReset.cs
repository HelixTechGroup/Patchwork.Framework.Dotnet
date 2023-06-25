#region Usings
using System;
#endregion

namespace Patchwork.Framework
{
    public interface IReset
    {
        #region Events
        event EventHandler Resetting;
        #endregion

        #region Properties
        bool IsReset { get; }
        #endregion

        #region Methods
        void Reset();
        #endregion
    }
}