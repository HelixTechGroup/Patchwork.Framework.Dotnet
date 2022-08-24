#region Usings
using System;
using System.Collections.Generic;
using Patchwork.Framework.Platform.Rendering.Resources;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.IoC.DependencyInjection;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public sealed class GdiResourceFactory : NRenderResourceFactory
    {
        #region Events
        #endregion

        #region Members
        #endregion

        #region Properties
        #endregion

        public GdiResourceFactory(IContainer iocContainer) : base(iocContainer)
        { }

        /// <inheritdoc />
        protected override void RegisterResources()
        {
            m_supportedResources.Add(typeof(GdiSurface));
        }
    }
}