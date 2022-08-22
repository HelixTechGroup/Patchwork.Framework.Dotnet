namespace Patchwork.Framework.Platform.Rendering
{

    public interface INRenderResource : INResource
    {
        //void Create(INObject )
    }

    public interface INRenderResource<TNType> : INRenderResource, INResource<TNType>
    {

    }
}