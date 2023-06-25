using Patchwork.Framework.Platform;

namespace Patchwork.Framework
{
    public interface IBind<T>
    {
        T Bind(T resource);
        
        T Unbind(T resource);
    }
}