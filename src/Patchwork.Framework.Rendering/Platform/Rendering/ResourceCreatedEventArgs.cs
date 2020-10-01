using System;

namespace Patchwork.Framework.Platform
{
    public sealed class ResourceCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// The newly created resource object.
        /// </summary>
        public Object Resource { get; internal set; }
    }
}