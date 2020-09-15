using System;
using System.Drawing;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public partial interface INativeRenderer : IInitialize, IDispose
    {
        event EventHandler Painting;
        event EventHandler<Rectangle> Paint;
        event EventHandler Painted;

        INativeScreen Screen { get; }
        Size Size { get; }
        Size VirutalSize { get; }

        bool Invalidate();
        bool Validate();

    }
}
