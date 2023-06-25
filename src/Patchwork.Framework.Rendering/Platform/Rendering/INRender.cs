#region Usings
using System;
using System.Drawing;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INRender : IInitialize, IDispose
    {
        #region Events
        event EventHandler Rendered;
        event EventHandler Rendering;
        #endregion

        #region Properties
        //Size Size { get; }
        //Size RenderSize { get; }
        //RenderPriority Priority { get; }
        //RenderStage Stage { get; }
        //bool OwnsRenderLoop { get; }
        bool IsRendering { get; }
        bool IsEnabled { get; }
        #endregion

        #region Methods
        void Render();
        #endregion
    }
}