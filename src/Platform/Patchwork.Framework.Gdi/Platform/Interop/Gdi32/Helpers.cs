#region Usings
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Shin.Framework;
using Shin.Framework.Extensions;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;
#endregion

// ReSharper disable InconsistentNaming

namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    public static class Helpers
    {
        #region Methods
        public static IntPtr GetStockObject(StockObject fnObject)
        {
            return Methods.GetStockObject((int)fnObject);
        }

        public static IntPtr CreateDIBSection(IntPtr hdc,
                                              ref BitmapInfo bitmapInfo,
                                              DibBmiColorUsageFlag iUsage,
                                              out IntPtr ppvBits,
                                              IntPtr hSection,
                                              int dwOffset)
        {
            if (bitmapInfo.Colors ==  null || bitmapInfo.Colors.Length == 0)
            {
                var tmp = new List<RgbQuad>();
                var c1 = new RgbQuad()
                {
                    Blue = 0x00,
                    Green = 0x00,
                    Red = 0xFF
                };
                tmp.Add(c1);

                bitmapInfo.Colors = tmp.ToArray();
            }

            using (var pbmi = BitmapInfo.NativeAlloc(ref bitmapInfo))
            {
                return Methods.CreateDIBSection(hdc,
                                                pbmi.GetDangerousHandle(),
                                                iUsage,
                                                out ppvBits,
                                                hSection,
                                                (uint)dwOffset);
            }
        }

        public static unsafe IntPtr CreateDIBSection(IntPtr hdc,
                                                     ref BitmapInfoHeader bitmapInfoHeader,
                                                     DibBmiColorUsageFlag iUsage,
                                                     out IntPtr ppvBits,
                                                     IntPtr hSection, 
                                                     int dwOffset)
        {
            fixed (BitmapInfoHeader* bitmapInfoHeaderPtr = &bitmapInfoHeader)
            {
                return Methods.CreateDIBSection(hdc,
                                                new IntPtr(bitmapInfoHeaderPtr),
                                                iUsage,
                                                out ppvBits,
                                                hSection,
                                                (uint)dwOffset);
            }
        }

        //public static IntPtr CreateDIBitmap(IntPtr hdc,
        //                                    ref BitmapInfoHeader
        //                                        lpbmih,
        //                                    uint fdwInit,
        //                                    byte[] lpbInit,
        //                                    ref BitmapInfo bitmapInfo,
        //                                    DibBmiColorUsageFlag fuUsage)
        //{
        //    using(var pbmi = BitmapInfo.NativeAlloc(ref bitmapInfo))
        //    {
        //        return Methods.CreateDIBitmap(hdc,
        //                                      ref lpbmih,
        //                                      fdwInit,
        //                                      lpbInit,
        //                                      pbmi.GetDangerousHandle(),
        //                                      fuUsage);
        //    }
        //}

        //public static int SetDIBitsToDevice(IntPtr hdc,
        //                                    int xDest,
        //                                    int yDest,
        //                                    uint
        //                                        dwWidth,
        //                                    uint dwHeight,
        //                                    int xSrc,
        //                                    int ySrc,
        //                                    uint uStartScan,
        //                                    uint cScanLines,
        //                                    byte[] lpvBits,
        //                                    ref BitmapInfo bitmapInfo,
        //                                    DibBmiColorUsageFlag fuColorUse)
        //{
        //    using(var pbmi = BitmapInfo.NativeAlloc(ref bitmapInfo))
        //    {
        //        return Methods.SetDIBitsToDevice(hdc,
        //                                         xDest,
        //                                         yDest,
        //                                         dwWidth,
        //                                         dwHeight,
        //                                         xSrc,
        //                                         ySrc,
        //                                         uStartScan,
        //                                         cScanLines,
        //                                         lpvBits,
        //                                         pbmi.GetDangerousHandle(),
        //                                         fuColorUse);
        //    }
        //}

        //public static unsafe int SetDIBitsToDevice(IntPtr hdc,
        //                                           int xDest,
        //                                           int yDest,
        //                                           uint
        //                                               dwWidth,
        //                                           uint dwHeight,
        //                                           int xSrc,
        //                                           int ySrc,
        //                                           uint uStartScan,
        //                                           uint cScanLines,
        //                                           byte[] lpvBits,
        //                                           ref BitmapInfoHeader bitmapInfoHeader,
        //                                           DibBmiColorUsageFlag fuColorUse)
        //{
        //    fixed(BitmapInfoHeader* bitmapInfoHeaderPtr = &bitmapInfoHeader)
        //    {
        //        return Methods.SetDIBitsToDevice(hdc,
        //                                         xDest,
        //                                         yDest,
        //                                         dwWidth,
        //                                         dwHeight,
        //                                         xSrc,
        //                                         ySrc,
        //                                         uStartScan,
        //                                         cScanLines,
        //                                         lpvBits,
        //                                         new IntPtr(bitmapInfoHeaderPtr),
        //                                         fuColorUse);
        //    }
        //}

        public static IntPtr SetBitsToBitmap(IntPtr hdc, int width, int height, byte[] bits, out BitmapInfo bmi, int xSrc = 0, 
           int ySrc = 0, int xDest = 0, int yDest = 0, bool isRgba = true, bool isImageTopDown = true)
        {
            var bi = new BitmapInfoHeader
                     {
                         Size = (uint)Marshal.SizeOf<BitmapInfoHeader>(),
                         Width = width,
                         Height = isImageTopDown ? -height : height,
                         CompressionMode = BitmapCompressionMode.BI_RGB,
                         BitCount = isRgba ? (ushort)32 : (ushort)24,
                         Planes = 1
                     };
            bmi = new BitmapInfo();
            bmi.Header = bi;

            var sdc = GetDC(IntPtr.Zero);
            var bmp = CreateCompatibleBitmap(sdc, width, height);


            var ret = SetBitmapBits(bmp, bits.Length.ToUint(), bits);
            ret = SetStretchBltMode(hdc, StretchBltMode.STRETCH_DELETESCANS);
 
            return SelectObject(hdc, bmp);
        }

        //public static unsafe int SetRgbBitsToDevice(IntPtr hdc, int width, int height, byte[] bits, int xSrc = 0,
        //    int ySrc = 0, int xDest = 0, int yDest = 0, bool isRgba = true, bool isImageTopDown = true)
        //{
        //    fixed (byte* ptr = &bits[0])
        //    {
        //        return SetRgbBitsToDevice(hdc, width, height, (IntPtr)ptr, xSrc, ySrc, xDest, yDest, isRgba);
        //    }
        //}

        //public static unsafe int SetRgbBitsToDevice(IntPtr hdc, int width, int height, IntPtr pixelBufferPtr,
        //    int xSrc = 0,
        //    int ySrc = 0, int xDest = 0, int yDest = 0, bool isRgba = true, bool isImageTopDown = true)
        public static unsafe int SetRgbBitsToDevice(IntPtr hdc, int width, int height, int srcWidth, int srcHeight, byte[] bits, int xSrc = 0,
            int ySrc = 0, int xDest = 0, int yDest = 0, bool isRgba = true, bool isImageTopDown = true)
        {
            var bi = new BitmapInfoHeader
            {
                Size = (uint)Marshal.SizeOf<BitmapInfoHeader>(),
                Width = width,
                Height = isImageTopDown ? -height : height,
                CompressionMode = BitmapCompressionMode.BI_RGB,
                BitCount = isRgba ? (ushort)32 : (ushort)24,
                Planes = 1
            };
            var bmi = new BitmapInfo();
            bmi.Header = bi;
            //bmi.Colors = new RgbQuad[256];

            var ret = SetStretchBltMode(hdc, StretchBltMode.STRETCH_DELETESCANS);
            return StretchDIBits(hdc,
                                         xDest,
                                         yDest,
                                         width,
                                         height,
                                         xSrc,
                                         ySrc,
                                         srcWidth,
                                         srcHeight,
                                         bits,
                                         ref bmi,
                                         DibBmiColorUsageFlag.DIB_RGB_COLORS,
                                         BitBltFlags.SRCCOPY);

            //return Methods.SetDIBitsToDevice(hdc, xDest, yDest, (uint)width, (uint)height, xSrc, ySrc, 0,
            //    (uint)height, pixelBufferPtr, new IntPtr(&bi),
            //    DibBmiColorUsageFlag.DIB_RGB_COLORS);
        }

        public static IntPtr CreateSolidBrush(uint r, uint g, uint b)
        {
            return Methods.CreateSolidBrush(Bgr32(r, g, b));
        }

        public static int Bgr32(uint r, uint g, uint b)
        {
            return (int)(r | (g << 8) | (b << 16));
        }
        #endregion
    }
}