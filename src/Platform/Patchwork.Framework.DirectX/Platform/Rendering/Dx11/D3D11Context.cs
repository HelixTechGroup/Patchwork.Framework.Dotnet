#region Usings
using System;
using System.Drawing;
using Patchwork.Framework.Platform.Rendering.Resources;
using Patchwork.Framework.Platform.Windowing;
using Vortice.Direct3D11;
using VDXGI = Vortice.DXGI;
using VD3D11 = Vortice.Direct3D11;
#endregion

//using Vortice.Direct2D1;

namespace Patchwork.Framework.Platform.Rendering.Dx11
{
    public class D3D11Context : NRenderContext<VD3D11.ID3D11DeviceContext4>, INRenderContext3D
    {
        #region Members
        private VD3D11.ID3D11Device5 m_dx11Device;
        #endregion

        internal D3D11Context(INRenderDevice device, VDXGI.IDXGIDevice dxgiDevice):
            base(device)
        {
            m_dx11Device = dxgiDevice.QueryInterface<ID3D11Device5>();
        }

        /// <inheritdoc />
        protected override void PlatformResize(INWindow window, Size size)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override bool PlatformBind(INWindow window, bool force = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override bool PlatformUnbind(INWindow window, bool force = false)
        {
            throw new NotImplementedException();
        }

        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            //m_resource = m_dx11Device.ImmediateContext
            //                        .QueryInterface<ID3D11DeviceContext4>();

            using var context = m_dx11Device.CreateDeferredContext();
            m_resource = context.QueryInterface<ID3D11DeviceContext4>();
            //var rtv = m_dx1
            //m_originalTarget = m_resource.Target;

            return m_resource != null;
        }

        /// <inheritdoc />
        protected override bool PlatformBegin()
        {
            //using var mt = m_dx11Device.QueryInterface<ID3D11Multithread>();
            //mt.Enter();
            return true;
        }

        /// <inheritdoc />
        protected override void PlatformClear(Color color)
        {
            //m_resource.ClearRenderTargetView(new );
        }

        /// <inheritdoc />
        protected override INRenderResource PlatformBind(INRenderResource resource)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override INRenderResource PlatformUnbind(INRenderResource resource)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        //protected override object PlatformClone()
        //{
        //    return new D3D11Context(m_device, m_dx11Device.QueryInterface<VDXGI.IDXGIDevice>());
        //}

        /// <inheritdoc />
        protected override bool PlatformEnd()
        {
            Flush();
            //using var mt = m_dx11Device.QueryInterface<ID3D11Multithread>();
            //mt.Leave();

            //m_resource.FinishCommandList(true, out var commandList);
            //m_dx11Device.ImmediateContext3.ExecuteCommandList(commandList, true);
            return true;
        }

        /// <inheritdoc />
        protected override void PlatformFlush()
        {
            m_resource.Flush();
        }
    }
}