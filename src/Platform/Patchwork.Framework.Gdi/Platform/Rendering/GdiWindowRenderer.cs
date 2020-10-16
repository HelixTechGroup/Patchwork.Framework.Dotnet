#region Usings
//using Patchwork.Framework.Platform.Interop.GdiPlus;
//using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Interop.Gdi32;
using Patchwork.Framework.Platform.Interop.User32;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Helpers;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class GdiWindowRenderer : NWindowRenderer, IFrameBufferRenderer
    {
        #region Members
        protected IntPtr m_bmp;
        protected NFrameBuffer m_buffer;

        protected IntPtr m_hdc;
        protected IntPtr m_memHdc;
        protected NFrameBuffer m_oldBuffer;

        //protected WindowsProcessHook m_hook;
        protected PaintStruct m_paintStruct;
        #endregion

        public GdiWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice, window)
        {
            //m_hook = new WindowsProcessHook(window as IWindowsProcess, WindowHookType.WH_GETMESSAGE);
            //m_hook.ProcessMessage += OnGetMsg;
            m_buffer = new NFrameBuffer(m_window.ClientSize.Width, m_window.ClientSize.Height);
            renderDevice.ProcessMessage += OnProcessMessage;
        }

        #region Methods
        public bool Validate(ref Rectangle rect)
        {
            return ValidateRect(m_window.Handle.Pointer, ref rect);
        }

        public bool Invalidate(ref Rectangle rect, bool shouldErase)
        {
            return InvalidateRect(m_window.Handle.Pointer, ref rect, shouldErase);
        }

        public bool Invalidate(bool shouldErase)
        {
            return InvalidateRect(m_window.Handle.Pointer, IntPtr.Zero, shouldErase);
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            //m_hook.Initialize();
        }

        protected override void OnProcessMessage(IPlatformMessage message)
        {
            switch (message.Id)
            {
                case MessageIds.Rendering:
                    Core.Logger.LogDebug("Found Rendering Messages.");
                    var data = message.RawData as IRenderMessageData;
                    switch (data?.MessageId)
                    {
                        case RenderMessageIds.None:
                            break;
                        case RenderMessageIds.SetFrameBuffer:
                            m_buffer = data.FrameBuffer;
                            Invalidate();
                            break;
                    }

                    break;
            }
        }

        /// <inheritdoc />
        protected override void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e) { }

        protected override bool PlatformInvalidate()
        {
            return Invalidate(false);
        }

        /// <inheritdoc />
        protected override void PlatformRender()
        {
            if (m_hdc == IntPtr.Zero)
            {
                m_hdc = GetDC(IntPtr.Zero);
                m_memHdc = CreateCompatibleDC(m_hdc);
                m_bmp = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
            }

            //var graphic = new GraphicsPlus(m_hdc);
            //SetBkMode(m_hdc, 1);
            //SetBkColor(m_hdc, 0);
            IntPtr brush = CreateSolidBrush(ColorTranslator.ToWin32(Color.Blue));
            //var gb = new GpBrush();
            //gb.ptr = brush;
            //var b = new BrushPlus();
            //b.SetNativeBrush(gb);
            var rec = m_window.ClientArea;
            SetClassLongPtr(m_window.Handle.Pointer, -10, brush);
            //graphic.FillRectangle(b, rec);
            //var s = graphic.GetLastStatus();
            //graphic.Dispose();
            IntPtr brush2 = CreateSolidBrush(ColorTranslator.ToWin32(Color.Orange));
            FillRect(m_hdc, ref rec, brush2);
            //BitBlt(m_hdc, 0, 0, rec.Width, -rec.Height, m_memHdc, 0, 0, BitBltFlags.SRCCOPY);
            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRender, this);
        }

        /// <inheritdoc />
        protected override void PlatformRendered()
        {
            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendered, this);
            SetBuffer(m_buffer);
            EndPaint(m_window.Handle.Pointer, ref m_paintStruct);
            //ReleaseDC(IntPtr.Zero, m_memHdc);
            DeleteObject(m_bmp);
            //if (GetUpdateRect(m_window.Handle.Pointer, out var rec, false))
            //    Validate(ref rec);
            //Validate();
            m_oldBuffer = m_buffer;
        }

        protected override void PlatformRendering()
        {
            m_hdc = BeginPaint(m_window.Handle.Pointer, out m_paintStruct);

            m_memHdc = CreateCompatibleDC(m_hdc);
            //m_bmp = CreateCompatibleBitmap(m_hdc, m_window.ClientSize.Width, m_window.ClientSize.Height);
            //if (m_bmp == IntPtr.Zero)
            //{
            //    var e = (int)GetLastError();
            //    if (e != 0)
            //        throw new Win32Exception(e);
            //    return;
            //}
            //SelectObject(m_memHdc, m_bmp);

            Core.MessagePump.PushRenderMessage(RenderMessageIds.OsRendering, this);
        }

        protected override bool PlatformValidate()
        {
            return ValidateRect(m_window.Handle.Pointer, IntPtr.Zero);
        }

        protected void SetBuffer(NFrameBuffer buffer)
        {
            //var sdc = GetDC(IntPtr.Zero);
            //var nMemSize = buffer.Width * buffer.Height * 3 * 7;
            //var hBmpMapFile = MemoryMappedFile.CreateOrOpen("bufferBmp", nMemSize, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, HandleInheritability.Inheritable); //CreateFileMapping(INVALID_HANDLE_VALUE, IntPtr.Zero, FileMapProtection.PageReadWrite, 0, (uint)nMemSize, null);

            //var bmi = new BitmapInfo();
            //bmi.Header.Size = (uint)Marshal.SizeOf<BitmapInfoHeader>();
            //var header = bmi.Header;
            //header.Width = buffer.Width; //GetDeviceCaps(m_hdc, DeviceCap.HORZRES);
            //header.Height = buffer.Height; //-GetDeviceCaps(m_hdc, DeviceCap.VERTRES);
            //header.Planes = 1;
            //header.BitCount = 32;
            //header.CompressionMode = BitmapCompressionMode.BI_RGB;

            var pb = buffer.PixelBuffer;
            //var hBitmap = IntPtr.Zero;
            var pBytes = pb.Contents;
            var p = IntPtr.Zero;
            // Call unmanaged code

            //var unmanagedPointer = Marshal.AllocHGlobal(pBytes.Length);
            //sptr = unmanagedPointer;
            //Marshal.Copy(pb.Contents, 0, unmanagedPointer, pb.Length);
            if (pBytes.Length <= 0) return;

            //var hOldBitmap = IntPtr.Zero;
            //using (var mmf = MemoryMappedFile.CreateOrOpen("MyMMF", pBytes.Length))
            //{
            //using (var accessor = mmf.CreateViewAccessor())
            //using(var stream = mmf.CreateViewStream())
            //{
            //    var pixels = new byte[pBytes.Length];//Array.Empty<byte>();
            //hBitmap = CreateCompatibleBitmap(m_hdc,
            //                                 pb.Width,
            //                                 pb.Height);

            //hOldBitmap = SelectObject(m_memHdc, m_bmp);

            //unsafe
            //{
            //byte[] arr = new byte[pBytes.Length];
            //byte* ptr = (byte*)0;
            //accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
            //accessor.WriteArray(0, pixels, 0, pBytes.Length);
            //accessor.Flush();
            //Marshal.Copy(IntPtr.Add(new IntPtr(ptr), 0), pixels, 0, pBytes.Length);
            //accessor.SafeMemoryMappedViewHandle.ReleasePointer();
            //}

            //    BinaryWriter writer = new BinaryWriter(stream);
            //    writer.Write(pBytes);
            //    writer.Flush();
            //    stream.Flush();
            //}

            //byte[] pa;
            //using(var stream = mmf.CreateViewStream())
            //{
            //    using(BinaryReader binReader = new BinaryReader(stream))
            //    {
            //        pa =  binReader.ReadBytes((int)stream.Length);
            //    }
            //var res = GetDIBits(tdc,
            //                            hBitmap,
            //                            0,
            //                            (uint)pb.RowBytes,
            //                            pixels,
            //                            ref bmi,
            //                            DibBmiColorUsageFlag.DIB_RGB_COLORS);

            //if (res != 0)
            //{
            //    var e = (int)GetLastError();
            //    DeleteDC(tdc);
            //    //ReleaseDC(sdc);
            //    if (e != 0)
            //        throw new Win32Exception(e);
            //    break;
            //}

            //var pixelSize = header.BitCount / 8;
            //for (var y = 0; y < pb.Height; y++)
            //{
            //    for (var x = 0; x < pb.Height; x++)
            //    {
            //var pixelOffset = y * pb.Stride + x * pixelSize;
            //var color = Bgr32(
            //                  pixels[pixelOffset + 2],
            //                  pixels[pixelOffset + 1],
            //                  pixels[pixelOffset + 0]);
            //SetPixel(m_hdc, x, y, (uint)color);
            //    }
            //}
            //hBitmap = CreateDIBSection(tdc, 
            //                           ref bmi, 
            //                           DibBmiColorUsageFlag.DIB_RGB_COLORS, 
            //                           out p, 
            //                           //IntPtr.Zero, 
            //                           accessor.SafeMemoryMappedViewHandle.DangerousGetHandle(),
            //                           //stream.SafeMemoryMappedViewHandle.DangerousGetHandle(), 
            //                           0);
            //hBitmap = CreateDIBSection(sdc,
            //                               bmi,
            //                               DibBmiColorUsageFlag.DIB_RGB_COLORS,
            //                               out p,
            //                               mmf.SafeMemoryMappedFileHandle.DangerousGetHandle(),
            //                               //stream.SafeMemoryMappedViewHandle.DangerousGetHandle(),
            //                               //accessor.SafeMemoryMappedViewHandle.DangerousGetHandle(),
            //                               0);
            //}

            //var unmanagedPointer = Marshal.AllocHGlobal(pBytes.Length);
            //hBitmap = unmanagedPointer;
            //Marshal.Copy(pb.Contents, 0, unmanagedPointer, pb.Length);

            //Array.Copy(pBytes, pixels, pBytes.Length);
            //SetDIBits(m_memHdc,
            //          m_bmp,
            //          0,
            //          (uint)pb.RowBytes,
            //          pBytes,
            //          ref bmi,
            //          DibBmiColorUsageFlag.DIB_RGB_COLORS);

            //var pa = new byte[pBytes.Length];
            //    accessor.ReadArray(0, pa, 0, pixels.Length);
            var ret = SetRgbBitsToDevice(m_memHdc,
                                            m_window.ClientArea.Width,
                                            m_window.ClientArea.Height,
                                            pb.Width,
                                            pb.Height,
                                            pBytes);

            //if (ret != 0)
            //    throw new Win32Exception(ret);
            //SetStretchBltMode(m_hdc, StretchBltMode.STRETCH_HALFTONE);
            //var ret = StretchBlt(m_memHdc,
            //           0,
            //           0,
            //           m_window.ClientArea.Width,
            //           m_window.ClientArea.Height,
            //           m_memHdc,
            //           0,
            //           0,
            //           buffer.Width,
            //           buffer.Height,
            //           BitBltFlags.SRCCOPY);

            if (ret != 0)
                throw new Win32Exception();

            var e = (int)GetLastError();
            if (e != 0)
                throw new Win32Exception(e);
            //Marshal.Copy(pBytes, 0, p, pBytes.Length);
            //Marshal.FreeHGlobal(unmanagedPointer);
            //}

            // select new bitmap into memory DC
            //hOldBitmap = SelectObject(m_memHdc, m_bmp);

            if (!StretchBlt(m_hdc,
                   0,
                   0,
                   m_window.ClientArea.Width,
                   m_window.ClientArea.Height,
                   m_memHdc,
                   0,
                   0,
                   pb.Width,
                   pb.Height,
                   BitBltFlags.SRCCOPY))
                throw new Win32Exception();

            e = (int)GetLastError();
            if (e != 0)
                throw new Win32Exception(e);
            // copy from the screen to memory
            //Core.Dispatcher.InvokeAsync(() =>
            //                            {
            //BitBlt(m_hdc,
            //       0,
            //       0,
            //       m_window.ClientArea.Width,
            //       m_window.ClientArea.Height,
            //       m_memHdc,
            //       0,
            //       0,
            //       BitBltFlags.SRCCOPY);
            //                            });

            // clean up
            //SelectObject(m_memHdc, hOldBitmap);

            //var bmp = CreateCompatibleBitmap(m_hdc, data.FrameBuffer.Width, data.FrameBuffer.Height);
            //SelectObject(tdc, bmp);
            //drawing code goes in here
            //BitBlt(m_hdc, 0, 0, data.FrameBuffer.Width, data.FrameBuffer.Height, tdc, 0, 0, BitBltFlags.SRCCOPY);
            //DeleteObject(bmp);
            //DeleteDC(tdc);
            //DeleteObject(hBitmap);
            //hBmpMapFile.Dispose();
            //DeleteDC(hdc);
            //Core.Dispatcher.InvokeAsync(() => 
            //                                SetRgbBitsToDevice(m_hdc, data.FrameBuffer.Width, data.FrameBuffer.Height, data.FrameBuffer.PixelBuffer.Handle.Pointer));
            //}
            //Validate();
        }

        private IntPtr OnGetMsg(WindowsMessage message)
        {
            var res = IntPtr.Zero;
            switch (message.Id)
            {
                case WindowsMessageIds.PAINT:
                    Render();
                    break;
                case WindowsMessageIds.NCCALCSIZE:
                case WindowsMessageIds.SIZE:
                case WindowsMessageIds.MOVE:
                case WindowsMessageIds.WINDOWPOSCHANGED:
                case WindowsMessageIds.WINDOWPOSCHANGING:
                case WindowsMessageIds.ERASEBKGND:
                    break;
                case WindowsMessageIds.DISPLAYCHANGE:
                case WindowsMessageIds.CAPTURECHANGED:
                case WindowsMessageIds.NCHITTEST:
                case WindowsMessageIds.GETMINMAXINFO:
                case WindowsMessageIds.NCPAINT:
                    break;
            }

            return res;
        }
        #endregion
    }
}