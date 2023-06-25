using System;
using Patchwork.Framework.Platform.Rendering.Resources;
using SharpGen.Runtime;
using Vortice.DXGI;
using VD3D12 = Vortice.Direct3D12;


namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    internal sealed class Dx12GraphicsCommandList : Dx12CommandList<VD3D12.ID3D12GraphicsCommandList>, IDx12GraphicsCommandList
    {
        private bool m_isClosed;

        /// <inheritdoc />
        internal Dx12GraphicsCommandList(INRenderDevice device, NRenderCommandType type) : base(device, type) { }

        /// <inheritdoc />
        internal Dx12GraphicsCommandList(INRenderDevice device, NRenderCommandType type, VD3D12.ID3D12CommandAllocator commandAllocator) : base(device, type, commandAllocator) { }

        /// <inheritdoc />
        internal Dx12GraphicsCommandList(INRenderCommandQueue queue) : base(queue)
        {
        }

        /// <inheritdoc />
        protected override void PlatformReset()
        {
            if (!m_isClosed) 
                return;

            base.PlatformReset();
            m_resource.Reset(m_commandAllocator);
            m_isClosed = false;
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            if (base.CreateResources(force))
            {
                PlatformClose();
            }

            return true;
        }

        /// <inheritdoc />
        protected override void PlatformEnd()
        {
            base.PlatformEnd();
            PlatformClose();
        }

        private void PlatformClose()
        {
            try
            {
                if (m_isClosed) 
                    return;

                m_resource.Close();
                m_isClosed = true;
            }
            catch (SharpGenException sgex)
            {
                Core.Logger.LogException(sgex);
                m_isClosed = false;
            }
        }
    }
}
