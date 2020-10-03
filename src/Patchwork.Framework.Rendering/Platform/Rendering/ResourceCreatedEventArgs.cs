#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public sealed class ResourceCreatedEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// The newly created resource object.
        /// </summary>
        public object Resource { get; internal set; }
        #endregion
    }
}