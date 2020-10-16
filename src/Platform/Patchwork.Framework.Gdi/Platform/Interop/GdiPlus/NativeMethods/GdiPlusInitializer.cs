using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods
{
    internal partial class GdiPlus
    {
        [DllImport(dllName)]
        extern static internal GpStatus GdiplusStartup(
            out IntPtr token,
            GdiplusStartupInput input,
            out GdiplusStartupOutput output);

        [DllImport(dllName)]
        extern static internal void GdiplusShutdown(IntPtr token);

        IntPtr token;
        GdiplusStartupInput input;
        GdiplusStartupOutput output;

        /// <summary>
        /// It's important to call GdiplusStartup before any other GdiPlus method can be called, therefore
        /// a static GdiPlus is instantiated which executes GdiplusStartup once.
        /// </summary>
        private static GdiPlus instance = new GdiPlus();

        /// <summary>
        /// Initializes the GdiPlus interface.
        /// </summary>
        private GdiPlus()
        {
            input = new GdiplusStartupInput();
            GdiPlus.GdiplusStartup(out token, input, out output);
        }

        /// <summary>
        /// Shutdown the gdiplus engine.
        /// This happens when the static GdiPlus instance is destroyed which means the application has shutdown.
        /// </summary>
        ~GdiPlus()
        {
           //if (token!= IntPtr.Zero) GdiPlus.GdiplusShutdown(token);
        }

        // Converts a status into exception
        // TODO: Add more status code mappings here
        public static void CheckStatus(GpStatus status)
        {
            string msg;
            switch (status)
            {
                case GpStatus.Ok:
                    return;
                case GpStatus.GenericError:
                    msg = string.Format("Generic Error [GDI+ status: {0}]", status);
                    throw new Exception(msg);
                case GpStatus.InvalidParameter:
                    msg = string.Format("A null reference or invalid value was found [GDI+ status: {0}]", status);
                    throw new ArgumentException(msg);
                case GpStatus.OutOfMemory:
                    msg = string.Format("Not enough memory to complete operation [GDI+ status: {0}]", status);
                    throw new OutOfMemoryException(msg);
                case GpStatus.ObjectBusy:
                    msg = string.Format("Object is busy and cannot state allow this operation [GDI+ status: {0}]", status);
                    throw new MemberAccessException(msg);
                case GpStatus.InsufficientBuffer:
                    msg = string.Format("Insufficient buffer provided to complete operation [GDI+ status: {0}]", status);
                    throw new InternalBufferOverflowException(msg);
                case GpStatus.PropertyNotSupported:
                    msg = string.Format("Property not supported [GDI+ status: {0}]", status);
                    throw new NotSupportedException(msg);
                case GpStatus.FileNotFound:
                    msg = string.Format("Requested file was not found [GDI+ status: {0}]", status);
                    throw new FileNotFoundException(msg);
                case GpStatus.AccessDenied:
                    msg = string.Format("Access to resource was denied [GDI+ status: {0}]", status);
                    throw new UnauthorizedAccessException(msg);
                case GpStatus.UnknownImageFormat:
                    msg = string.Format("Either the image format is unknown or you don't have the required libraries to decode this format [GDI+ status: {0}]", status);
                    throw new NotSupportedException(msg);
                case GpStatus.NotImplemented:
                    msg = string.Format("The requested feature is not implemented [GDI+ status: {0}]", status);
                    throw new NotImplementedException(msg);
                case GpStatus.WrongState:
                    msg = string.Format("Object is not in a state that can allow this operation [GDI+ status: {0}]", status);
                    throw new ArgumentException(msg);
                case GpStatus.FontFamilyNotFound:
                    msg = string.Format("The requested FontFamily could not be found [GDI+ status: {0}]", status);
                    throw new ArgumentException(msg);
                case GpStatus.ValueOverflow:
                    msg = string.Format("Argument is out of range [GDI+ status: {0}]", status);
                    throw new OverflowException(msg);
                case GpStatus.Win32Error:
                    msg = string.Format("The operation is invalid [GDI+ status: {0}]", status);
                    throw new InvalidOperationException(msg);
                default:
                    msg = string.Format("Unknown Error [GDI+ status: {0}]", status);
                    throw new Exception(msg);
            }
        }

        //--------------------------------------------------------------------------
        // Unit constants
        //--------------------------------------------------------------------------

        //public enum Unit
        //{
        //    UnitWorld, // 0 -- World coordinate (non-physical unit)
        //    UnitDisplay, // 1 -- Variable -- for PageTransform only
        //    UnitPixel, // 2 -- Each unit is one device pixel.
        //    UnitPoint, // 3 -- Each unit is a printer's point, or 1/72 inch.
        //    UnitInch, // 4 -- Each unit is 1 inch.
        //    UnitDocument, // 5 -- Each unit is 1/300 inch.
        //    UnitMillimeter // 6 -- Each unit is 1 millimeter.
        //};

        public enum ImageType
        {
            Unknown, // 0
            Bitmap, // 1
            Metafile // 2
        };

        public enum BrushType
        {
            SolidColor = 0,
            HatchFill = 1,
            TextureFill = 2,
            PathGradient = 3,
            LinearGradient = 4
        }
    }
}
