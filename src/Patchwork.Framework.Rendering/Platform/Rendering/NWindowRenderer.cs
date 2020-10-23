#region Usings
using System;
using System.Drawing;
using System.Linq;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract partial class  NWindowRenderer : NRenderer, INWindowRenderer
    {
        #region Members
        protected float m_aspectRatio;
        protected double m_dpiScaling;
        protected NWindowDecorations m_supportedDecorations;
        protected INWindow m_window;
        #endregion

        #region Properties
        /// <inheritdoc />
        public float AspectRatio
        {
            get { return m_aspectRatio; }
        }

        /// <inheritdoc />
        public double DpiScaling
        {
            get { return m_dpiScaling; }
        }

        /// <inheritdoc />
        public NWindowDecorations SupportedDecorations
        {
            get { return m_supportedDecorations; }
            set { m_supportedDecorations = value; }
        }

        /// <inheritdoc />
        public INWindow Window
        {
            get { return m_window; }
        }
        #endregion

        protected NWindowRenderer(INRenderDevice renderDevice, INWindow window) : base(renderDevice)
        {
            Throw.IfNull(window);
            Throw.IfNull(renderDevice);
            Throw.If(!window.IsRenderable).InvalidOperationException();
            Throw.If(!renderDevice.SupportedRenderers.Contains(typeof(INWindowRenderer))).InvalidOperationException();

            m_window = window;
            m_device.ProcessMessage += OnProcessMessage;
        }

        #region Methods
        public void Initialize(INWindow window, INRenderDevice renderDevice)
        {
            Throw.IfNull(window);
            Throw.IfNull(renderDevice);
            Throw.If(!window.IsRenderable).InvalidOperationException();
            Throw.If(!renderDevice.SupportedRenderers.Contains(GetType())).InvalidOperationException();

            m_window = window;
            m_device = renderDevice;

            Initialize();
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_device.ProcessMessage -= OnProcessMessage;
            //m_window.SizeChanging -= OnSizeChanging;
            base.DisposeManagedResources();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_window.AddRenderer(this);
            m_device.ProcessMessage += OnProcessMessage;
            //m_window.SizeChanging += OnSizeChanging;
            m_window.SizeChanged += (sender, args) => { Invalidate(); };
            m_window.StateChanged += (sender, args) => { if (args.CurrentValue != NWindowState.Minimized) Invalidate(); };
            Invalidate();
        }

        protected abstract void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e);

        protected virtual void OnProcessMessage(IPlatformMessage message)
        {

        }

        protected virtual void OnWindowRender()
        {

        }
        #endregion
    }
}