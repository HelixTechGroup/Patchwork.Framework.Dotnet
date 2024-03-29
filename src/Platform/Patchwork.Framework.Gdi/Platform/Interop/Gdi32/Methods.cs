﻿#region Usings
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Patchwork.Framework.Platform.Interop.User32;
#endregion

namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    public static class Methods
    {
        #region Members
        public const string LibraryName = "gdi32";
        #endregion

        #region Methods
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr GetStockObject(int fnObject);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern uint SetPixel(IntPtr hdc, int x, int y, uint crColor);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int GetPixelFormat(IntPtr hdc);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int SetBkMode(IntPtr hdc, int iBkMode);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern uint SetBkColor(
            IntPtr hdc,
            int crColor
        );

        [DllImport(LibraryName)]
        public static extern int GetDIBits([In] IntPtr hdc,
                                    [In] IntPtr hbmp,
                                    uint uStartScan,
                                    uint cScanLines,
                                    [Out] byte[] lpvBits,
                                    ref BitmapInfo lpbi,
                                    DibBmiColorUsageFlag uUsage);

        [DllImport(LibraryName)]
        public static extern int SetDIBits(
            [In] IntPtr hdc,
            [In] IntPtr hbmp,
            uint uStartScan,
            uint cScanLines,
            byte[] lpBits,
            ref BitmapInfo lpbi,
            DibBmiColorUsageFlag uUsage

        );

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc,
                                                     IntPtr pbmi,
                                                     DibBmiColorUsageFlag iUsage,
                                                     out IntPtr ppvBits,
                                                     IntPtr hSection,
                                                     uint dwOffset);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport(LibraryName, CharSet = Properties.BuildCharSet)]
        public static extern int GetObject(IntPtr hgdiobj, int cbBuffer, IntPtr lpvObject);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, IntPtr lpvBits);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateBitmapIndirect([In] ref Bitmap lpbm);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateDIBitmap(IntPtr hdc,
                                                   [In] ref BitmapInfoHeader
                                                       lpbmih,
                                                   uint fdwInit,
                                                   byte[] lpbInit,
                                                   IntPtr lpbmi,
                                                   DibBmiColorUsageFlag fuUsage);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateDIBitmap(IntPtr hdc,
                                                   [In] ref BitmapInfoHeader
                                                       lpbmih,
                                                   uint fdwInit,
                                                   IntPtr lpbInit,
                                                   IntPtr lpbmi,
                                                   DibBmiColorUsageFlag fuUsage);

        //[DllImport(LibraryName, ExactSpelling = true)]
        //public static extern int SetDIBitsToDevice(IntPtr hdc,
        //                                           int xDest,
        //                                           int yDest,
        //                                           uint dwWidth,
        //                                           uint dwHeight,
        //                                           int xSrc,
        //                                           int ySrc,
        //                                           uint uStartScan,
        //                                           uint cScanLines,
        //                                           byte[] lpvBits,
        //                                           BitmapInfo lpbmi,
        //                                           DibBmiColorUsageFlag fuColorUse);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int SetDIBitsToDevice(IntPtr hdc,
                                                   int xDest,
                                                   int yDest,
                                                   uint dwWidth,
                                                   uint dwHeight,
                                                   int xSrc,
                                                   int ySrc,
                                                   uint uStartScan,
                                                   uint cScanLines,
                                                   IntPtr lpvBits,
                                                   BitmapInfo lpbmi,
                                                   DibBmiColorUsageFlag fuColorUse);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int SetDIBitsToDevice(IntPtr hdc,
                                                   int xDest,
                                                   int yDest,
                                                   uint
                                                       dwWidth,
                                                   uint dwHeight,
                                                   int xSrc,
                                                   int ySrc,
                                                   uint uStartScan,
                                                   uint cScanLines,
                                                   IntPtr lpvBits,
                                                   IntPtr lpbmi,
                                                   DibBmiColorUsageFlag fuColorUse);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int SetDIBitsToDevice(IntPtr hdc,
                                            int XDest,
                                            int YDest,
                                            uint dwWidth,
                                            uint dwHeight,
                                            int XSrc,
                                            int YSrc,
                                            uint uStartScan,
                                            uint cScanLines,
                                            byte[] lpvBits,
                                            [In] ref BitmapInfo lpbmi,
                                            DibBmiColorUsageFlag fuColorUse);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int SetBitmapBits(IntPtr hbmp, uint cBytes, [In] byte[] lpBits);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int GetBitmapBits(IntPtr hbmp, int cbBuffer, [Out] byte[] lpvBits);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int SetBitmapBits(IntPtr hbmp, uint cBytes, IntPtr lpBits);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int GetBitmapBits(IntPtr hbmp, int cbBuffer, IntPtr lpvBits);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateRectRgnIndirect([In] ref Rectangle lprc);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateEllipticRgn(int nLeftRect,
                                                      int nTopRect,
                                                      int nRightRect,
                                                      int nBottomRect);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateEllipticRgnIndirect([In] ref Rectangle lprc);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateRoundRectRgn(int x1,
                                                       int y1,
                                                       int x2,
                                                       int y2,
                                                       int cx,
                                                       int cy);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int CombineRgn(IntPtr hrgnDest,
                                            IntPtr hrgnSrc1,
                                            IntPtr hrgnSrc2,
                                            RegionModeFlags fnCombineMode);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool OffsetViewportOrgEx(IntPtr hdc, int nXOffset, int nYOffset, out Point lpPoint);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool SetViewportOrgEx(IntPtr hdc, int x, int y, out Point lpPoint);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int SetMapMode(IntPtr hdc, MapModes fnMapMode);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool SetViewportOrgEx(IntPtr hDC, int x, int y, Point[] prevPoint);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool SetWindowOrgEx(IntPtr hDC, int x, int y, Point[] prevPoint);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool SetViewportExtEx(IntPtr hDC, int nExtentX, int nExtentY, Size[] prevSize);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool SetWindowExtEx(IntPtr hDC, int nExtentX, int nExtentY, Size[] prevSize);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool SetBrushOrgEx(IntPtr hDC, int x, int y, Point[] prevPoint);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int CreatePen(int nPenStyle, int nWidth, int nColor);


        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int SelectObject(IntPtr hDC, int hGdiObject);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int DeleteObject(int hBitmap);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int MoveToEx(IntPtr hDC, int x, int y, int nPreviousPoint);

        [DllImport(LibraryName, ExactSpelling = true, EntryPoint = "Rectangle")]
        public static extern int DrawRectangle(IntPtr hDC, int nLeft, int nTop, int nRight, int nBottom);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool DPtoLP(IntPtr hdc, [In, Out] Point[] lpPoints, int nCount);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool SwapBuffers(IntPtr hdc);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool GdiFlush();

        /// <summary>
        ///     The GetClipBox function retrieves the dimensions of the tightest bounding rectangle that can be drawn around the
        ///     current visible area on the device. The visible area is defined by the current clipping region or clip path, as
        ///     well as any overlapping windows.
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <param name="lprc">A pointer to a RECT structure that is to receive the rectangle dimensions, in logical units.</param>
        /// <returns>If the function succeeds, the return value specifies the clipping box's complexity.</returns>
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern RegionType GetClipBox(IntPtr hdc, out Rectangle lprc);

        /// <summary>
        ///     The GetClipRgn function retrieves a handle identifying the current application-defined clipping region for the
        ///     specified device context.
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <param name="hrgn">
        ///     A handle to an existing region before the function is called. After the function returns, this
        ///     parameter is a handle to a copy of the current clipping region.
        /// </param>
        /// <returns>
        ///     f the function succeeds and there is no clipping region for the given device context, the return value is
        ///     zero. If the function succeeds and there is a clipping region for the given device context, the return value is 1.
        ///     If an error occurs, the return value is -1.
        /// </returns>
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int GetClipRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern RegionType SelectClipRgn(IntPtr hdc, IntPtr hrgn);

        /// <summary>
        ///     The ExtSelectClipRgn function combines the specified region with the current clipping region using the specified
        ///     mode.
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <param name="hrgn">
        ///     A handle to the region to be selected. This handle must not be NULL unless the RGN_COPY mode is
        ///     specified.
        /// </param>
        /// <param name="fnMode">The operation to be performed</param>
        /// <returns>The return value specifies the new clipping region's complexity.</returns>
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern RegionType ExtSelectClipRgn(IntPtr hdc, IntPtr hrgn, RegionModeFlags fnMode);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern RegionType IntersectClipRect(IntPtr hdc,
                                                          int nLeftRect,
                                                          int nTopRect,
                                                          int nRightRect,
                                                          int nBottomRect
        );

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern RegionType ExcludeClipRect(IntPtr hdc,
                                                        int nLeftRect,
                                                        int nTopRect,
                                                        int nRightRect,
                                                        int nBottomRect
        );

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern RegionType OffsetClipRgn(IntPtr hdc,
                                                      int nXOffset,
                                                      int nYOffset
        );

        /// <summary>
        ///     The GetMetaRgn function retrieves the current metaregion for the specified device context.
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <param name="hrgn">
        ///     A handle to an existing region before the function is called. After the function returns, this
        ///     parameter is a handle to a copy of the current metaregion.
        /// </param>
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool GetMetaRgn(IntPtr hdc, IntPtr hrgn);

        /// <summary>
        ///     The SetMetaRgn function intersects the current clipping region for the specified device context with the current
        ///     metaregion and saves the combined region as the new metaregion for the specified device context. The clipping
        ///     region is reset to a null region.
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <returns>The return value specifies the new clipping region's complexity.</returns>
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern RegionType SetMetaRgn(IntPtr hdc);

        /// <summary>
        ///     The LPtoDP function converts logical coordinates into device coordinates. The conversion depends on the mapping
        ///     mode of the device context, the settings of the origins and extents for the window and viewport, and the world
        ///     transformation.
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <param name="lpPoints">
        ///     A pointer to an array of POINT structures. The x-coordinates and y-coordinates contained in each
        ///     of the POINT structures will be transformed.
        /// </param>
        /// <param name="nCount">The number of points in the array.</param>
        /// <remarks>
        ///     The LPtoDP function fails if the logical coordinates exceed 32 bits, or if the converted device coordinates exceed
        ///     27 bits. In the case of such an overflow, the results for all the points are undefined.
        ///     LPtoDP calculates complex floating-point arithmetic, and it has a caching system for efficiency.Therefore, the
        ///     conversion result of an initial call to LPtoDP might not exactly match the conversion result of a later call to
        ///     LPtoDP.We recommend not to write code that relies on the exact match of the conversion results from multiple calls
        ///     to LPtoDP even if the parameters that are passed to each call are identical.
        /// </remarks>
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool LPtoDP(IntPtr hdc, [In] [Out] ref Point lpPoints, int nCount);

        /// <summary>
        ///     The DPtoLP function converts device coordinates into logical coordinates. The conversion depends on the mapping
        ///     mode of the device context, the settings of the origins and extents for the window and viewport, and the world
        ///     transformation.
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <param name="lpPoints">
        ///     A pointer to an array of POINT structures. The x-coordinates and y-coordinates contained in each
        ///     of the POINT structures will be transformed.
        /// </param>
        /// <param name="nCount">The number of points in the array.</param>
        /// <remarks>
        ///     The DPtoLP function fails if the device coordinates exceed 27 bits, or if the converted logical coordinates exceed
        ///     32 bits. In the case of such an overflow, the results for all the points are undefined.
        /// </remarks>
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool DPtoLP(IntPtr hdc, [In] [Out] ref Point lpPoints, int nCount);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool SelectClipPath(IntPtr hdc, RegionModeFlags iMode);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool FillRgn(IntPtr hdc, IntPtr hrgn, IntPtr hbr);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreateSolidBrush(int crColor);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool FrameRgn(IntPtr hdc,
                                           IntPtr hrgn,
                                           IntPtr hbr,
                                           int nWidth,
                                           int nHeight);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool PaintRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool InvertRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool MoveToEx(IntPtr hdc, int x, int y, out Point lpPoint);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool RoundRect(IntPtr hdc,
                                            int nLeftRect,
                                            int nTopRect,
                                            int nRightRect,
                                            int nBottomRect,
                                            int nWidth,
                                            int nHeight);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport(LibraryName, CharSet = Properties.BuildCharSet)]
        public static extern bool TextOut(IntPtr hdc,
                                          int nXStart,
                                          int nYStart,
                                          string lpString,
                                          int cbString);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool BitBlt(IntPtr hdc,
                                         int nXDest,
                                         int nYDest,
                                         int nWidth,
                                         int nHeight,
                                         IntPtr hdcSrc,
                                         int nXSrc,
                                         int nYSrc,
                                         BitBltFlags dwRop);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool StretchBlt(IntPtr hdcDest,
                                             int nXOriginDest,
                                             int nYOriginDest,
                                             int nWidthDest,
                                             int nHeightDest,
                                             IntPtr hdcSrc,
                                             int nXOriginSrc,
                                             int nYOriginSrc,
                                             int nWidthSrc,
                                             int nHeightSrc,
                                             BitBltFlags dwRop);

        [DllImport(LibraryName, SetLastError = false, ExactSpelling = true)]
        public static extern int SetStretchBltMode(IntPtr hdc, StretchBltMode iStretchMode);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int StretchDIBits(IntPtr hdc,
                                               int XDest,
                                               int YDest,
                                               int nDestWidth,
                                               int nDestHeight,
                                               int XSrc,
                                               int YSrc,
                                               int nSrcWidth,
                                               int nSrcHeight,
                                               byte[] lpBits,
                                               [In] ref BitmapInfo lpBitsInfo,
                                               DibBmiColorUsageFlag iUsage,
                                               BitBltFlags dwRop);

        public enum StretchBltMode : int
        {
            STRETCH_ANDSCANS = 1,
            STRETCH_ORSCANS = 2,
            STRETCH_DELETESCANS = 3,
            STRETCH_HALFTONE = 4,
        }

    [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int SaveDC(IntPtr hdc);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool RestoreDC(IntPtr hdc, int nSavedDc);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr PathToRegion(IntPtr hdc);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreatePolygonRgn(
            Point[] lppt,
            int cPoints,
            int fnPolyFillMode);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern IntPtr CreatePolyPolygonRgn(Point[] lppt,
                                                         int[] lpPolyCounts,
                                                         int nCount,
                                                         int fnPolyFillMode);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int GetDeviceCaps(IntPtr hdc, DeviceCap nIndex);

        [DllImport(LibraryName, ExactSpelling = true, EntryPoint = "GdiAlphaBlend")]
        public static extern bool AlphaBlend(IntPtr hdcDest,
                                             int nXOriginDest,
                                             int nYOriginDest,
                                             int nWidthDest,
                                             int nHeightDest,
                                             IntPtr hdcSrc,
                                             int nXOriginSrc,
                                             int nYOriginSrc,
                                             int nWidthSrc,
                                             int nHeightSrc,
                                             BlendFunction blendFunction);

        /// <summary>
        ///     The GetRandomRgn function copies the system clipping region of a specified device context to a specific region.
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <param name="hrgn">
        ///     A handle to a region. Before the function is called, this identifies an existing region. After the
        ///     function returns, this identifies a copy of the current system region. The old region identified by hrgn is
        ///     overwritten.
        /// </param>
        /// <param name="iNum">This parameter must be SYSRGN.</param>
        /// <returns>
        ///     If the function succeeds, the return value is 1. If the function fails, the return value is -1. If the region
        ///     to be retrieved is NULL, the return value is 0. If the function fails or the region to be retrieved is NULL, hrgn
        ///     is not initialized.
        /// </returns>
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int GetRandomRgn(IntPtr hdc, IntPtr hrgn, DcRegionType iNum);

        /// <summary>
        ///     The OffsetRgn function moves a region by the specified offsets.
        /// </summary>
        /// <param name="hrgn">Handle to the region to be moved.</param>
        /// <param name="nXOffset">Specifies the number of logical units to move left or right.</param>
        /// <param name="nYOffset">Specifies the number of logical units to move up or down.</param>
        /// <returns>The return value specifies the new region's complexity. </returns>
        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern RegionType OffsetRgn(IntPtr hrgn, int nXOffset, int nYOffset);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern RegionType GetRgnBox(IntPtr hWnd, out Rectangle lprc);
        #endregion
    }

    public enum DeviceCap
    {
        /// <summary>
        /// Device driver version
        /// </summary>
        DRIVERVERSION = 0,
        /// <summary>
        /// Device classification
        /// </summary>
        TECHNOLOGY = 2,
        /// <summary>
        /// Horizontal size in millimeters
        /// </summary>
        HORZSIZE = 4,
        /// <summary>
        /// Vertical size in millimeters
        /// </summary>
        VERTSIZE = 6,
        /// <summary>
        /// Horizontal width in pixels
        /// </summary>
        HORZRES = 8,
        /// <summary>
        /// Vertical height in pixels
        /// </summary>
        VERTRES = 10,
        /// <summary>
        /// Number of bits per pixel
        /// </summary>
        BITSPIXEL = 12,
        /// <summary>
        /// Number of planes
        /// </summary>
        PLANES = 14,
        /// <summary>
        /// Number of brushes the device has
        /// </summary>
        NUMBRUSHES = 16,
        /// <summary>
        /// Number of pens the device has
        /// </summary>
        NUMPENS = 18,
        /// <summary>
        /// Number of markers the device has
        /// </summary>
        NUMMARKERS = 20,
        /// <summary>
        /// Number of fonts the device has
        /// </summary>
        NUMFONTS = 22,
        /// <summary>
        /// Number of colors the device supports
        /// </summary>
        NUMCOLORS = 24,
        /// <summary>
        /// Size required for device descriptor
        /// </summary>
        PDEVICESIZE = 26,
        /// <summary>
        /// Curve capabilities
        /// </summary>
        CURVECAPS = 28,
        /// <summary>
        /// Line capabilities
        /// </summary>
        LINECAPS = 30,
        /// <summary>
        /// Polygonal capabilities
        /// </summary>
        POLYGONALCAPS = 32,
        /// <summary>
        /// Text capabilities
        /// </summary>
        TEXTCAPS = 34,
        /// <summary>
        /// Clipping capabilities
        /// </summary>
        CLIPCAPS = 36,
        /// <summary>
        /// Bitblt capabilities
        /// </summary>
        RASTERCAPS = 38,
        /// <summary>
        /// Length of the X leg
        /// </summary>
        ASPECTX = 40,
        /// <summary>
        /// Length of the Y leg
        /// </summary>
        ASPECTY = 42,
        /// <summary>
        /// Length of the hypotenuse
        /// </summary>
        ASPECTXY = 44,
        /// <summary>
        /// Shading and Blending caps
        /// </summary>
        SHADEBLENDCAPS = 45,

        /// <summary>
        /// Logical pixels inch in X
        /// </summary>
        LOGPIXELSX = 88,
        /// <summary>
        /// Logical pixels inch in Y
        /// </summary>
        LOGPIXELSY = 90,

        /// <summary>
        /// Number of entries in physical palette
        /// </summary>
        SIZEPALETTE = 104,
        /// <summary>
        /// Number of reserved entries in palette
        /// </summary>
        NUMRESERVED = 106,
        /// <summary>
        /// Actual color resolution
        /// </summary>
        COLORRES = 108,

        // Printing related DeviceCaps. These replace the appropriate Escapes
        /// <summary>
        /// Physical Width in device units
        /// </summary>
        PHYSICALWIDTH = 110,
        /// <summary>
        /// Physical Height in device units
        /// </summary>
        PHYSICALHEIGHT = 111,
        /// <summary>
        /// Physical Printable Area x margin
        /// </summary>
        PHYSICALOFFSETX = 112,
        /// <summary>
        /// Physical Printable Area y margin
        /// </summary>
        PHYSICALOFFSETY = 113,
        /// <summary>
        /// Scaling factor x
        /// </summary>
        SCALINGFACTORX = 114,
        /// <summary>
        /// Scaling factor y
        /// </summary>
        SCALINGFACTORY = 115,

        /// <summary>
        /// Current vertical refresh rate of the display device (for displays only) in Hz
        /// </summary>
        VREFRESH = 116,
        /// <summary>
        /// Vertical height of entire desktop in pixels
        /// </summary>
        DESKTOPVERTRES = 117,
        /// <summary>
        /// Horizontal width of entire desktop in pixels
        /// </summary>
        DESKTOPHORZRES = 118,
        /// <summary>
        /// Preferred blt alignment
        /// </summary>
        BLTALIGNMENT = 119
    }
}