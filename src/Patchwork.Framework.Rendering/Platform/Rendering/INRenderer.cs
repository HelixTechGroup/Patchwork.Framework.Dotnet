#region Usings
using System;
using System.Drawing;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public interface INRenderer : IInitialize, IDispose
    {
        #region Events
        event EventHandler Rendered;
        event EventHandler Rendering;
        #endregion

        #region Properties
        Size Size { get; }
        Size VirutalSize { get; }
        RenderPriority Priority { get; }
        RenderStage Stage { get; }
        bool OwnsRenderLoop { get; }
        #endregion

        #region Methods
        bool Invalidate();

        bool Validate();

        void Render();
        #endregion
    }
}