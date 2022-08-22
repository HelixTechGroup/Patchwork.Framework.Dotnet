using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
using Shin.Framework.Runtime;
using Shin.Framework.Threading;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Helpers;
using static Patchwork.Framework.Platform.Interop.Utilities;

namespace Patchwork.Framework.Platform.Rendering
{
    internal sealed class GdiHdcManager : Initializable
    {
        private readonly ReaderWriterLockSlim m_lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        //private ConcurrentDictionary<INWindow, IList<INHandle>> m_hdcs;
        private ConcurrentDictionary<INWindow, INHandle> m_hdcs;

        //private SafeMemoryHandle m_memHdc;
        //private SafeMemoryHandle m_hdc;

        public INHandle CurrentWindowHdc
        {
            get
            {
                return this[Core.Window.CurrentWindow];
            }
        }

        public INHandle this[INWindow window]
        {
            get
            {
                m_lockSlim.TryEnter();
                try
                {
                    return /*m_hdcs.ContainsKey(window) ? m_hdcs[window] :*/ CreateHdc(window);
                }
                finally
                {
                    m_lockSlim.TryExit();
                }
            }
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_hdcs = new ConcurrentDictionary<INWindow, INHandle>();
        }

        public INHandle CreateHdc(INWindow window)
        {
            if (!m_isInitialized || m_isDisposed)
                return new NHandle();

            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            var hdc = IntPtr.Zero;
            try
            {
                //if (m_hdcs.ContainsKey(window))
                //    return m_hdcs[window];

                hdc = GetDC(window.Handle.Pointer);
                CheckOperation(hdc != IntPtr.Zero);
                var safeHdc = new NHandle(hdc);
                m_hdcs.TryAdd(window, safeHdc);
            }
            catch (Win32Exception winEx)
            {
                if (hdc != IntPtr.Zero && winEx.NativeErrorCode != 1400)
                    throw;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }

            return (NHandle)hdc;
        }

        public void DestroyHdc(INWindow window, bool delete = false)
        {
            m_lockSlim.TryEnter(SynchronizationAccess.Write);
            try
            {
                if (!m_hdcs.TryGetValue(window, out var safeHdc))
                    return;

                ReleaseDC(window.Handle.Pointer, safeHdc.Pointer);

                if (delete)
                    DeleteDC(safeHdc.Pointer);

                m_hdcs.Remove(window, out safeHdc);

                //if (delete)
                //    hdc = IntPtr.Zero;

                //if (m_memHdc != IntPtr.Zero)
                //{
                //    if (delete)
                //    {
                //        ReleaseDC(hwnd, m_memHdc);
                //        CheckOperation(DeleteDC(m_memHdc));
                //        m_memHdc = IntPtr.Zero;
                //    }
                //}

                //if (m_bmpPtr != IntPtr.Zero)
                //{
                //    if (delete)
                //    {
                //        CheckOperation(DeleteObject(m_bmpPtr));
                //        m_bmpPtr = IntPtr.Zero;
                //    }
                //}
            }
            catch (Win32Exception winEx)
            {
                //if (m_hdc != IntPtr.Zero && winEx.NativeErrorCode != 1400)
                throw;
            }
            finally
            {
                m_lockSlim.TryExit(SynchronizationAccess.Write);
            }
        }

        public INHandle CloneHdc(INWindow window)
        {
            INHandle safeHdc = new NHandle();
            //if (m_hdcs.ContainsKey(window))
            //    safeHdc = m_hdcs[window];
            //else
                safeHdc = CreateHdc(window);

            var cloneHdc = CreateCompatibleDC(safeHdc.Pointer);
            CheckOperation(cloneHdc != IntPtr.Zero);
            return (NHandle)cloneHdc;
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            foreach (var hdc in m_hdcs)
            {
                ReleaseDC(hdc.Key.Handle.Pointer, hdc.Value.Pointer);
            }

            base.DisposeUnmanagedResources();
        }
    }
}
