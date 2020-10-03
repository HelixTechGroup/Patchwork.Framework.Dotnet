#region Usings
using Patchwork.Framework.Platform.Windowing;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public partial interface INWindowRenderer : INRenderer
    {
        #region Properties
        float AspectRatio { get; }
        double DpiScaling { get; }
        NWindowDecorations SupportedDecorations { get; set; }
        INWindow Window { get; }
        #endregion

        #region Methods
        void Initialize(INWindow window, INRenderDevice renderDevice);


        #endregion
    }
}