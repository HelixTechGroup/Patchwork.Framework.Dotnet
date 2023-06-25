using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INRenderContext : INRenderTarget
    {
        static INRenderContext CurrentContext { get; }

        INRenderContext this[INWindow window] { get; }

        //static bool Bind(INWindow window, out INRenderContext context, bool force = false);

        //static bool Unbind(INWindow window, ref INRenderContext context, bool force = false);

        bool Bind(INWindow window, bool force = false);

        bool Unbind(INWindow window, bool force = false);

        void Resize(int width, int height);

        void Resize(Size size);

        void Resize(INWindow window, Size size);
    }

    public interface INRenderContext2D : INRenderContext
    {

    }

    public interface INRenderContext3D : INRenderContext { }

    //public interface INRenderContext<T> : INRenderContext
    //                                      where T : INRenderContext
    //{
    //    new T CurrentContext { get; }

    //    new T this[INWindow window] { get; }

    //    new T Bind(INWindow window);
    //}
}
