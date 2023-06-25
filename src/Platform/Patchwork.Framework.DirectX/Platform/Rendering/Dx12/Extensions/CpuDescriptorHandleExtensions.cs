using System;
using Vortice.Direct3D12;

namespace Patchwork.Framework.Platform.Rendering.Dx12.Extensions
{
    public static class CpuDescriptorHandleExtensions
    {
        public static IntPtr ToIntPtr(this CpuDescriptorHandle handle)
        {
            return new IntPtr((uint)handle.Ptr);
        }
    }
}