#region Usings
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Patchwork.Framework.Platform.Interop.GdiPlus;
using Patchwork.Framework.Platform.Interop.GdiPlus.NativeMethods;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class GdiTextureBrush : TextureBrush<GpTexture>
    {
        internal GdiTextureBrush(GpTexture resource) : base(resource) { }

        #region Methods
        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            // NOTE: this has been known to fail in the past (cairo)
            // but it's the only way to reclaim brush related memory
            if (m_resource != IntPtr.Zero)
            {
                var status = GdiPlus.GdipDeleteBrush(m_resource);
                m_resource = IntPtr.Zero;
                GdiPlus.CheckStatus(status);
            }

            base.DisposeUnmanagedResources();
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            var attr = m_attributes == null ? IntPtr.Zero : m_attributes.NativeObject;
            var status = GpStatus.Ok;

            if (m_rectangle.IsEmpty && m_rectangleF.IsEmpty && m_attributes == null && m_mode != WrapMode.None)
            {
                var tex = m_resource;
                status = GdiPlus.GdipCreateTexture(m_image.nativeObject, m_mode, out tex);
                GdiPlus.CheckStatus(status);
                m_resource = tex;
                return;
            }

            if (!m_rectangle.IsEmpty)
            {
                if (m_attributes != null)
                {
                    status = GdiPlus.GdipCreateTextureIAI(m_image.nativeObject,
                                                          m_attributes.NativeObject,
                                                          m_rectangle.X,
                                                          m_rectangle.Y,
                                                          m_rectangle.Width,
                                                          m_rectangle.Height,
                                                          out m_resource);
                    GdiPlus.CheckStatus(status);
                    return;
                }

                if (m_mode != WrapMode.None)
                {
                    status = GdiPlus.GdipCreateTexture2I(m_image.nativeObject,
                                                         m_mode,
                                                         m_rectangle.X,
                                                         m_rectangle.Y,
                                                         m_rectangle.Width,
                                                         m_rectangle.Height,
                                                         out m_resource);

                    GdiPlus.CheckStatus(status);
                    return;
                }
            }

            if (!m_rectangleF.IsEmpty)
            {
                if (m_attributes != null)
                {
                    status = GdiPlus.GdipCreateTextureIA(m_image.nativeObject,
                                                         m_attributes.NativeObject,
                                                         m_rectangleF.X,
                                                         m_rectangleF.Y,
                                                         m_rectangleF.Width,
                                                         m_rectangleF.Height,
                                                         out m_resource);
                    GdiPlus.CheckStatus(status);
                    return;
                }

                if (m_mode != WrapMode.None)
                {
                    status = GdiPlus.GdipCreateTexture2(m_image.nativeObject,
                                                        m_mode,
                                                        m_rectangleF.X,
                                                        m_rectangleF.Y,
                                                        m_rectangleF.Width,
                                                        m_rectangleF.Height,
                                                        out m_resource);
                    GdiPlus.CheckStatus(status);
                }
            }
        }

        /// <inheritdoc />
        protected override object PlatformClone()
        {
            var status = GdiPlus.GdipCloneBrush(m_resource, out var clonePtr);
            GdiPlus.CheckStatus(status);

            return new GdiTextureBrush(clonePtr);
        }

        /// <inheritdoc />
        protected override Image PlatformGetImage()
        {
            // this check is required here as GDI+ doesn't check for it 
            if (m_resource == IntPtr.Zero)
                throw new ArgumentException("Object was disposed");

            IntPtr img;
            var status = GdiPlus.GdipGetTextureImage(m_resource, out img);
            GdiPlus.CheckStatus(status);
            return new Bitmap(img);
        }

        /// <inheritdoc />
        protected override Matrix PlatformGetTransform()
        {
            var matrix = new Matrix();
            var status = GdiPlus.GdipGetTextureTransform(m_resource, matrix.nativeMatrix);
            GdiPlus.CheckStatus(status);

            return matrix;
        }

        /// <inheritdoc />
        protected override WrapMode PlatformGetWrapMode()
        {
            var status = GdiPlus.GdipGetTextureWrapMode(m_resource, out var mode);
            GdiPlus.CheckStatus(status);
            return mode;
        }

        /// <inheritdoc />
        protected override void PlatformMultiplyTransform(Matrix matrix, MatrixOrder order)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");

            var status = GdiPlus.GdipMultiplyTextureTransform(m_resource, matrix.nativeMatrix, order);
            GdiPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformResetTransform()
        {
            var status = GdiPlus.GdipResetTextureTransform(m_resource);
            GdiPlus.CheckStatus(status);
        }

        protected override void PlatformRotateTransform(float angle, MatrixOrder order)
        {
            var status = GdiPlus.GdipRotateTextureTransform(m_resource, angle, order);
            GdiPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override void PlatformScaleTransform(float sx, float sy, MatrixOrder order)
        {
            var status = GdiPlus.GdipScaleTextureTransform(m_resource, sx, sy, order);
            GdiPlus.CheckStatus(status);
        }

        /// <inheritdoc />
        protected override Matrix PlatformSetTransform(Matrix transform)
        {
            var status = GdiPlus.GdipSetTextureTransform(m_resource, transform.NativeObject);
            GdiPlus.CheckStatus(status);
            return transform;
        }

        /// <inheritdoc />
        protected override WrapMode PlatformSetWrapMode(WrapMode mode)
        {
            var status = GdiPlus.GdipSetTextureWrapMode(m_resource, mode);
            GdiPlus.CheckStatus(status);
            return mode;
        }

        protected override void PlatformTranslateTransform(float dx, float dy, MatrixOrder order)
        {
            var status = GdiPlus.GdipTranslateTextureTransform(m_resource, dx, dy, order);
            GdiPlus.CheckStatus(status);
        }
        #endregion
    }
}