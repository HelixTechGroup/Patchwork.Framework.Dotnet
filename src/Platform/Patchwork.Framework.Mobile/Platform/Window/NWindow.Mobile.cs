using System;
using System.Collections.Generic;
using System.Drawing;
using Patchwork.Framework.Messaging;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform.Window
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
