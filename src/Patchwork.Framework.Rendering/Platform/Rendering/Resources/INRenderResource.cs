namespace Patchwork.Framework.Platform.Rendering.Resources
{

    public interface INRenderResource : INResource
    {
        INRenderDevice Device { get; }

        //void Create(INObject )
    }

    public interface INRenderResource<TNType> : INRenderResource, INResource<TNType>/*, INRender*/
    {

    }

    //public interface INRenderResource<TNDevice> : INRenderResource where TNDevice : INRenderDevice
    //{
    //    TNDevice Device { get; }
    //}
}