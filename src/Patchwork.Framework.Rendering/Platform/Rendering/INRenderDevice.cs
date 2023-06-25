#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.IoC.DependencyInjection;
using Vortice.Direct3D11.Debug;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public partial interface INRenderDevice : ICreate
    {
        #region Events
        event EventHandler<EventArgs> DeviceLost;
        event EventHandler<EventArgs> DeviceReset;
        event EventHandler<EventArgs> DeviceResetting;
        event ProcessMessageHandler ProcessMessage;
        event EventHandler<ResourceCreatedEventArgs> ResourceCreated;
        event EventHandler<ResourceDestroyedEventArgs> ResourceDestroyed;
        #endregion

        #region Properties
        Priority Priority { get; }

        INRenderDeviceConfiguration Configuration { get; }

        IEnumerable<Type> SupportedRenderers { get; }

        IEnumerable<Type> SupportedResources { get; }

        PointF Dpi { get; }
        #endregion

        #region Methods
        TRenderer GetRenderer<TRenderer>(params object[] parameters) where TRenderer : class, INRender;

        TResource GetResource<TResource>(params object[] parameters) where TResource : class, INRenderResource;

        //void ClearResources();

        //INRenderContext Context { get; }

        INRenderFactory Renderer { get; }

        INRenderResourceFactory Resource { get; }
        #endregion

        //void SetFrameBuffer(NFrameBuffer buffer);

        //INRenderDeviceAdvancedSupport Advanced { get; }

        //INRenderDeviceGamingSupport Gaming { get; }

        //void DrawBitmap(INImage source,
        //                double opacity,
        //                Rectangle sourceRect,
        //                Rectangle destRect,
        //                BitmapInterpolationMode bitmapInterpolationMode = BitmapInterpolationMode.Default);


        //void DrawBitmap(INImage source, IBrush opacityMask, Rectangle opacityMaskRect, Rectangle  destRect);


        //void DrawLine(IPen pen, Point p1, Point p2);


        //void DrawGeometry(IBrush brush, IPen pen, IGeometryImpl geometry);


        //void DrawRectangle(IBrush brush,
        //                   IPen pen,
        //                   RoundedRect rect,
        //                   BoxShadows boxShadow = default);


        //void DrawText(IBrush foreground, Point origin, IFormattedTextImpl text);


        //void DrawGlyphRun(IBrush foreground, GlyphRun glyphRun, Point baselineOrigin);


        //IRenderTargetBitmapImpl CreateLayer(Size size);

        internal interface INParentRenderDevice : INRenderDevice
        {
            IEnumerable<INChildRenderDevice> Children { get; }
        }

        internal interface INChildRenderDevice : INRenderDevice
        {
            INParentRenderDevice Parent { get; }
        }

        internal interface INRenderDevicePump
        {
            void Pump(CancellationToken token);

            void Wait();

            void Run(CancellationToken token);

            void RunAsync(CancellationToken token);

            bool Push(IPlatformMessage message);
        }
    }

    public interface INRenderDeviceAdvancedSupport
    {
        //void PushClip(Rectangle clip);


        //void PushClip(RoundedRect clip);


        //void PopClip();


        //void PushOpacity(double opacity);


        //void PopOpacity();


        //void PushOpacityMask(IBrush mask, Rectangle bounds);


        //void PopOpacityMask();


        //void PushGeometryClip(IGeometryImpl clip);


        //void PopGeometryClip();


        //void Custom(ICustomDrawOperation custom);
    }

    public interface INRenderDeviceGamingSupport { }

    //public interface INRenderDevice<TAdapter> : INRenderDevice
    //    where TAdapter : INRenderAdapter
    //{
    //    TAdapter Adapter { get; }
    //}

    //public interface INRenderDevice<TAdapter, TContext> : INRenderDevice
    //    where TAdapter : INRenderAdapter
    //    where TContext : class, INRenderContext
    //{

    //    TContext Context { get; }
    //}
}