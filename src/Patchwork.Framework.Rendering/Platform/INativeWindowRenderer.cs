using System;
using System.Drawing;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public partial interface INativeWindowRenderer : IInitialize, IDispose
    {
        event EventHandler Painting;
        event EventHandler<Rectangle> Paint;
        event EventHandler Painted;

        INativeRenderAdapter Adapter { get; }
        INativeScreen Screen { get; }
        Size VirutalSize { get; }
        float AspectRatio { get; }

        bool Invalidate();
        bool Validate();

    }
}
