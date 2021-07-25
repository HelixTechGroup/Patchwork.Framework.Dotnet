using System;

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INOperatingSystemRenderer : INRenderer
    {
        event EventHandler OsRendered;
        event EventHandler OsRendering;

        void OsRender();
    }
}