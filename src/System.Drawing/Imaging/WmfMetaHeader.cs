using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
    [StructLayout (LayoutKind.Sequential, Pack=2)]
    internal struct WmfMetaHeader {
        // field order match: http://wvware.sourceforge.net/caolan/ora-wmf.html
        // for WMFHEAD structure
        public short file_type;
        public short header_size;
        public short version;
        // this is unaligned and fails on the SPARC architecture (see bug #81254 for details)
        // public int file_size;
        public ushort file_size_low;
        public ushort file_size_high;
        public short num_of_objects;
        public int max_record_size;
        public short num_of_params;
    }
}