using System.Drawing;

namespace Patchwork.Framework.Platform.Windowing
{
    public partial interface IWindowDataCache : IDataCache
    {
        bool IsRenderable { get; set; }

        bool PreviouslyRenderable { get; }
    }
}