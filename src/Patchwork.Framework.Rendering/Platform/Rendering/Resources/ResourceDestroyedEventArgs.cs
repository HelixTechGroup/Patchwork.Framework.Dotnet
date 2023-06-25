#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public sealed class ResourceDestroyedEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// The name of the destroyed resource.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The resource manager tag of the destroyed resource.
        /// </summary>
        public object Tag { get; internal set; }
        #endregion
    }
}