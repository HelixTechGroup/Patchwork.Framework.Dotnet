using Patchwork.Framework.Messaging;

namespace Patchwork.Framework.Platform.Windowing
{
    public abstract partial class NWindow
    {
        partial void InitializeResourcesShared() { }

        partial void DisposeManagedResourcesShared() { }

        partial void DisposeUnmanagedResourcesShared() { }

        partial void WireUpWindowEventsShared() { }

        partial void OnProcessMessageShared(IPlatformMessage message) { }
    }
}
