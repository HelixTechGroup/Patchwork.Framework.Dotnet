#region Usings
using System;
using System.Drawing;
using Shin.Framework.ComponentModel;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public abstract class NWindowRenderer : NRenderer, INWindowRenderer
    {
        #region Events
        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<double>> DpiScalingChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<NWindowMode>> ModeChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<NWindowMode>> ModeChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<double>> OpacityChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<NWindowState>> StateChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<NWindowState>> StateChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<NWindowDecorations>> SupportedDecorationsChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<NWindowDecorations>> SupportedDecorationsChanging;

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<NWindowTransparency>> TransparencySupportChanged;

        /// <inheritdoc />
        public event EventHandler<PropertyChangingEventArgs<NWindowTransparency>> TransparencySupportChanging;
        #endregion

        #region Members
        protected float m_aspectRatio;
        protected double m_dpiScaling;
        protected double m_opacity;
        protected NWindowDecorations m_supportedDecorations;
        protected int m_titlebarSize;
        protected NWindowTransparency m_transparencySupport;
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
        public double Opacity
        {
            get { return m_opacity; }
            set { m_opacity = value; }
        }

        /// <inheritdoc />
        public NWindowDecorations SupportedDecorations
        {
            get { return m_supportedDecorations; }
            set { m_supportedDecorations = value; }
        }

        /// <inheritdoc />
        public int TitlebarSize
        {
            get { return m_titlebarSize; }
        }

        /// <inheritdoc />
        public NWindowTransparency TransparencySupport
        {
            get { return m_transparencySupport; }
            set { m_transparencySupport = value; }
        }
        #endregion

        protected NWindowRenderer(INWindow window, INRenderDevice renderDevice)
        {
            m_window = window;
            m_device = renderDevice;

        }

        #region Methods
        /// <inheritdoc />
        public void EnableWindowSystemDecorations()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void DisableWindowSystemDecorations()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();
            m_window.SizeChanging += OnSizeChanging;
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            m_window.SizeChanging -= OnSizeChanging;
            base.DisposeManagedResources();
        }

        protected abstract void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e);
        #endregion
    }
}