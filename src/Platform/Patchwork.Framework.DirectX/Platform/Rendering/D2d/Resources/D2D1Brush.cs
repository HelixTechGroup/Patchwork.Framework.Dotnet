using System.Drawing;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Platform.Rendering.Resources;
using Vortice.Direct2D1;

//using Vortice.Direct2D1;

namespace Patchwork.Framework.Platform.Rendering.D2d.Resources
{
    public class D2D1SolidBrush : D2D1Resource<ID2D1SolidColorBrush>, INBrush
    {
        private Color m_color;

        protected D2D1SolidBrush(INRenderDevice device, ID2D1Factory d2D1Factory, 
                                 ID2D1RenderTarget d2D1RenderTarget, 
                                 Color color) : 
            base(device, d2D1Factory, d2D1RenderTarget)
        {
            m_color = color;
        }

        public D2D1SolidBrush(INRenderDevice device, Color color) : base(device)
        {
            m_color = color;
        }


        /// <param name="force"></param>
        /// <inheritdoc />
        protected override bool CreateResources(bool force)
        {
            m_resource = m_d2D1RenderTarget
               .CreateSolidColorBrush(m_color.ToColor4());

            return true;
        }

        /// <inheritdoc />
        //protected override object PlatformClone()
        //{
        //    return new D2D1SolidBrush(m_device, m_d2D1Factory, m_d2D1RenderTarget, m_color);
        //}

        /// <inheritdoc />
        public Color Color
        {
            get { return m_color; }
            set
            {
                m_color = value;
                m_resource.Color = m_color.ToColor4();
            }
        }
    }
}