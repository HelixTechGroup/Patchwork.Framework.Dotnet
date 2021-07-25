#region Usings
using System;
using Shin.Framework.IoC.DependencyInjection;
using SkiaSharp;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public sealed class SkiaRenderDevice : NRenderDevice<SkiaRenderAdapter>
    {
        #region Methods
        /// <inheritdoc />
        protected override void RegisterRenderers()
        {
            m_iocContainer.Register<INWindowRenderer, SkiaWindowRenderer>(false);
            m_supportedRenderers.Add(typeof(INWindowRenderer));
        }

        /// <inheritdoc />
        protected override void PlatformSetFrameBuffer(NFrameBuffer buffer)
        {
            
        }

        //protected override TRenderer PlatformCreateRenderer<TRenderer>()
        //{
        //    var renderer = m_iocContainer.Resolve<TRenderer>();
        //    //if (typeof(TRenderer).ContainsInterface<INWindowRenderer>())
        //    //{
        //    //renderer.Render += (sender, rectangle) =>
        //        //                  {
        //        //                      var pixelBuffer = new NPixelBuffer(rectangle.Width, rectangle.Height);
        //        //                      var info = new SKImageInfo(rectangle.Width, rectangle.Height);
        //        //                      var surface = SKSurface.Create(info, pixelBuffer.Handle.Pointer);
        //        //                  };
        //    //}

        //    return renderer;
        //}
        #endregion

        /// <inheritdoc />
        public SkiaRenderDevice(IContainer iocContainer) : base(iocContainer) { }
    }
}