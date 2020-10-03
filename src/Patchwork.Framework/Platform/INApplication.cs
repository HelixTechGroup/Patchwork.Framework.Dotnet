#region Usings
using System.Threading;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public partial interface INApplication : INObject, IInitialize, IDispose
    {
        #region Properties
        Thread Thread { get; }
        #endregion

        #region Methods
        void PumpMessages(CancellationToken cancellationToken);
        #endregion
    }
}