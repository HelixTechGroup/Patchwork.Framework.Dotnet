using System.ComponentModel;
using Patchwork.Framework.Platform.Rendering;

namespace Patchwork.Framework.Platform.Windowing
{
    public partial interface INWindow : INRender
    {
        #region Properties
        bool IsRenderable { get; }
        #endregion

        void AddRenderer(params INWindowRenderer[] renderer);

        void Render();
    }
}