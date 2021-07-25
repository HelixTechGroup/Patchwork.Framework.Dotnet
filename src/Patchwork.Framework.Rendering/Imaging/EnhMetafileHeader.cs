using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
    [StructLayout(LayoutKind.Sequential, Pack=2)]
    struct EnhMetafileHeader {
        public int		type;
        public int		size;
        public Rectangle	bounds;
        public Rectangle	frame;
        public int		signature;
        public int		version;
        public int		bytes;
        public int		records;
        public short		handles;
        public short		reserved;
        public int		description;
        public int		off_description;
        public int		palette_entires;
        public Size		device;
        public Size		millimeters;
    }
}