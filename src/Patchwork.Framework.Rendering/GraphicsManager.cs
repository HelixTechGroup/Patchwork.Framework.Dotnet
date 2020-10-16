///-------------------------------------------------------------------------------------------------
// <copyright file="GraphicsManager.cs" company="EmpireGaming, llc">
// Copyright (c) 2013 EmpireGaming, llc. All rights reserved.
// </copyright>
// <date>10/3/2013</date>
// <summary>Implements the graphics manager class</summary>
///-------------------------------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using UniverseSol.Framework.Service;
using UniverseSol.Framework.Service.Graphics;
using UniverseSol.Framework.Settings;
using UniverseSol.Framework.System;
using UniverseSol.Framework.System.Logging.Logger;
using UniverseSol.Framework.System.Threading;

namespace UniverseSol.Framework.Graphics
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Manager for graphics.</summary>
    ///
    /// <seealso cref="T:UniverseSol.Framework.Service.GameService{UniverseSol.Framework.Settings.GraphicsSettings}"/>
    /// <seealso cref="T:UniverseSol.Framework.Service.IGraphicsManager"/>
    /// <seealso cref="T:UniverseSol.Framework.ILoad"/>
    ///-------------------------------------------------------------------------------------------------
    public class GraphicsManager : GameService<GraphicsSettings>, IGraphicsManager, ILoad
    {
        #region Fields
        /// <summary>The sprite batch.</summary>
        private SpriteBatch m_spriteBatch = null;
        /// <summary>The device.</summary>
        private GraphicsDevice m_device;
        /// <summary>The graphics.</summary>
        private GraphicsDeviceManager m_graphics;
        /// <summary>The states.</summary>
        private DeviceStates states = new DeviceStates();
        /// <summary>The default depth.</summary>
        private float m_defaultDepth = 0.0f;
        /// <summary>The render target.</summary>
        private RenderTarget2D m_renderTarget;
        /// <summary>The color.</summary>
        private Color m_color = Color.Black;
        /// <summary>The pixel.</summary>
        private Texture2D m_pixel;
        /// <summary>The global transform.</summary>
        private Matrix m_globalTransform;
        /// <summary>Manager for resolution.</summary>
        private ResolutionManager m_resolutionManager;
        /// <summary>Manager for primitive 2 d.</summary>
        private Primitive2DManager m_primitive2DManager;
        /// <summary>Manager for texture 2 d.</summary>
        private Texture2DManager m_texture2DManager;
        /// <summary>Manager for text 2 d.</summary>
        private Text2DManager m_text2DManager;
        /// <summary>The effect.</summary>
        private Effect m_effect;
        /// <summary>The sprite sort mode.</summary>
        private SpriteSortMode m_spriteSortMode = SpriteSortMode.BackToFront;
        /// <summary>The current matrix.</summary>
        private Matrix m_currentMatrix;
        private List<DisplayMode> m_supportedDisplayModes;
        #endregion

        #region Properties
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the graphics device.</summary>
        ///
        /// <value>The graphics device.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.GraphicsDevice"/>
        ///-------------------------------------------------------------------------------------------------
        public GraphicsDevice GraphicsDevice { get { return m_device; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the manager for graphics device.</summary>
        ///
        /// <value>The graphics device manager.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.GraphicsDeviceManager"/>
        ///-------------------------------------------------------------------------------------------------
        public GraphicsDeviceManager GraphicsDeviceManager { get { return m_graphics; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the sprite batch.</summary>
        ///
        /// <value>The sprite batch.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.SpriteBatch"/>
        ///-------------------------------------------------------------------------------------------------
        public SpriteBatch SpriteBatch { get { return m_spriteBatch; } }

        public DisplayMode[] SupportedDisplayModes { get { return m_supportedDisplayModes.ToArray(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the sort mode.</summary>
        ///
        /// <value>The sort mode.</value>
        ///-------------------------------------------------------------------------------------------------
        public SpriteSortMode SortMode
        {
            get { return m_spriteSortMode; }
            set { m_spriteSortMode = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the default depth.</summary>
        ///
        /// <value>The default depth.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.DefaultDepth"/>
        ///-------------------------------------------------------------------------------------------------
        public float DefaultDepth
        {
            get { return m_defaultDepth; }
            set { m_defaultDepth = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the effect.</summary>
        ///
        /// <value>The effect.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.Effect"/>
        ///-------------------------------------------------------------------------------------------------
        public Effect Effect
        {
            get { return m_effect; }
            set { m_effect = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the render target.</summary>
        ///
        /// <value>The render target.</value>
        ///-------------------------------------------------------------------------------------------------
        public RenderTarget2D RenderTarget { get { return m_renderTarget; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the pixel.</summary>
        ///
        /// <value>The pixel.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.Pixel"/>
        ///-------------------------------------------------------------------------------------------------
        public Texture2D Pixel { get { return m_pixel; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the viewport.</summary>
        ///
        /// <value>The viewport.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.Viewport"/>
        ///-------------------------------------------------------------------------------------------------
        public Viewport Viewport { get { return m_resolutionManager.ScaleViewport; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the viewport position.</summary>
        ///
        /// <value>The viewport position.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.ViewportPosition"/>
        ///-------------------------------------------------------------------------------------------------
        public Vector2 ViewportPosition { get { return m_resolutionManager.ScaleViewportPosition; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the scale.</summary>
        ///
        /// <value>The scale.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.Scale"/>
        ///-------------------------------------------------------------------------------------------------
        public Matrix Scale { get { return m_resolutionManager.ScaleMatrix; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the scale resolution.</summary>
        ///
        /// <value>The scale resolution.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.ScaleResolution"/>
        ///-------------------------------------------------------------------------------------------------
        public Vector2 ScaleResolution { get { return m_resolutionManager.VirtualResolution; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the load status.</summary>
        ///
        /// <value>The load status.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.ILoad.LoadStatus"/>
        ///-------------------------------------------------------------------------------------------------
        public ILoadStatus LoadStatus { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the primitive 2 d.</summary>
        ///
        /// <value>The primitive 2 d.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.Primitive2D"/>
        ///-------------------------------------------------------------------------------------------------
        public IPrimitive2DManager Primitive2D { get { return m_primitive2DManager; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the texture.</summary>
        ///
        /// <value>The texture.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.Texture"/>
        ///-------------------------------------------------------------------------------------------------
        public ITexture2DManager Texture { get { return m_texture2DManager; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the text.</summary>
        ///
        /// <value>The text.</value>
        ///
        /// <seealso cref="P:UniverseSol.Framework.Service.IGraphicsManager.Text"/>
        ///-------------------------------------------------------------------------------------------------
        public IText2DManager Text { get { return m_text2DManager; } }
        #endregion

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Initializes a new instance of the GraphicsManager class.</summary>
        ///
        /// <exception cref="NullReferenceException">Thrown when a value was unexpectedly null.</exception>
        ///
        /// <param name="Settings">Options for controlling the operation.</param>
        /// <param name="graphics">The graphics.</param>
        ///-------------------------------------------------------------------------------------------------
        public GraphicsManager(GraphicsSettings Settings, GraphicsDeviceManager graphics, ILogger logger)
            : base(logger)
        {
            m_settings = Settings;

            m_graphics = graphics;

            if (m_graphics.GraphicsDevice == null)
                throw new NullReferenceException("GraphicsDevice cannot be null.");

            m_device = m_graphics.GraphicsDevice;

            m_supportedDisplayModes = new List<DisplayMode>();
            foreach (var mode in m_device.Adapter.SupportedDisplayModes)
                m_supportedDisplayModes.Add(mode);

            m_resolutionManager = new ResolutionManager();
            m_resolutionManager.Initialize(ref m_graphics);

#if ANDROID
            m_resolutionManager.SetVirtualResolution(800, 480);
#if OUYA
            m_resolutionManager.SetVirtualResolution(1280, 720);
#endif
#else
            m_resolutionManager.SetVirtualResolution(1280, 720);

            //double aspect = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //aspect = Math.Truncate(100 * aspect) / 100;

            //if (aspect == 1.77)
            //    m_resolutionManager.SetVirtualResolution(1600, 900);
            //else if (aspect == 1.6)
            //    m_resolutionManager.SetVirtualResolution(1440, 900);
            //else
            //    m_resolutionManager.SetVirtualResolution(1280, 960);

#endif

            m_primitive2DManager = new Primitive2DManager(this);
            m_texture2DManager = new Texture2DManager(this);
            m_text2DManager = new Text2DManager(this);
        }

        #region Methods
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Loads the given resource manager.</summary>
        ///
        /// <param name="resourceManager">The resource manager to load.</param>
        ///
        /// <seealso cref="M:UniverseSol.Framework.ILoad.Load(IResourceManager)"/>
        ///-------------------------------------------------------------------------------------------------
        public void Load(IResourceManager resourceManager)
        {
            m_spriteBatch = new SpriteBatch(m_device);
            //m_blankTexture = content.Load<Texture2D>("textures/ui/blank");
            m_renderTarget = new RenderTarget2D(m_device, m_device.DisplayMode.Width, m_device.DisplayMode.Height);

            m_pixel = new Texture2D(m_device, 1, 1, false, SurfaceFormat.Color);
            m_pixel.SetData(new[] { Color.White });

            float horScaling = (float)m_device.PresentationParameters.BackBufferWidth / 1280;
            float verScaling = (float)m_device.PresentationParameters.BackBufferHeight / 720;
            Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);
            m_globalTransform = Matrix.CreateScale(screenScalingFactor);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Unloads this object.</summary>
        ///
        /// <seealso cref="M:UniverseSol.Framework.ILoad.Unload()"/>
        ///-------------------------------------------------------------------------------------------------
        public void Unload()
        {
            m_renderTarget.Dispose();
            m_pixel.Dispose();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Updates the given gameTime.</summary>
        ///
        /// <param name="gameTime">The game time.</param>
        ///-------------------------------------------------------------------------------------------------
        public override void Update(GameTime gameTime) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>public void Draw()
        ///     {
        ///     m_resolutionManager.BeginDraw();
        ///     Begin();
        ///     m_texture2DManager.Draw(m_pixel, 0, 0, new Rectangle(m_device.Viewport.X,
        ///     m_device.Viewport.Y, m_device.Viewport.Width, m_device.Viewport.Height), m_color);
        ///     End();
        ///     }</summary>
        ///
        /// <seealso cref="M:UniverseSol.Framework.Service.IGraphicsManager.SetGraphicsSettings()"/>
        ///-------------------------------------------------------------------------------------------------
        public void SetGraphicsSettings()
        {
            m_resolutionManager.SetResolution((int)m_settings.Resolution.X, (int)m_settings.Resolution.Y, m_settings.FullScreen);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Begins the given view.</summary>
        ///
        /// <param name="view">The view.</param>
        ///
        /// <seealso cref="M:UniverseSol.Framework.Service.IGraphicsManager.Begin(Matrix)"/>
        ///-------------------------------------------------------------------------------------------------
        public void Begin(Matrix view)
        {
            m_currentMatrix = view;
            Begin();
        }

        /// <summary>Begins the given view.</summary>
        public void Begin()
        {
            m_resolutionManager.BeginDraw();
            m_spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, m_effect, m_currentMatrix);
            //if (mode != BlendingMode.None)
            //    m_spriteBatch.Begin(SpriteSortMode.Deferred, states.BlendState, states.SamplerState, states.DepthStencilState, states.RasterizerState, effect, m_scaleManager.getTransformationMatrix());
            //else
            //    m_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, states.SamplerState, states.DepthStencilState, states.RasterizerState, effect, m_scaleManager.getTransformationMatrix());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Ends this object.</summary>
        ///
        /// <seealso cref="M:UniverseSol.Framework.Service.IGraphicsManager.End()"/>
        ///-------------------------------------------------------------------------------------------------
        public void End()
        {
            m_spriteBatch.End();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Draw points.</summary>
        ///
        /// <param name="position"> The position.</param>
        /// <param name="points">   The points.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="color">    The color.</param>
        ///
        /// <seealso cref="M:UniverseSol.Framework.Service.IGraphicsManager.DrawPoints(Vector2,List{Vector2},float,Color)"/>
        ///-------------------------------------------------------------------------------------------------
        public void DrawPoints(Vector2 position, List<Vector2> points, Color color, float thickness, float depth)
        {
            if (points.Count < 2)
                return;

            for (int i = 1; i < points.Count; i++)
                m_primitive2DManager.Line.Draw(points[i - 1] + position, points[i] + position, Vector2.Zero, color, thickness, depth);
        }

        public void DrawPoints(Texture2D texture, Vector2 position, List<Vector2> points, Color color, float thickness, float depth)
        {
            if (points.Count < 2)
                return;

            for (int i = 1; i < points.Count; i++)
                m_primitive2DManager.Line.Draw(texture, points[i - 1] + position, points[i] + position, Vector2.Zero, color, thickness, depth);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets destination area.</summary>
        ///
        /// <param name="area">     The area.</param>
        /// <param name="margins">  The margins.</param>
        /// <param name="alignment">The alignment.</param>
        ///
        /// <returns>The destination area.</returns>
        ///
        /// <seealso cref="M:UniverseSol.Framework.Service.IGraphicsManager.GetDestinationArea(Rectangle,Margins,Alignment)"/>
        ///-------------------------------------------------------------------------------------------------
        public Rectangle GetDestinationArea(Rectangle area, Margins margins, Alignment alignment)
        {
            Rectangle rect = new Rectangle();

            int adj = 1;
            margins.Left += margins.Left > 0 ? adj : 0;
            margins.Top += margins.Top > 0 ? adj : 0;
            margins.Right += margins.Right > 0 ? adj : 0;
            margins.Bottom += margins.Bottom > 0 ? adj : 0;

            margins = new Margins(margins.Left, margins.Top, margins.Right, margins.Bottom);

            switch (alignment)
            {
                case Alignment.TopLeft:
                    {
                        rect = new Rectangle(area.Left + 0,
                                            area.Top + 0,
                                            margins.Left,
                                            margins.Top);
                        break;

                    }
                case Alignment.TopCenter:
                    {
                        rect = new Rectangle(area.Left + margins.Left,
                                            area.Top + 0,
                                            area.Width - margins.Left - margins.Right,
                                            margins.Top);
                        break;

                    }
                case Alignment.TopRight:
                    {
                        rect = new Rectangle(area.Left + area.Width - margins.Right,
                                            area.Top + 0,
                                            margins.Right,
                                            margins.Top);
                        break;

                    }
                case Alignment.MiddleLeft:
                    {
                        rect = new Rectangle(area.Left + 0,
                                            area.Top + margins.Top,
                                            margins.Left,
                                            area.Height - margins.Top - margins.Bottom);
                        break;
                    }
                case Alignment.MiddleCenter:
                    {
                        rect = new Rectangle(area.Left + margins.Left,
                                            area.Top + margins.Top,
                                            area.Width - margins.Left - margins.Right,
                                            area.Height - margins.Top - margins.Bottom);
                        break;
                    }
                case Alignment.MiddleRight:
                    {
                        rect = new Rectangle(area.Left + area.Width - margins.Right,
                                            area.Top + margins.Top,
                                            margins.Right,
                                            area.Height - margins.Top - margins.Bottom);
                        break;
                    }
                case Alignment.BottomLeft:
                    {
                        rect = new Rectangle(area.Left + 0,
                                            area.Top + area.Height - margins.Bottom,
                                            margins.Left,
                                            margins.Bottom);
                        break;
                    }
                case Alignment.BottomCenter:
                    {
                        rect = new Rectangle(area.Left + margins.Left,
                                            area.Top + area.Height - margins.Bottom,
                                            area.Width - margins.Left - margins.Right,
                                            margins.Bottom);
                        break;
                    }
                case Alignment.BottomRight:
                    {
                        rect = new Rectangle(area.Left + area.Width - margins.Right,
                                            area.Top + area.Height - margins.Bottom,
                                            margins.Right,
                                            margins.Bottom);
                        break;
                    }
            }

            return rect;
        }
        #endregion
    }
}
