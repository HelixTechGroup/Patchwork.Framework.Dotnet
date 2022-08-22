using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Methods;
using static Patchwork.Framework.Platform.Interop.Gdi32.Helpers;
using static Patchwork.Framework.Platform.Interop.Utilities;

namespace Patchwork.Framework.Platform.Rendering.Resources
{
    public class GdiSurface : NResource<IntPtr>, INRenderResource<IntPtr>
    {
        protected GdiDevice m_device;
        //protected INHandle m_handle;
        //protected string m_name;
        //protected IntPtr m_resource;
        protected Size m_size;
        protected Point m_position;
        protected INWindow m_window;

        /// <inheritdoc />
        public INHandle Handle
        {
            get { return m_handle; }
        }

        /// <inheritdoc />
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <inheritdoc />
        object INResource.Resource
        {
            get { return m_resource; }
        }

        public Size Size
        {
            get { return m_size; }
        }

        public Point Position
        {
            get { return m_position; }
        }

        public Rectangle RenderArea
        {
            get { return new Rectangle(m_position, m_size); }
        }

        protected GdiSurface(INRenderDevice device)
        {
            m_device = device as GdiDevice;
        }

        protected GdiSurface(INRenderDevice device, INWindow window) : this(device)
        {
            m_window = window;
        }

        protected GdiSurface(INRenderDevice device, int width, int height) : this(device)
        {
            m_size = new Size(width, height);
        }

        protected GdiSurface(INRenderDevice device, INWindow window, int width, int height) : this(device, window)
        {
            m_size = new Size(width, height);
        }

        protected void CreateBitmap()
        {
            using var hdc = m_device.Context.CurrentContext;
            CreateBitmap(hdc, m_size.Width, m_size.Height);
        }

        protected void CreateBitmap(int width, int height)
        {
            using var hdc = m_device.Context.CurrentContext;
            CreateBitmap(hdc, width, height);
        }

        protected void CreateBitmap(INWindow window)
        {
            using var hdc = m_device.Context[window];
            CreateBitmap(hdc, window.ClientArea.Width, window.ClientArea.Height);
        }

        protected void CreateBitmap(INWindow window, int width, int height)
        {
            using var hdc = m_device.Context[window];
            CreateBitmap(hdc, width, height);
        }

        protected void CreateBitmap(INHandle hdc, int width, int height)
        {
            if (width <= 0 || height <= 0)
                return;

            CheckOperation(hdc.Pointer != IntPtr.Zero);
            var bmpPtr = CreateCompatibleBitmap(hdc.Pointer, width, height);
            CheckOperation(bmpPtr != IntPtr.Zero);
            m_handle = new NHandle(bmpPtr);
            m_resource = bmpPtr;
            m_size = new Size(width, height);
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_handle = new NHandle();
            //var hdc = m_window is null ? m_device.Context.CurrentContext : m_device.Context[m_window];
            m_window ??= Core.Window.CurrentWindow;
            m_size = m_size.IsEmpty ? m_window.ClientSize : m_size;

            m_window.SizeChanged += (sender, args) => 
                                    { 
                                        m_size = args.CurrentValue;
                                        CreateBitmap(args.CurrentValue.Width, args.CurrentValue.Height);
                                    };
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            CheckOperation(DeleteObject(m_handle.Pointer));
            m_resource = IntPtr.Zero;

            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            //m_handle.Dispose();

            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override object PlatformClone()
        {
            return new GdiSurface(m_device, m_window);
        }

        /// <inheritdoc />
        protected override void CreateResources()
        {
            base.CreateResources();
            Initialize();
            CreateBitmap();
        }

        public void Create(INRenderDevice device, INWindow window)
        {
            m_device = device as GdiDevice;
            m_window = window;
            m_isCreated = false;
            m_isInitialized = false;
            m_isDisposed = false;
            Create();
        }
    }
}