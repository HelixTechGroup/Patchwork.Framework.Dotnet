#region Usings
using System;
using System.Collections.Generic;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Rendering;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Patchwork.Framework.Runtime;
#endregion

namespace Patchwork.Framework
{
    public interface IPlatformRenderingManager : IPlatformManager<AssemblyRenderingAttribute, IPlatformMessage<IRenderMessageData>>
    {
        #region Events
        public event EventHandler<INWindow> WindowCreated;

        /// <inheritdoc />
        public event EventHandler<INWindow> WindowDestroyed;
        #endregion

        #region Methods
        TDevice GetDevice<TDevice>(params object[] parameters) where TDevice : class, INRenderDevice;

        bool IsRendererSupported<TRenderer>() where TRenderer : class, INRender;

        bool IsResourceSupported<TResource>() where TResource : class, INRenderResource;

        IEnumerable<Type> SupportedRenderers { get; }

        IEnumerable<Type> SupportedResources { get; }

        TRenderer GetRenderer<TRenderer>(params object[] parameters) where TRenderer : class, INRender;

        TRenderer[] GetRenderers<TRenderer>(params object[] parameters) where TRenderer : class, INRender;

        TResource GetResource<TResource>(params object[] parameters) where TResource : class, INResource;
        #endregion
    }
}