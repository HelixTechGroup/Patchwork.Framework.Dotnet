using System;

using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public abstract class NDisposableResource<TNative> : NResource<TNative> where TNative : IDisposable
    {
        protected NDisposableResource() { }

        protected NDisposableResource(TNative resource) : base(resource) { }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            m_resource?.Dispose();
            base.DisposeUnmanagedResources();
        }
    }
}