#region Usings
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract partial class  NWindowRenderer : NRenderer, INWindowRenderer, IEquatable<NWindowRenderer>
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

            m_priority = RenderPriority.High;
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
        protected override bool CheckEnabled()
        {
            //if (!base.CheckEnabled())
            //    return false;

            if (!m_window.IsEnabled)// ^ !m_window.IsActive)
                m_isEnabled = false;
            else
                m_isEnabled = true;

            return m_isEnabled;
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
            m_device.ProcessMessage += OnProcessMessage;
            m_window.AddRenderer(this);
            m_window.SizeChanging += OnSizeChanging;
            m_window.SizeChanged += OnSizeChanged;
            InitializeResourcesShared();
            Invalidate();
        }

        partial void InitializeResourcesShared();

        protected abstract void OnSizeChanging(object sender, PropertyChangingEventArgs<Size> e);

        protected abstract void OnSizeChanged(object sender, PropertyChangedEventArgs<Size> e);

        protected virtual void OnProcessMessage(IPlatformMessage message)
        {

        }

        protected virtual void OnWindowRender()
        {

        }

        /// <inheritdoc />
        public bool Equals(NWindowRenderer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(m_window, other.m_window) && GetType() == other.GetType();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is NWindowRenderer other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(m_window, m_device);
        }

        public static bool operator ==(NWindowRenderer left, NWindowRenderer right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NWindowRenderer left, NWindowRenderer right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public bool Equals(INWindowRenderer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(m_window, (other as NWindowRenderer)?.m_window) && GetType() == other.GetType();
        }

        public static bool operator ==(NWindowRenderer left, INWindowRenderer right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NWindowRenderer left, INWindowRenderer right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(INWindowRenderer left, NWindowRenderer right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(INWindowRenderer left, NWindowRenderer right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}