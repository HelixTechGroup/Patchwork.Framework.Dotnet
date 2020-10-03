#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

namespace Patchwork.Framework.Platform.Interop.Gdi32
{
    public class NativeBitmapInfoHandle : CriticalHandle
    {
        #region Properties
        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }
        #endregion

        public unsafe NativeBitmapInfoHandle(ref BitmapInfo bitmapInfo) : base(new IntPtr(0))
        {
            var quads = bitmapInfo.Colors;
            var quadsLength = quads.Length;
            if (quadsLength == 0) quadsLength = 1;
            var success = false;
            var ptr = IntPtr.Zero;
            try
            {
                ptr =
                    Marshal.AllocHGlobal(Marshal.SizeOf<BitmapInfoHeader>() + Marshal.SizeOf<RgbQuad>() * quadsLength);
                var headerPtr = (BitmapInfoHeader*)ptr.ToPointer();
                *headerPtr = bitmapInfo.Header;
                var quadPtr = (RgbQuad*)(headerPtr + 1);
                var i = 0;
                for (; i < quads.Length; i++) *(quadPtr + i) = quads[i];
                if (i == 0) *quadPtr = new RgbQuad();
                SetHandle(ptr);
                success = true;
            }
            finally
            {
                if (!success)
                {
                    SetHandleAsInvalid();
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        #region Methods
        public IntPtr GetDangerousHandle()
        {
            return handle;
        }

        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(handle);
            return true;
        }
        #endregion
    }
}