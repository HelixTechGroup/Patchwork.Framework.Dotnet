using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Patchwork.Framework.Platform.Windowing
{
    public partial struct NWindowDefinition
    {
        public static NWindowDefinition Default
        {
            get
            {
                return new NWindowDefinition
                       {
                           AcceptsInput = true,
                           DesiredSize = new Size(800, 600),
                           IsRegularWindow = true,
                           Title = "test",
                           Type = NWindowType.Normal,
                           IsRenderable = true,
                           IsMainApplicationWindow = true
                       };
            }
        }
    }
}
