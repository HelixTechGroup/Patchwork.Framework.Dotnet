using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UniverseSol.Framework.Graphics
{
    public class ResolutionManager
    {
        private GraphicsDeviceManager _Device = null;

        private int _Width = 800;
        private int _Height = 600;
        private int _VWidth = 1024;
        private int _VHeight = 768;
        private Matrix _ScaleMatrix;
        private bool _FullScreen = false;
        private bool _dirtyMatrix = true;

        private Vector2 m_scaleViewportPosition = new Vector2();
        private Viewport m_scaleViewport = new Viewport();

        public Vector2 ScaleViewportPosition { get { return m_scaleViewportPosition; } }

        public Viewport ScaleViewport { get { return m_scaleViewport; } }

        public Matrix ScaleMatrix
        {
            get
            {
                if (_dirtyMatrix)
                    RecreateScaleMatrix();

                return _ScaleMatrix;
            }
        }

        public Vector2 VirtualResolution { get { return new Vector2(_VWidth, _VHeight); } }

        public void Initialize(ref GraphicsDeviceManager device)
        {
            _Width = device.PreferredBackBufferWidth;
            _Height = device.PreferredBackBufferHeight;
            _Device = device;
            _dirtyMatrix = true;
            ApplyResolutionSettings();
        }


        public Matrix GetTransformationMatrix()
        {
            if (_dirtyMatrix) RecreateScaleMatrix();

            return _ScaleMatrix;
        }

        public void SetResolution(int Width, int Height, bool FullScreen)
        {
            _Width = Width;
            _Height = Height;

            _FullScreen = FullScreen;

            ApplyResolutionSettings();
            //ResetViewport();
        }

        public void SetVirtualResolution(int Width, int Height)
        {
            _VWidth = Width;
            _VHeight = Height;

            _dirtyMatrix = true;
        }

        private void ApplyResolutionSettings()
        {

#if XBOX360 || ANDROID
           _FullScreen = true;
#endif

            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (_FullScreen == false)
            {
                if ((_Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (_Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    _Device.PreferredBackBufferWidth = _Width;
                    _Device.PreferredBackBufferHeight = _Height;
                    _Device.IsFullScreen = _FullScreen;
                    _Device.ApplyChanges();
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set.  To do this, we will
                // iterate through the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == _Width) && (dm.Height == _Height))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        _Device.PreferredBackBufferWidth = _Width;
                        _Device.PreferredBackBufferHeight = _Height;
                        _Device.IsFullScreen = _FullScreen;
                        _Device.ApplyChanges();
                    }
                }
            }

            _dirtyMatrix = true;

            _Width = _Device.PreferredBackBufferWidth;
            _Height = _Device.PreferredBackBufferHeight;
        }

        /// <summary>
        /// Sets the device to use the draw pump
        /// Sets correct aspect ratio
        /// </summary>
        public void BeginDraw()
        {
            // Start by reseting viewport to (0,0,1,1)
            FullViewport();
            // Clear to Black
            _Device.GraphicsDevice.Clear(Color.Black);
            // Calculate Proper Viewport according to Aspect Ratio
            ResetViewport();
            // and clear that
            // This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
            //_Device.GraphicsDevice.Clear(Color.Gray);
        }

        private void RecreateScaleMatrix()
        {
            _dirtyMatrix = false;
            Matrix.CreateScale(
                           (float)_Width / _VWidth,
                           (float)_Width / _VWidth,
                           1f, out _ScaleMatrix);
        }


        public void FullViewport()
        {
            Viewport vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = _Width;
            vp.Height = _Height;
            _Device.GraphicsDevice.Viewport = vp;
        }

        /// <summary>
        /// Get virtual Mode Aspect Ratio
        /// </summary>
        /// <returns>aspect ratio</returns>
        public float GetVirtualAspectRatio()
        {
            return (float)_VWidth / (float)_VHeight;
        }

        public void ResetViewport()
        {
            float targetAspectRatio = GetVirtualAspectRatio();
            // figure out the largest area that fits in this resolution at the desired aspect ratio
            int width = _Width;
            int height = (int)(width / targetAspectRatio + .5f);
            bool changed = false;

            if (height > _Height)
            {
                height = _Height;
                // PillarBox
                width = (int)(height * targetAspectRatio + .5f);
                changed = true;
            }

            // set up the new viewport centered in the backbuffer
            Viewport viewport = new Viewport();

            m_scaleViewportPosition.X = viewport.X = (_Width / 2) - (width / 2);
            m_scaleViewportPosition.Y = viewport.Y = (_Height / 2) - (height / 2);
            viewport.Width = width;
            viewport.Height = height;
            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            if (changed)
            {
                _dirtyMatrix = true;
            }

            _Device.GraphicsDevice.Viewport = m_scaleViewport = viewport;
        }

    }
}
