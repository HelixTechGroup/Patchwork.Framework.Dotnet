using System;

namespace Patchwork.Framework.Platform
{
    public sealed class ResourceDestroyedEventArgs : EventArgs
    {
        /// <summary>
        /// The name of the destroyed resource.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The resource manager tag of the destroyed resource.
        /// </summary>
        public Object Tag { get; internal set; }
    }
}