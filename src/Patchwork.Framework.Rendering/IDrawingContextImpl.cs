using System;
using System.Drawing;
using Avalonia.Media;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Utilities;
using Avalonia.Visuals.Media.Imaging;

namespace Avalonia.Platform
{

    public interface IDrawingContextImpl : IDisposable
    {

        Matrix Transform { get; set; }


        void Clear(Color color);


        void DrawBitmap(IRef<IBitmapImpl> source, double opacity, Rect sourceRect, Rect destRect, BitmapInterpolationMode bitmapInterpolationMode = BitmapInterpolationMode.Default);


        void DrawBitmap(IRef<IBitmapImpl> source, IBrush opacityMask, Rect opacityMaskRect, Rect destRect);


        void DrawLine(IPen pen, Point p1, Point p2);


        void DrawGeometry(IBrush brush, IPen pen, IGeometryImpl geometry);


        void DrawRectangle(IBrush brush, IPen pen, RoundedRect rect,
            BoxShadows boxShadow = default);


        void DrawText(IBrush foreground, Point origin, IFormattedTextImpl text);


        void DrawGlyphRun(IBrush foreground, GlyphRun glyphRun, Point baselineOrigin);


        IRenderTargetBitmapImpl CreateLayer(Size size);

        void PushClip(Rect clip);


        void PushClip(RoundedRect clip);


        void PopClip();


        void PushOpacity(double opacity);


        void PopOpacity();


        void PushOpacityMask(IBrush mask, Rect bounds);


        void PopOpacityMask();


        void PushGeometryClip(IGeometryImpl clip);


        void PopGeometryClip();


        void Custom(ICustomDrawOperation custom);
    }
}
