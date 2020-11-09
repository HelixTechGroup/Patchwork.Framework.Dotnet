#region Usings
using System;
using Shin.Framework.Extensions;
using Shin.Framework.IoC.DependencyInjection;
using SkiaSharp;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public sealed class GdiRenderDevice : NRenderDevice<GdiRenderAdapter>
    {
        #region Methods
        /// <inheritdoc />
        protected override void RegisterRenderers()
        {
            m_iocContainer.Register<INRenderDevice>(this);
            m_iocContainer.Register<INWindowRenderer, GdiWindowRenderer>(false);
            m_supportedRenderers.AddRange(new[] { typeof(INWindowRenderer), typeof(IFrameBufferRenderer)});
            //Core.IoCContainer.Register<INRenderDevice>(this);
        }

        /// <inheritdoc />
        protected override void PlatformSetFrameBuffer(NFrameBuffer buffer)
        {
            //foreach (var renderer in m_iocContainer.ResolveAll<INRenderer>())
            //{
            //    renderer.
            //}
        }

        protected override TRenderer PlatformCreateRenderer<TRenderer>()
        {
            var renderer = m_iocContainer.Resolve<TRenderer>();
            //if (typeof(TRenderer).ContainsInterface<INWindowRenderer>())
            //{
            //renderer.Render += (sender, rectangle) =>
                //                  {
                //                      var pixelBuffer = new NPixelBuffer(rectangle.Width, rectangle.Height);
                //                      var info = new SKImageInfo(rectangle.Width, rectangle.Height);
                //                      var surface = SKSurface.Create(info, pixelBuffer.Handle.Pointer);
                //                  };
            //}

            return renderer;
        }
        #endregion

        /// <inheritdoc />
        public GdiRenderDevice(IContainer iocContainer) : base(iocContainer) { }
    }
}