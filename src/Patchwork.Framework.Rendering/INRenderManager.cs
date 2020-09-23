using System;
using System.Collections.Generic;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;

namespace Patchwork.Framework.Platform
{
    public interface INRenderManager : IPlatformManager<AssemblyRenderingAttribute, IPlatformMessage<IRenderMessageData>>
    {
        public event EventHandler<INWindow> WindowCreated;

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowDestroyed;

        bool IsRendererSupported<TRenderer>() where TRenderer : INRenderer;

        TRenderer GetRenderer<TRenderer>() where TRenderer : INRenderer;
    }
}