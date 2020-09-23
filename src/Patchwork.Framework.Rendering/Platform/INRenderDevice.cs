#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using Patchwork.Framework.Platform.Window;
using Patchwork.Framework.Rendering;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform
{
    public interface INRenderDevice : IInitialize, IDispose
    {
        event EventHandler<EventArgs> DeviceLost;
        event EventHandler<EventArgs> DeviceReset;
        event EventHandler<EventArgs> DeviceResetting;
        event EventHandler<ResourceCreatedEventArgs> ResourceCreated;
        event EventHandler<ResourceDestroyedEventArgs> ResourceDestroyed;

        INRenderAdapter Adapter { get; }

        Priority Priority { get; }

        IEnumerable<Type> SupportedRenderers { get; }

        #region Methods
        TRenderer GetRenderer<TRenderer>() where TRenderer : INRenderer;

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
        #endregion
    }

    public interface INImage { }

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

    public interface INRenderDevice<TAdapter> : INRenderDevice
        where TAdapter : INRenderAdapter { }
}