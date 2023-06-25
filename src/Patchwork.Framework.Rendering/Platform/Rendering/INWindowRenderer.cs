#region Usings
using System;
using Patchwork.Framework.Platform.Windowing;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public partial interface INWindowRenderer : INRender, IEquatable<INWindowRenderer>
    {
        #region Properties
        float AspectRatio { get; }
        double DpiScaling { get; }
        INWindow Window { get; }
        #endregion

        #region Methods
        //void Initialize(INWindow window, INRenderDevice renderDevice);


        #endregion
    }
}