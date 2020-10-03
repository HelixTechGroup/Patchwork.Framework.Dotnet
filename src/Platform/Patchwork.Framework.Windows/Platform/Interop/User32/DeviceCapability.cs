#region Usings
using System;
#endregion

namespace Patchwork.Framework.Platform.Interop.User32
{
    [Flags]
    public enum DeviceCapability
    {
        DC_ACTIVE = 0x0001,
        DC_SMALLCAP = 0x0002,
        DC_ICON = 0x0004,
        DC_TEXT = 0x0008,
        DC_INBUTTON = 0x0010,
        DC_GRADIENT = 0x0020,
        DC_BUTTONS = 0x1000,
        DC_HASDEFID = 0x534B,
        DC_BRUSH = 18,
        DC_PEN = 19,
        DC_FIELDS = 1,
        DC_PAPERS = 2,
        DC_PAPERSIZE = 3,
        DC_MINEXTENT = 4,
        DC_MAXEXTENT = 5,
        DC_BINS = 6,
        DC_DUPLEX = 7,
        DC_SIZE = 8,
        DC_EXTRA = 9,
        DC_VERSION = 10,
        DC_DRIVER = 11,
        DC_BINNAMES = 12,
        DC_ENUMRESOLUTIONS = 13,
        DC_FILEDEPENDENCIES = 14,
        DC_TRUETYPE = 15,
        DC_PAPERNAMES = 16,
        DC_ORIENTATION = 17,
        DC_COPIES = 18,
        DC_BINADJUST = 19,
        DC_EMF_COMPLIANT = 20,
        DC_DATATYPE_PRODUCED = 21,
        DC_COLLATE = 22,
        DC_MANUFACTURER = 23,
        DC_MODEL = 24,
        DC_PERSONALITY = 25,
        DC_PRINTRATE = 26,
        DC_PRINTRATEUNIT = 27,
        DC_PRINTERMEM = 28,
        DC_MEDIAREADY = 29,
        DC_STAPLE = 30,
        DC_PRINTRATEPPM = 31,
        DC_COLORDEVICE = 32,
        DC_NUP = 33,
        DC_MEDIATYPENAMES = 34,
        DC_MEDIATYPES = 35
    }
}