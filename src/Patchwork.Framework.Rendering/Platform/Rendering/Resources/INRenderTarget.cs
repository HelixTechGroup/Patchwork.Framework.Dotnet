using System.Drawing;

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public interface INRenderTarget : INRenderResource, 
                                      IBind<INRenderResource>
    {
        INRenderResource Target { get; set; }

        Size RenderSize {get;}
        
        void Clear();

        void Begin();

        void End();

        void Flush();

        void Clear(Color color);
        
        void Resize(Size size);
    }

    public interface INRenderTarget<TNative> : 
        INRenderTarget, 
        INRenderResource<TNative>
    {

    }
}