using System;
using System.Linq;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using VD3D = Vortice.Direct3D;

namespace Patchwork.Framework.Platform.Rendering
{
    public static class DirectXFeatureLevel
    {
        public static VD3D.FeatureLevel[] v9 =
        {
            VD3D.FeatureLevel.Level_9_1,
            VD3D.FeatureLevel.Level_9_2,
            VD3D.FeatureLevel.Level_9_3
        };

        public static VD3D.FeatureLevel[] v10 =
        {
            VD3D.FeatureLevel.Level_10_0
        };

        public static VD3D.FeatureLevel[] v11 =
        {
            VD3D.FeatureLevel.Level_11_0
        };

        public static VD3D.FeatureLevel[] v12 =
        {
            VD3D.FeatureLevel.Level_12_0,
            VD3D.FeatureLevel.Level_12_1
        };

        public static VD3D.FeatureLevel[] All = v9.Concat(v10).Concat(v11).Concat(v12).ToArray();
    }
}