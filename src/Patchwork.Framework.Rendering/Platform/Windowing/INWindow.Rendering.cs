using System.ComponentModel;
using Patchwork.Framework.Platform.Rendering;

namespace Patchwork.Framework.Platform.Windowing
{
    public partial interface INWindow
    {
        #region Properties
        bool IsRenderable { get; }
        #endregion

        void AddRenderer(params INRenderer[] renderer);

        void Render();
    }
}