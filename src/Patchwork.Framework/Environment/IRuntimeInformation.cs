#region Usings
using System;
#endregion

namespace Patchwork.Framework.Environment
{
    public interface IRuntimeInformation
    {
        #region Properties
        RuntimeType Runtime { get; }
        Version RuntimeVersion { get; }
        #endregion

        #region Methods
        void DetectRuntime();
        #endregion
    }
}