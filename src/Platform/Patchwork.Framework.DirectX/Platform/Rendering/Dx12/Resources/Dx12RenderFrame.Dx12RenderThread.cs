using Shin.Framework;

namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources
{
    internal sealed partial class Dx12RenderFrame
    {
        internal sealed class Dx12RenderThread : Disposable
        {
            private readonly Vortice.Direct3D12.ID3D12CommandAllocator m_commandAllocator;
            private readonly Vortice.Direct3D12.ID3D12GraphicsCommandList m_commandList;

            //internal VD3D12.ID3D12CommandAllocator CommandAllocator
            //{
            //    get { return m_commandAllocator; }
            //}

            internal Vortice.Direct3D12.ID3D12GraphicsCommandList CommandList
            {
                get { return m_commandList; }
            }

            internal Dx12RenderThread(Vortice.Direct3D12.ID3D12Device d3d12Device)
            {
                m_commandAllocator = d3d12Device.CreateCommandAllocator(Vortice.Direct3D12.CommandListType.Direct);
                m_commandList = d3d12Device
                   .CreateCommandList<Vortice.Direct3D12.ID3D12GraphicsCommandList>(Vortice.Direct3D12.CommandListType.Direct, 
                                                                                    m_commandAllocator);

                
            }

            public void Reset()
            {
                m_commandAllocator.Reset();
                m_commandList.Reset(m_commandAllocator);
            }

            /// <inheritdoc />
            protected override void DisposeUnmanagedResources()
            {
                m_commandAllocator.Dispose();
                m_commandList.Dispose();

                base.DisposeUnmanagedResources();
            }
        }
    }
}