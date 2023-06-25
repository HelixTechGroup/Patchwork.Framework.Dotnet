using System.Drawing;

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public interface INRenderFrame : INRenderTarget
    {
        int FrameId { get; }

        int FenceValue { get; set; }

        Color BackgroundColor { get; set; }

        void Begin();
        void Execute(params INRenderCommandList[]  commands);
        void End();
        void Clear();
        void Clear(Color color);
        void Reset();
        //void Wait();
        //void Signal();
    }
}