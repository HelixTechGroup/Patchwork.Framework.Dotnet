using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Rendering;

namespace Hatzap.Rendering
{
    public class NFrameBuffer : INObject
    {
        protected INHandle m_handle;
        protected NPixelBuffer m_pixelBuffer;

        /// <inheritdoc />
        public INHandle Handle
        {
            get { return m_handle; }
        }

        public NPixelBuffer PixelBuffer
        {
            get { return m_pixelBuffer; }
        }

        public NFrameBuffer(int imageWidth, int imageHeight)
        {
            //m_pixelBuffer = new NPixelBuffer(imageWidth, imageHeight);
        }
    }
}
