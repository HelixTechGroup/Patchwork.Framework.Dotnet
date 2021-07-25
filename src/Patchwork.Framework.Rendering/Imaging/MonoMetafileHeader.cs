using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
    [StructLayout(LayoutKind.Explicit)]
    struct MonoMetafileHeader {
        [FieldOffset (0)]
        public MetafileType	type;
        [FieldOffset (4)]
        public int		size;
        [FieldOffset (8)]
        public int		version;
        [FieldOffset (12)]
        public int		emf_plus_flags;
        [FieldOffset (16)]
        public float		dpi_x;
        [FieldOffset (20)]
        public float		dpi_y;
        [FieldOffset (24)]
        public int		x;
        [FieldOffset (28)]
        public int		y;
        [FieldOffset (32)]
        public int		width;
        [FieldOffset (36)]
        public int		height;
        [FieldOffset (40)]
        public WmfMetaHeader	wmf_header;
        [FieldOffset (40)]
        public EnhMetafileHeader emf_header;
        [FieldOffset (128)]
        public int		emfplus_header_size;
        [FieldOffset (132)]
        public int		logical_dpi_x;
        [FieldOffset (136)]
        public int		logical_dpi_y;
    }
}