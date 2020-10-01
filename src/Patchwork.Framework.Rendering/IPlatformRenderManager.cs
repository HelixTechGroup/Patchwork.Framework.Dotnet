using System;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Windowing;

namespace Patchwork.Framework
{
    public interface IPlatformRenderManager : IPlatformManager<AssemblyRenderingAttribute, IPlatformMessage<IRenderMessageData>>
    {
        public event EventHandler<INWindow> WindowCreated;

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowDestroyed;

        bool IsRendererSupported<TRenderer>() where TRenderer : INRenderer;

        TRenderer GetRenderer<TRenderer>() where TRenderer : INRenderer;
    }
}