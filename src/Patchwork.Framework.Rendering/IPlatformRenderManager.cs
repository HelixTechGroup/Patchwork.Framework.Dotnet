#region Usings
using System;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Windowing;
#endregion

namespace Patchwork.Framework
{
    public interface IPlatformRenderManager : IPlatformManager<AssemblyRenderingAttribute, IPlatformMessage<IRenderMessageData>>
    {
        #region Events
        public event EventHandler<INWindow> WindowCreated;

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowDestroyed;
        #endregion

        #region Methods
        bool IsRendererSupported<TRenderer>() where TRenderer : INRenderer;

        TRenderer GetRenderer<TRenderer>(params object[] parameters) where TRenderer : INRenderer;
        #endregion
    }
}