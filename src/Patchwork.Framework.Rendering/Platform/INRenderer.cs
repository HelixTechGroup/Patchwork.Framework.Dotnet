using System;
using System.Drawing;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public partial interface INRenderer : IInitialize, IDispose
    {
        event EventHandler Painting;
        event EventHandler<Rectangle> Paint;
        event EventHandler Painted;

        Size Size { get; }
        Size VirutalSize { get; }

        bool Invalidate();
        bool Validate();

    }
}
