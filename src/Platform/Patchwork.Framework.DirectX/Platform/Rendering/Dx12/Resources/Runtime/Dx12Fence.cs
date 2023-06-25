using System;
using System.Threading;
using VD3D12 = Vortice.Direct3D12;

namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources.Runtime
{
    public class Dx12Fence : Dx12Object<VD3D12.ID3D12Fence>, INRenderFence
    {
        protected ulong m_fenceValue;
        private bool m_signaled;

        public int CurrentValue { get { return (int)m_fenceValue; } }

        /// <inheritdoc />
        public int CompletedValue
        {
            get { return (int)m_fenceValue + 1; }
        }

        /// <inheritdoc />
        public Dx12Fence(INRenderDevice device) : base(device) { }

        /// <inheritdoc />
        public Dx12Fence(INRenderDevice device, VD3D12.ID3D12Fence resource) : base(device, resource) { }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            using var fence = ((DX12RenderDevice)m_device).D3D12Device.CreateFence();
            m_resource = fence.QueryInterface<VD3D12.ID3D12Fence1>();
            m_fenceValue = fence.CompletedValue;
            return base.CreateResources(force);
        }

        /// <inheritdoc />
        //protected override object PlatformClone()
        //{
        //    throw new NotImplementedException();
        //}

        /// <inheritdoc />
        public bool WasSignaled
        {
            get { return m_signaled; }
        }

        /// <inheritdoc />
        protected override void PlatformReset()
        {
            if (!m_signaled)
                return;

            Interlocked.Decrement(ref m_fenceValue);
        }

        public void Wait()
        {
            lock(m_lock)
            {
                if (!m_signaled)
                    return;

                using var fEvent = new AutoResetEvent(false);
                m_resource.SetEventOnCompletion((ulong)CompletedValue,
                                                fEvent.SafeWaitHandle.DangerousGetHandle());
                fEvent.WaitOne();
                fEvent.Close();
                
                m_signaled = false;
            }
        }

        public void Signal()
        {
            if (m_signaled)
                return;
            
            lock(m_lock)
            {
                if (m_resource.CompletedValue < m_fenceValue)
                    return;

                Interlocked.Increment(ref m_fenceValue);
                m_signaled = true;
            }
        }

        //public void Signal(int value)
        //{
        //    if (m_resource.CompletedValue < m_fenceValue)
        //        return;

        //    Interlocked.Exchange(ref m_fenceValue, (ulong)value);
        //    m_signaled = true;
        //}
    }
}
