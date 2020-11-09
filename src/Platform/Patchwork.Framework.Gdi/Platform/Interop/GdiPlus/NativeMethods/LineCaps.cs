using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods
{
    internal partial class GdiPlus
    {
        [DllImport(dllName)]
        internal static extern GpStatus GdipCreateCustomLineCap(GpPath fillPath, GpPath strokePath, LineCap baseCap, float baseInset, out GpCustomLineCap customCap);

        [DllImport(dllName)]
        internal static extern GpStatus GdipDeleteCustomLineCap(GpCustomLineCap customCap);

        [DllImport(dllName)]
        internal static extern GpStatus GdipSetCustomLineCapStrokeCaps(GpCustomLineCap customCap, GpLineCap startCap, GpLineCap endCap);


    }
}
