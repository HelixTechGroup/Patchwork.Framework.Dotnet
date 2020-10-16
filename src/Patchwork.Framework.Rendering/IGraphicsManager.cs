#region Header
// // ----------------------------------------------------------------------
// // filename: IGraphicsManager.cs
// // company: EmpireGaming, LLC
// // date: 10-03-2013
// // namespace: UniverseSol.Framework.Service
// // interface: IGraphicsManager : IGameService
// // summary: Interface representing a IGraphicsManager : IGameService entity.
// // legal: Copyright (c) 2017 All Right Reserved
// // ------------------------------------------------------------------------
#endregion

#region Usings
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UniverseSol.Framework.Service.Graphics;
#endregion

namespace UniverseSol.Framework.Service
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>Interface for graphics manager.</summary>
    ///
    /// <seealso cref="T:IGameService"/>
    ///-------------------------------------------------------------------------------------------------
    public interface IGraphicsManager : IGameService
    {
        #region Properties
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the default depth.</summary>
        ///
        /// <value>The default depth.</value>
        ///-------------------------------------------------------------------------------------------------
        float DefaultDepth { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the graphics device.</summary>
        ///
        /// <value>The graphics device.</value>
        ///-------------------------------------------------------------------------------------------------
        GraphicsDevice GraphicsDevice { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the pixel.</summary>
        ///
        /// <value>The pixel.</value>
        ///-------------------------------------------------------------------------------------------------
        Texture2D Pixel { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the scale.</summary>
        ///
        /// <value>The scale.</value>
        ///-------------------------------------------------------------------------------------------------
        Matrix Scale { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the scale resolution.</summary>
        ///
        /// <value>The scale resolution.</value>
        ///-------------------------------------------------------------------------------------------------
        Point ScaleResolution { get; }

        Matrix Projection { get; set; }

        Matrix View { get; set; }

        Matrix World { get; set; }

        BoundingFrustum BoundingFrustum { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the sprite batch.</summary>
        ///
        /// <value>The sprite batch.</value>
        ///-------------------------------------------------------------------------------------------------
        SpriteBatch SpriteBatch { get; }

        bool IsRendering { get; set; }
        #endregion

        #region Public Methods
        void PopRenderTarget();

        void PushRenderTarget(RenderTargetBinding renderTarget);

        void PushRenderTarget(params RenderTargetBinding[] renderTargets);

        IRenderPass CurrentRenderPass { get; }

        bool IsCurrentRenderPass<T>() where T : class, IRenderPass;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Begins the given view.</summary>
        ///
        /// <param name="view">The view.</param>
        ///-------------------------------------------------------------------------------------------------
        void Begin(Matrix view);

        /// <summary>Begins the given view.</summary>
        void Begin();        

        /// <summary>Ends this object.</summary>
        void End();        

        /// <summary>Sets graphics settings.</summary>
        void SetGraphicsSettings();
        #endregion
    }
}