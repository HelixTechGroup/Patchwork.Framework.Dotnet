#region Usings
using System;
using System.Drawing;
using System.Linq;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
using Shin.Framework.ComponentModel;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public abstract partial class NWindowRenderer
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
        protected double m_opacity;
        protected int m_titlebarSize;
        protected NWindowTransparency m_transparencySupport;
        #endregion

        #region Properties
        /// <inheritdoc />
        public double Opacity
        {
            get { return m_opacity; }
            set { m_opacity = value; }
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
        #endregion
    }
}