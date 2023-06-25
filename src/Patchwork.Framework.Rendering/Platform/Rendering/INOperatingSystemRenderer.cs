using System;

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INOperatingSystemRenderer : INRender
    {
        event EventHandler OsRendered;
        event EventHandler OsRendering;

        void OsRender();
    }
}